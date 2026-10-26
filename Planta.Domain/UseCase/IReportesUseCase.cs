using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Reportes del administrador. Empresa y RUC salen del token.</summary>
public interface IReportesUseCase
{
    /// <summary>Reporte diario de produccion de una campania (KPIs, por variedad, por acopio y por consignatario).</summary>
    Task<JsonElement> ReporteDiarioAsync(ReporteDiarioRequest request, CancellationToken cancellationToken = default);

    /// <summary>Opciones de los filtros del reporte semanal de una campania.</summary>
    Task<JsonElement> ReporteSemanalFiltrosAsync(ProyectoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reporte semanal: peso por dia, por tipo de empaque, por variedad y Driscoll's vs publicas.</summary>
    Task<JsonElement> ReporteSemanalAsync(ReporteSemanalRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reporte de campania: peso acumulado por semana, por tipo de empaque, por variedad y Driscoll's vs publicas.</summary>
    Task<JsonElement> ReporteCampaniaAsync(ReporteSemanalRequest request, CancellationToken cancellationToken = default);

    /// <summary>RUC de la empresa externa que el usuario puede consultar en KG ingresados (null si no tiene).</summary>
    Task<JsonElement> ReporteKgAcopioEmpresaExternaAsync(CancellationToken cancellationToken = default);

    /// <summary>Campañas de una empresa (la propia o la externa autorizada) para el reporte de KG ingresados.</summary>
    Task<JsonElement> ReporteKgAcopioCampaniasAsync(RucRequest request, CancellationToken cancellationToken = default);

    /// <summary>KG de materia prima ingresada a los acopios, con destaraje.</summary>
    Task<JsonElement> ReporteKgAcopioAsync(ReporteKgAcopioRequest request, CancellationToken cancellationToken = default);

    /// <summary>Indica si el reporte BH / CAO esta habilitado para la empresa del usuario: { habilitado }.</summary>
    Task<JsonElement> ReporteBhHabilitadoAsync(CancellationToken cancellationToken = default);

    /// <summary>Reporte diario separado por origen BH (propio) / CAO (externo).</summary>
    Task<JsonElement> ReporteDiarioPropioExternoAsync(ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default);
}
