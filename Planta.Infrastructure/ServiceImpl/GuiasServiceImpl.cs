using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class GuiasServiceImpl(
    IGuiasRepository repository,
    IMaestrosService maestros,
    ICatalogosLocalService catalogos,
    IDocumentosElectronicosService documentosElectronicos,
    IExcelService excel,
    ILogger<GuiasServiceImpl> logger) : IGuiasService
{
    private static readonly JsonSerializerOptions JsonWeb = new(JsonSerializerDefaults.Web);

    private readonly GuiasEnriquecedor _enriquecedor = new(maestros, catalogos);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarGuiasRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarAsync(contexto, request.IdProyecto, request.Guias, cancellationToken);

    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => repository.ListarAsync(contexto, filtros, cancellationToken);

    public Task<JsonElement> EliminarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
        => repository.EliminarAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);

    public Task<JsonElement> EditarAsync(ContextoUsuario contexto, EditarGuiaRequest request, CancellationToken cancellationToken = default)
        => repository.EditarAsync(contexto, request.IdProyecto, request.Guias, cancellationToken);

    public Task<JsonElement> AnularAsync(ContextoUsuario contexto, AnularGuiaRequest request, CancellationToken cancellationToken = default)
    {
        var acopio = string.IsNullOrWhiteSpace(request.CodigoAcopio) ? contexto.CodigoAcopio : request.CodigoAcopio;
        return repository.AnularAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, acopio, cancellationToken);
    }

    public Task<JsonElement> ListarCodigosCajaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarCodigosCajaAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarProcesosConPaletsCerradosAsync(ContextoUsuario contexto, ProcesosGuiaRequest request, CancellationToken cancellationToken = default)
        => repository.ListarProcesosConPaletsCerradosAsync(contexto, request, cancellationToken);

    // ───────── Detalle ─────────

    public async Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ObtenerAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        var guia = PrimeraGuia(sobre);
        if (guia is null)
        {
            return sobre;
        }

        var faltantes = await _enriquecedor.EnriquecerGuiaAsync(contexto, request.IdProyecto, guia, cancellationToken);
        if (faltantes.Count > 0)
        {
            return Sobre(true, $"No se encontraron los siguientes datos de catálogo: {string.Join(", ", faltantes)}", new JsonArray(guia));
        }

        return Sobre(false, MensajeDe(sobre), new JsonArray(guia));
    }

    // ───────── Emision SUNAT ─────────

    public async Task<JsonElement> EmitirAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var emision = await repository.EmitirAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        if (EsError(emision))
        {
            return emision;
        }

        // El calculo de sobrepeso es complementario: si falla no impide la emision.
        try
        {
            await repository.CalcularSobrepesoAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "No se pudo calcular el sobrepeso de la guia {Guia}.", request.CodigoGuiaRemision);
        }

        var respuestas = new List<RespuestaDocumentosElectronicos>();
        foreach (var item in GuiasDeData(emision))
        {
            var codigoGuia = GuiasEnriquecedor.Texto(item, "codigoGuiaRemision");
            if (string.IsNullOrWhiteSpace(codigoGuia))
            {
                continue;
            }

            // Cada guia emitida trae su propia empresa, proyecto y acopio.
            var idProyecto = GuiasEnriquecedor.Texto(item, "idProyecto") ?? request.IdProyecto;
            var contextoGuia = contexto with
            {
                IdEmpresa = GuiasEnriquecedor.Texto(item, "idempresa") ?? contexto.IdEmpresa,
                Ruc = GuiasEnriquecedor.Texto(item, "ruc") ?? contexto.Ruc,
                CodigoAcopio = GuiasEnriquecedor.Texto(item, "codigoAcopio") ?? contexto.CodigoAcopio,
            };

            var detalle = await ObtenerAsync(contextoGuia, new GuiaRequest { IdProyecto = idProyecto, CodigoGuiaRemision = codigoGuia }, cancellationToken);
            var guia = PrimeraGuia(detalle);
            if (guia is null)
            {
                continue;
            }

            guia.Remove("palets");
            var respuesta = await documentosElectronicos.EnviarGuiaAsync(GuiaParaDocumentosElectronicos.Convertir(guia), cancellationToken);
            var codigoEstado = CodigoEstadoSunat(respuesta);
            await ActualizarYAnularSiCorrespondeAsync(contextoGuia, idProyecto, codigoGuia, codigoEstado, respuesta, cancellationToken);
            respuestas.Add(respuesta);
        }

        var mensaje = respuestas.Count == 0 ? "No se procesaron guías." : $"Se procesaron {respuestas.Count} guía(s).";
        var hayError = respuestas.Any(r => !r.Success || r.Data?.Exito != true);
        return Sobre(hayError, mensaje, JsonSerializer.SerializeToNode(respuestas, JsonWeb));
    }

    public async Task<JsonElement> ConsultarEstadoSunatAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default)
    {
        var detalle = await ObtenerAsync(contexto, request, cancellationToken);
        if (EsError(detalle) && PrimeraGuia(detalle) is null)
        {
            return detalle;
        }

        var guia = PrimeraGuia(detalle);
        if (guia is null)
        {
            return Sobre(true, "No se encontró data de la guía de remisión.", null);
        }

        var idEmpresa = GuiasEnriquecedor.Texto(guia, "idempresa") ?? contexto.IdEmpresa;
        var serie = GuiasEnriquecedor.Texto(guia, "serie") ?? string.Empty;
        var numero = GuiasEnriquecedor.Texto(guia, "numero") ?? string.Empty;
        if (string.IsNullOrWhiteSpace(serie) || string.IsNullOrWhiteSpace(numero))
        {
            return Sobre(true, "La guía no tiene serie o número asignado.", null);
        }

        var respuesta = await documentosElectronicos.ConsultarEstadoGuiaAsync(idEmpresa, serie, numero, cancellationToken);
        await ActualizarYAnularSiCorrespondeAsync(contexto, request.IdProyecto, request.CodigoGuiaRemision, CodigoEstadoSunat(respuesta), respuesta, cancellationToken);

        var mensaje = respuesta.Data?.MensajeSunat ?? respuesta.Data?.Mensaje ?? respuesta.Message;
        var hayError = !respuesta.Success || respuesta.Data?.Exito != true;
        return Sobre(hayError, mensaje, JsonSerializer.SerializeToNode(respuesta, JsonWeb));
    }

    private async Task ActualizarYAnularSiCorrespondeAsync(ContextoUsuario contexto, string idProyecto, string codigoGuia, string codigoEstado, RespuestaDocumentosElectronicos respuesta, CancellationToken ct)
    {
        await repository.ActualizarEstadoSunatAsync(contexto, idProyecto, codigoGuia, codigoEstado, respuesta.Data, ct);

        // Rechazada por SUNAT (RC_05) o con error de envio: se anula la guia para liberar los palets.
        if (codigoEstado is "RC_05" or "ERROR")
        {
            var anulacion = await repository.AnularAsync(contexto, idProyecto, codigoGuia, contexto.CodigoAcopio, ct);
            if (EsError(anulacion))
            {
                logger.LogInformation("Anulacion automatica no aplicada para {Guia}: {Mensaje}", codigoGuia, MensajeDe(anulacion));
            }
        }
    }

    private static string CodigoEstadoSunat(RespuestaDocumentosElectronicos respuesta)
    {
        if (respuesta.Success && respuesta.Data?.Exito == true) return respuesta.Data.EstadoSunat ?? "PE_02";
        return respuesta.Data?.EstadoSunat ?? "ERROR";
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
            throw new BadRequestException("No se encontraron datos para exportar.");
        }

        var filas = await _enriquecedor.EnriquecerFilasExcelAsync(contexto, data, request.CodigoCultivo, cancellationToken);
        var bytes = excel.GenerarTabla("Guías de Remisión", JsonSerializer.SerializeToElement(filas));
        return new ArchivoGenerado(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "guias-remision.xlsx");
    }

    // ───────── Utilidades de sobres ─────────

    private static bool EsError(JsonElement sobre)
        => sobre.ValueKind == JsonValueKind.Object && sobre.TryGetProperty("error", out var e) && e.ValueKind == JsonValueKind.True;

    private static string? MensajeDe(JsonElement sobre)
        => sobre.ValueKind == JsonValueKind.Object && sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;

    /// <summary>Primera guia de la propiedad "data" (el SP la entrega como arreglo; si ya es objeto, se usa tal cual).</summary>
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

    /// <summary>Guias de "data" tras emitir. El SP puede entregarlas como arreglo o como texto con el JSON del arreglo.</summary>
    private static List<JsonObject> GuiasDeData(JsonElement sobre)
    {
        var guias = new List<JsonObject>();
        if (!sobre.TryGetProperty("data", out var data)) return guias;

        JsonNode? nodo = data.ValueKind switch
        {
            JsonValueKind.String when !string.IsNullOrWhiteSpace(data.GetString()) => JsonNode.Parse(data.GetString()!),
            JsonValueKind.Array => JsonNode.Parse(data.GetRawText()),
            _ => null,
        };

        if (nodo is JsonArray arreglo)
        {
            guias.AddRange(arreglo.OfType<JsonObject>());
        }

        return guias;
    }

    private static JsonElement Sobre(bool error, string? mensaje, JsonNode? data)
    {
        var sobre = new JsonObject { ["error"] = error, ["mensaje"] = mensaje, ["data"] = data };
        return JsonSerializer.SerializeToElement(sobre);
    }
}
