using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class PaletsServiceImpl(IPaletsRepository repository, IMaestrosService maestros, IPdfService pdf) : IPaletsService
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public Task<JsonElement> ListarPorProcesoAsync(ContextoUsuario contexto, string idProceso, CancellationToken cancellationToken = default)
        => repository.ListarPorProcesoAsync(contexto, idProceso, cancellationToken);

    public Task<JsonElement> ListarStockCajasPorProcesoAsync(ContextoUsuario contexto, string idProceso, CancellationToken cancellationToken = default)
        => repository.ListarStockCajasPorProcesoAsync(contexto, idProceso, cancellationToken);

    /// <summary>Con Flujo_Empaque activo usa PLANTA_SincronizarPaletsConStock (eliminar un palet devuelve sus cajas al stock); sin el parametro, PLANTA_SincronizarPalets de siempre.</summary>
    public async Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string jsonPalets, CancellationToken cancellationToken = default)
    {
        var (activo, errorParametro) = await LeerFlujoEmpaqueAsync(contexto, cancellationToken);
        if (errorParametro is { } error)
        {
            return error;
        }

        return activo
            ? await repository.SincronizarConStockAsync(contexto, jsonPalets, cancellationToken)
            : await repository.SincronizarAsync(contexto, jsonPalets, cancellationToken);
    }

    /// <summary>
    /// Parametro Flujo_Empaque activo (valor SI) -> PLANTA_SincronizarDPaletsConStock (valida y descuenta el stock de cajas).
    /// Sin el parametro -> PLANTA_SincronizarDPalets de siempre. Si no se puede leer el parametro no se guarda nada y se devuelve su error.
    /// </summary>
    public async Task<JsonElement> SincronizarDPaletsAsync(ContextoUsuario contexto, string jsonDPalets, CancellationToken cancellationToken = default)
    {
        var (activo, errorParametro) = await LeerFlujoEmpaqueAsync(contexto, cancellationToken);
        if (errorParametro is { } error)
        {
            return error;
        }

        return activo
            ? await repository.SincronizarDPaletsConStockAsync(contexto, jsonDPalets, cancellationToken)
            : await repository.SincronizarDPaletsAsync(contexto, jsonDPalets, cancellationToken);
    }

    /// <summary>Lee el parametro Flujo_Empaque. Si no se puede leer devuelve su error (no se guarda nada); si no, si esta activo (valor SI).</summary>
    private async Task<(bool Activo, JsonElement? Error)> LeerFlujoEmpaqueAsync(ContextoUsuario contexto, CancellationToken cancellationToken)
    {
        var parametro = await repository.ObtenerParametroAsync(contexto, "Flujo_Empaque", cancellationToken);
        if (parametro.ValueKind == JsonValueKind.Object && parametro.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            return (false, parametro);
        }

        return (FlujoEmpaqueActivo(parametro), null);
    }

    private static bool FlujoEmpaqueActivo(JsonElement sobre)
    {
        if (sobre.ValueKind != JsonValueKind.Object || !sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        foreach (var fila in data.EnumerateArray())
        {
            var valor = fila.TryGetProperty("valor", out var v) && v.ValueKind == JsonValueKind.String ? v.GetString()?.Trim() : null;
            var activo = !fila.TryGetProperty("activo", out var a) || a.ValueKind is JsonValueKind.True or JsonValueKind.Null
                         || (a.ValueKind == JsonValueKind.Number && a.GetInt32() != 0);
            if (activo && string.Equals(valor, "SI", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public Task<JsonElement> ListarDPaletsPorAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarDPaletsPorAcopioAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarDPaletsPorPaletAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default)
        => repository.ListarDPaletsPorPaletAsync(contexto, idPalet, cancellationToken);

    public Task<JsonElement> ListarCodigosRanchoPorLugarAsync(ContextoUsuario contexto, CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default)
        => repository.ListarCodigosRanchoPorLugarAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarDestinosPorMatrizAsync(ContextoUsuario contexto, DestinosPorMatrizRequest request, CancellationToken cancellationToken = default)
        => repository.ListarDestinosPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarFormatosPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ListarFormatosPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ListarTiposEmpaqueGuiaPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarPresentacionesPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ListarPresentacionesPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarTiposCajaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ListarTiposCajaPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarTiposClamshellPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ListarTiposClamshellPorMatrizAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.ObtenerCalibreEmpaquePorMatrizAsync(contexto, request, cancellationToken);

    public async Task<ArchivoGenerado> GenerarFichaComposicionAsync(ContextoUsuario contexto, string idPalet, string? webRootPath, CancellationToken cancellationToken = default)
    {
        // 1. Datos del palet (SP). Si el SP informa error, se muestra su mensaje.
        var respuesta = await repository.ObtenerDatosFichaComposicionAsync(contexto, idPalet, cancellationToken);
        if (respuesta.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            var mensaje = respuesta.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
            throw new BadRequestException(mensaje ?? "No se pudo obtener la ficha del palet.");
        }

        if (!respuesta.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object)
        {
            throw new NotFoundException("No se encontraron datos para el palet especificado.");
        }

        var ficha = data.Deserialize<FichaComposicionPalet>(JsonOptions)
            ?? throw new BadRequestException("Los datos de la ficha no tienen el formato esperado.");

        // 2. Los nombres (clientes, paises, variedades, empresa, acopio) viven en apimaestra.
        await CompletarNombresAsync(ficha, contexto, cancellationToken);

        // 3. PDF
        var bytes = pdf.GenerarFichaComposicionPalet(ficha, webRootPath);
        return new ArchivoGenerado(bytes, "application/pdf", $"FichaComposicion_{idPalet}.pdf");
    }

    private async Task CompletarNombresAsync(FichaComposicionPalet ficha, ContextoUsuario contexto, CancellationToken ct)
    {
        var clientes = await LeerAsync<ClienteExterno>(MaestroExterno.Clientes, contexto, ct);
        foreach (var cliente in ficha.Cabecera.Clientes.Concat(ficha.Detalle.SelectMany(d => d.Clientes)))
        {
            var documento = cliente.Documento?.Trim() ?? "";
            var encontrado = clientes.FirstOrDefault(c => Coincide(c.Documento, documento) || Coincide(c.DocumentoFiscal, documento));
            if (encontrado is not null)
            {
                cliente.Cliente = encontrado.Nombre;
            }
        }

        var paises = await LeerAsync<PaisExterno>(MaestroExterno.Paises, contexto, ct);
        foreach (var destino in ficha.Cabecera.Destinos)
        {
            var encontrado = paises.FirstOrDefault(p => p.Id == destino.DestinoId);
            if (encontrado is not null)
            {
                destino.Destino = encontrado.Pais;
            }
        }

        var variedades = await LeerAsync<VariedadExterna>(MaestroExterno.Variedades, contexto, ct);
        foreach (var variedad in ficha.Detalle.SelectMany(d => d.Variedad))
        {
            var encontrada = variedades.FirstOrDefault(v => v.IdVariedad == variedad.VariedadId && v.IdCultivo == variedad.CodigoCultivo);
            if (encontrada is not null)
            {
                variedad.Variedad = encontrada.Variedad;
            }
        }

        var empresas = await LeerAsync<EmpresaExterna>(MaestroExterno.Empresas, contexto, ct);
        var empresa = empresas.FirstOrDefault(e => e.Ruc == ficha.Cabecera.Ruc);
        if (empresa is not null)
        {
            ficha.Cabecera.RazonSocial = empresa.RazonSocial;
        }

        var acopios = await LeerAsync<AcopiosExterno>(MaestroExterno.Acopios, contexto, ct);
        var acopio = acopios.FirstOrDefault(a => a.codigo_acopio == ficha.Cabecera.PlantaDeEmpaque);
        if (acopio is not null)
        {
            ficha.Cabecera.PlantaDeEmpaque = acopio.Acopio;
        }
    }

    private async Task<List<T>> LeerAsync<T>(MaestroExterno maestro, ContextoUsuario contexto, CancellationToken ct)
    {
        var json = await maestros.ConsultarAsync(maestro, contexto.IdEmpresa, contexto.Ruc, contexto.Usuario, formatoSp: false, cancellationToken: ct);
        return json.Deserialize<List<T>>(JsonOptions) ?? [];
    }

    private static bool Coincide(string? a, string b)
        => !string.IsNullOrWhiteSpace(a) && a.Trim().Equals(b, StringComparison.OrdinalIgnoreCase);
}
