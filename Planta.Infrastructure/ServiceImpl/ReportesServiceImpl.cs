using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class ReportesServiceImpl(IReportesRepository repository, IMaestrosService maestros) : IReportesService
{
    private const string ParametroRucExterno = "REPORTE_KG_INGRESADOS_EXTERNO";
    private const string ParametroReporteBh = "REPORTE_BH";

    public Task<JsonElement> ReporteDiarioAsync(ContextoUsuario contexto, ReporteDiarioRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteDiarioAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ReporteSemanalFiltrosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => repository.ReporteSemanalFiltrosAsync(contexto, idProyecto, cancellationToken);

    public Task<JsonElement> ReporteSemanalAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteSemanalAsync(contexto, new ReporteSemanalParametros(
            request.IdProyecto,
            ComoJson(request.Semanas),
            ComoJson(request.Variedades),
            ComoJson(request.Formatos),
            ComoJson(request.Destinos),
            ComoJson(request.Clientes),
            ComoJson(request.Consignatarios)), cancellationToken);

    public Task<JsonElement> ReporteCampaniaAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteCampaniaAsync(contexto, new ReporteSemanalParametros(
            request.IdProyecto,
            ComoJson(request.Semanas),
            ComoJson(request.Variedades),
            ComoJson(request.Formatos),
            ComoJson(request.Destinos),
            ComoJson(request.Clientes),
            ComoJson(request.Consignatarios)), cancellationToken);

    public async Task<string?> RucExternoKgAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarParametrosAsync(contexto, cancellationToken);
        if (!sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        foreach (var parametro in data.EnumerateArray())
        {
            var id = parametro.TryGetProperty("idparametro", out var idJson) ? idJson.GetString()?.Trim() : null;
            var valor = parametro.TryGetProperty("valor", out var valorJson) ? valorJson.GetString()?.Trim() : null;
            if (string.Equals(id, ParametroRucExterno, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(valor)
                && !string.Equals(valor, contexto.Ruc, StringComparison.OrdinalIgnoreCase))
            {
                return valor;
            }
        }

        return null;
    }

    public async Task<JsonElement> CampaniasKgAcopioAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken = default)
    {
        await ValidarRucAsync(contexto, ruc, cancellationToken);
        return await maestros.ConsultarAsync(MaestroExterno.Campanias, contexto.IdEmpresa, ruc.Trim(), null, false, cancellationToken);
    }

    public async Task<JsonElement> ReporteKgAcopioAsync(ContextoUsuario contexto, ReporteKgAcopioRequest request, CancellationToken cancellationToken = default)
    {
        await ValidarRucAsync(contexto, request.Ruc, cancellationToken);
        request.Ruc = request.Ruc.Trim();
        return await repository.ReporteKgAcopioAsync(contexto, request, cancellationToken);
    }

    public async Task<bool> ReporteBhHabilitadoAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarParametrosAsync(contexto, cancellationToken);
        if (!sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        // PLANTA_ListarParametros ya devuelve solo los parametros activos.
        return data.EnumerateArray().Any(parametro =>
            parametro.TryGetProperty("idparametro", out var id)
            && string.Equals(id.GetString()?.Trim(), ParametroReporteBh, StringComparison.OrdinalIgnoreCase));
    }

    public Task<JsonElement> ReporteDiarioPropioExternoAsync(ContextoUsuario contexto, ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteDiarioPropioExternoAsync(contexto, request, cancellationToken);

    /// <summary>Solo se puede consultar el RUC propio o el externo autorizado por parametro.</summary>
    private async Task ValidarRucAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken)
    {
        var rucPedido = ruc?.Trim() ?? "";
        if (rucPedido.Length == 0 || string.Equals(rucPedido, contexto.Ruc, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var externo = await RucExternoKgAcopioAsync(contexto, cancellationToken);
        if (!string.Equals(externo, rucPedido, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("No tiene permiso para consultar la empresa indicada.");
        }
    }

    private static string? ComoJson(List<string>? valores)
        => valores is { Count: > 0 } ? JsonSerializer.Serialize(valores) : null;
}
