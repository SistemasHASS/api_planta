using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IReportesService
{
    Task<JsonElement> ReporteDiarioAsync(ContextoUsuario contexto, ReporteDiarioRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ReporteSemanalFiltrosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);

    /// <summary>Las listas vacias se envian como null: el SP lo entiende como "sin filtro".</summary>
    Task<JsonElement> ReporteSemanalAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reporte de campania: mismos filtros que el semanal; las listas vacias se envian como null.</summary>
    Task<JsonElement> ReporteCampaniaAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default);

    /// <summary>RUC de la empresa externa cuyo reporte de KG ingresados puede ver el usuario (parametro REPORTE_KG_INGRESADOS_EXTERNO), o null.</summary>
    Task<string?> RucExternoKgAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Campañas de la empresa propia o de la externa autorizada.</summary>
    Task<JsonElement> CampaniasKgAcopioAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken = default);

    /// <summary>Reporte de KG ingresados. Solo permite el RUC propio o el externo autorizado.</summary>
    Task<JsonElement> ReporteKgAcopioAsync(ContextoUsuario contexto, ReporteKgAcopioRequest request, CancellationToken cancellationToken = default);

    /// <summary>true si la empresa tiene activo el parametro REPORTE_BH (habilita el menu y el reporte BH / CAO).</summary>
    Task<bool> ReporteBhHabilitadoAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    Task<JsonElement> ReporteDiarioPropioExternoAsync(ContextoUsuario contexto, ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default);
}
