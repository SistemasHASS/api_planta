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

    /// <summary>Campañas de la empresa elegida (RUC de apimaestra) para el avance por cosechador.</summary>
    Task<JsonElement> ReporteAvanceCosechadorCampaniasAsync(RucRequest request, CancellationToken cancellationToken = default);

    /// <summary>Avance por grupo y supervisor: jarra blanca, jarra verde y descarte.</summary>
    Task<JsonElement> ReporteAvanceGrupoAsync(ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Excel de la tabla "Detalle por grupo" con los mismos filtros del reporte.</summary>
    Task<ArchivoGenerado> ExportarAvanceGrupoExcelAsync(ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Opciones de grupo y cosechador para el rango de fechas de la empresa elegida.</summary>
    Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Excel de la tabla "Detalle por cosechador" con los mismos filtros del reporte.</summary>
    Task<ArchivoGenerado> ExportarAvanceCosechadorExcelAsync(ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Avance por cosechador: jarra blanca, jarra verde y descarte por cosechador y por dia.</summary>
    Task<JsonElement> ReporteAvanceCosechadorAsync(ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los procesos de una fecha para elegir de cuál sacar la planilla de empaque (vacío en acopio = todas las plantas).</summary>
    Task<JsonElement> ProcesosPlanillaEmpaqueAsync(PlanillaEmpaqueProcesosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los anotadores de un proceso, para sacar la planilla de uno solo.</summary>
    Task<JsonElement> AnotadoresPlanillaEmpaqueAsync(PlanillaEmpaqueAnotadoresRequest request, CancellationToken cancellationToken = default);

    /// <summary>Excel de la planilla de producción del proceso (todos los anotadores o uno) con el formato de la empresa. <paramref name="webRootPath"/> es donde están los logos (assets/logos/{ruc}.jpg).</summary>
    Task<ArchivoGenerado> ExportarPlanillaProduccionExcelAsync(PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default);

    /// <summary>Excel del tareo / asistencia del proceso (todos los anotadores o uno) con el formato de la empresa.</summary>
    Task<ArchivoGenerado> ExportarPlanillaTareoExcelAsync(PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default);

    /// <summary>Los empacadores asignados a un puesto en un rango de fechas de proceso.</summary>
    Task<JsonElement> HistoricoEmpacadoresAsync(HistoricoEmpacadoresRequest request, CancellationToken cancellationToken = default);

    /// <summary>Las asignaciones de un empacador (jornada, proceso, línea o curo, puesto y cajas).</summary>
    Task<JsonElement> HistoricoEmpacadorDetalleAsync(HistoricoEmpacadorDetalleRequest request, CancellationToken cancellationToken = default);
}
