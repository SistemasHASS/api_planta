using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class GuiasManualesServiceImpl(
    IGuiasManualesRepository repository,
    IMaestrosService maestros,
    ICatalogosLocalService catalogos,
    IDocumentosElectronicosService documentosElectronicos,
    IExcelService excel,
    ILogger<GuiasManualesServiceImpl> logger) : IGuiasManualesService
{
    private static readonly JsonSerializerOptions JsonWeb = new(JsonSerializerDefaults.Web);

    private readonly GuiasEnriquecedor _enriquecedor = new(maestros, catalogos);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarGuiasRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarAsync(contexto, request.IdProyecto, request.Guias, cancellationToken);

    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => repository.ListarAsync(contexto, filtros, cancellationToken);

    public Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
        => repository.ObtenerAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);

    public Task<JsonElement> EditarAsync(ContextoUsuario contexto, EditarGuiaRequest request, CancellationToken cancellationToken = default)
        => repository.EditarAsync(contexto, request.IdProyecto, request.Guias, cancellationToken);

    public Task<JsonElement> EliminarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
        => repository.EliminarAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);

    public Task<JsonElement> AnularAsync(ContextoUsuario contexto, AnularGuiaRequest request, CancellationToken cancellationToken = default)
    {
        var acopio = string.IsNullOrWhiteSpace(request.CodigoAcopio) ? contexto.CodigoAcopio : request.CodigoAcopio;
        return repository.AnularAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, acopio, cancellationToken);
    }

    // ───────── Emision SUNAT ─────────

    public async Task<JsonElement> EmitirAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var emision = await repository.EmitirAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        if (EsError(emision))
        {
            return emision;
        }

        // El SP puede devolver el codigo definitivo de la guia emitida; si no, se usa el recibido.
        var codigoEmitido = request.CodigoGuiaRemision;
        if (emision.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array && data.GetArrayLength() > 0
            && JsonNode.Parse(data[0].GetRawText()) is JsonObject primera)
        {
            codigoEmitido = GuiasEnriquecedor.Texto(primera, "codigoGuiaRemision") ?? codigoEmitido;
        }

        return await EnviarASunatAsync(contexto, request.IdProyecto, codigoEmitido, null, cancellationToken);
    }

    public async Task<JsonElement> ReenviarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var detalle = await repository.ObtenerAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        var guia = PrimeraGuia(detalle);
        if (guia is null)
        {
            return detalle;
        }

        if (GuiasEnriquecedor.Texto(guia, "estado") != "CERRADA")
        {
            return Sobre(true, "La guía debe estar CERRADA para poder reenviarla.", null);
        }

        if (EsVerdadero(guia["enviadoBizlinks"]))
        {
            return Sobre(true, "La guía ya fue enviada a SUNAT.", null);
        }

        return await EnviarASunatAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, guia, cancellationToken);
    }

    public async Task<JsonElement> ConsultarEstadoSunatAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var detalle = await repository.ObtenerAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        var guia = PrimeraGuia(detalle);
        if (guia is null)
        {
            return detalle;
        }

        var serie = GuiasEnriquecedor.Texto(guia, "serie") ?? string.Empty;
        var numero = GuiasEnriquecedor.Texto(guia, "numero") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serie) || string.IsNullOrWhiteSpace(numero))
        {
            return Sobre(true, "La guía no tiene serie o número asignado.", null);
        }

        var respuesta = await documentosElectronicos.ConsultarEstadoGuiaAsync(contexto.IdEmpresa, serie, numero, cancellationToken);
        await GuardarEstadoAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, respuesta, cancellationToken);
        return RespuestaDeEnvio(respuesta);
    }

    /// <summary>Envia la guia a ApiDocumentosElectronicos y guarda el estado SUNAT devuelto. <paramref name="guiaCargada"/> evita volver a leerla.</summary>
    private async Task<JsonElement> EnviarASunatAsync(ContextoUsuario contexto, string idProyecto, string codigoGuia, JsonObject? guiaCargada, CancellationToken ct)
    {
        JsonObject guia;
        if (guiaCargada is not null)
        {
            guia = guiaCargada;
        }
        else
        {
            var detalle = await repository.ObtenerAsync(contexto, idProyecto, codigoGuia, ct);
            var leida = PrimeraGuia(detalle);
            if (leida is null)
            {
                return detalle;
            }

            guia = leida;
        }

        var documentoDestinatario = GuiasEnriquecedor.Texto(guia, "documentoDestinatario");
        if (!string.IsNullOrWhiteSpace(documentoDestinatario))
        {
            var nombre = await _enriquecedor.NombreDestinatarioAsync(contexto, documentoDestinatario, ct);
            if (nombre is not null)
            {
                guia["nombreDestinatario"] = nombre;
            }
        }

        var respuesta = await documentosElectronicos.EnviarGuiaAsync(GuiaManualParaDocumentosElectronicos.Convertir(guia)!, ct);
        await GuardarEstadoAsync(contexto, idProyecto, codigoGuia, respuesta, ct);
        return RespuestaDeEnvio(respuesta);
    }

    /// <summary>Guarda el estado SUNAT. Si SUNAT la rechazo (RC_05) se anula la guia para liberarla.</summary>
    private async Task GuardarEstadoAsync(ContextoUsuario contexto, string idProyecto, string codigoGuia, RespuestaDocumentosElectronicos respuesta, CancellationToken ct)
    {
        var exito = respuesta.Success && respuesta.Data?.Exito == true;
        var codigoEstado = exito ? respuesta.Data!.EstadoSunat ?? "PE_02" : respuesta.Data?.EstadoSunat ?? "ERROR";
        var rechazada = codigoEstado == "RC_05";
        var respuestaJson = JsonSerializer.Serialize(respuesta, JsonWeb);

        await repository.ActualizarEstadoSunatAsync(contexto, idProyecto, codigoGuia, codigoEstado, respuesta.Data, exito || rechazada, respuestaJson, ct);

        if (rechazada)
        {
            var anulacion = await repository.AnularAsync(contexto, idProyecto, codigoGuia, contexto.CodigoAcopio, ct);
            if (EsError(anulacion))
            {
                logger.LogInformation("Anulacion automatica no aplicada para {Guia}: {Mensaje}", codigoGuia, MensajeDe(anulacion));
            }
        }
    }

    private static JsonElement RespuestaDeEnvio(RespuestaDocumentosElectronicos respuesta)
    {
        var mensaje = respuesta.Data?.MensajeSunat ?? respuesta.Data?.Mensaje ?? respuesta.Message;
        var hayError = !respuesta.Success || respuesta.Data?.Exito != true;
        return Sobre(hayError, mensaje, JsonSerializer.SerializeToNode(respuesta, JsonWeb));
    }

    // ───────── Excel ─────────

    public async Task<ArchivoGenerado> ExportarExcelAsync(ContextoUsuario contexto, ExportarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarParaExcelAsync(contexto, request, cancellationToken);
        if (EsError(sobre))
        {
            throw new BadRequestException(MensajeDe(sobre) ?? "No se pudo obtener los datos para exportar.");
        }

        if (!sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array || data.GetArrayLength() == 0)
        {
            throw new BadRequestException("No se encontraron guías manuales para exportar.");
        }

        var filas = await _enriquecedor.EnriquecerFilasExcelAsync(contexto, data, request.CodigoCultivo, cancellationToken);
        var bytes = excel.GenerarTabla("Guías Manuales", JsonSerializer.SerializeToElement(filas));
        return new ArchivoGenerado(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "guias-remision-manuales.xlsx");
    }

    // ───────── Utilidades de sobres ─────────

    private static bool EsError(JsonElement sobre)
        => sobre.ValueKind == JsonValueKind.Object && sobre.TryGetProperty("error", out var e) && e.ValueKind == JsonValueKind.True;

    private static string? MensajeDe(JsonElement sobre)
        => sobre.ValueKind == JsonValueKind.Object && sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;

    /// <summary>Primera guia de "data" (arreglo u objeto). Null si el sobre es un error o no trae guia.</summary>
    private static JsonObject? PrimeraGuia(JsonElement sobre)
    {
        if (EsError(sobre) || !sobre.TryGetProperty("data", out var data)) return null;
        return data.ValueKind switch
        {
            JsonValueKind.Array when data.GetArrayLength() > 0 => JsonNode.Parse(data[0].GetRawText()) as JsonObject,
            JsonValueKind.Object => JsonNode.Parse(data.GetRawText()) as JsonObject,
            _ => null,
        };
    }

    /// <summary>El SP entrega el bit como true/false o 1/0.</summary>
    private static bool EsVerdadero(JsonNode? valor)
    {
        if (valor is not JsonValue json) return false;
        if (json.TryGetValue<bool>(out var booleano)) return booleano;
        return json.TryGetValue<int>(out var numero) && numero == 1;
    }

    private static JsonElement Sobre(bool error, string? mensaje, JsonNode? data)
    {
        var sobre = new JsonObject { ["error"] = error, ["mensaje"] = mensaje, ["data"] = data };
        return JsonSerializer.SerializeToElement(sobre);
    }
}
