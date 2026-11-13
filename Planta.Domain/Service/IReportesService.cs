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

    /// <summary>Campañas de una empresa de apimaestra (get_empresas) para el reporte de avance por cosechador.</summary>
    Task<JsonElement> CampaniasAvanceCosechadorAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken = default);

    /// <summary>Avance por grupo y supervisor. El RUC debe ser el de una empresa listada por apimaestra.</summary>
    Task<JsonElement> ReporteAvanceGrupoAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Excel con la tabla "Detalle por grupo" (mismos filtros que el reporte).</summary>
    Task<ArchivoGenerado> ExportarAvanceGrupoExcelAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Grupos y cosechadores con avance en el rango de fechas, de la empresa elegida.</summary>
    Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ContextoUsuario contexto, ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Excel con la tabla "Detalle por cosechador" (mismos filtros que el reporte).</summary>
    Task<ArchivoGenerado> ExportarAvanceCosechadorExcelAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Avance por cosechador. El RUC debe ser el de una empresa listada por apimaestra.</summary>
    Task<JsonElement> ReporteAvanceCosechadorAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los procesos de una fecha para elegir de cuál sacar la planilla de empaque (el SP valida Flujo_Empaque).</summary>
    Task<JsonElement> ProcesosPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueProcesosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los anotadores de un proceso, para sacar la planilla de uno solo.</summary>
    Task<JsonElement> AnotadoresPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueAnotadoresRequest request, CancellationToken cancellationToken = default);

    /// <summary>Planilla de producción del proceso (cajas por recolección) con el formato de la empresa. <paramref name="webRootPath"/> es donde están los logos (assets/logos/{ruc}.jpg).</summary>
    Task<ArchivoGenerado> ExportarPlanillaProduccionExcelAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default);

    /// <summary>Tareo / asistencia del proceso (los empacadores asignados, hayan anotado cajas o no) con el formato de la empresa.</summary>
    Task<ArchivoGenerado> ExportarPlanillaTareoExcelAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default);

    /// <summary>Los empacadores asignados a un puesto en un rango de fechas de proceso.</summary>
    Task<JsonElement> HistoricoEmpacadoresAsync(ContextoUsuario contexto, HistoricoEmpacadoresRequest request, CancellationToken cancellationToken = default);

    /// <summary>Las asignaciones de un empacador (jornada, proceso, línea o curo, puesto y cajas).</summary>
    Task<JsonElement> HistoricoEmpacadorDetalleAsync(ContextoUsuario contexto, HistoricoEmpacadorDetalleRequest request, CancellationToken cancellationToken = default);
}
