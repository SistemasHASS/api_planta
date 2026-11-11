using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>Reportes (SP PLANTA_Reporte*). Cada metodo llama a un SP y devuelve su respuesta { error, data, mensaje }.</summary>
public interface IReportesRepository
{
    /// <summary>Reporte diario de produccion (PLANTA_ReporteDiario).</summary>
    Task<JsonElement> ReporteDiarioAsync(ContextoUsuario contexto, ReporteDiarioRequest request, CancellationToken cancellationToken = default);

    /// <summary>Opciones de los filtros del reporte semanal (PLANTA_ReporteSemanal_Filtros).</summary>
    Task<JsonElement> ReporteSemanalFiltrosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);

    /// <summary>Datos del reporte semanal (PLANTA_ReporteSemanal_Datos).</summary>
    Task<JsonElement> ReporteSemanalAsync(ContextoUsuario contexto, ReporteSemanalParametros parametros, CancellationToken cancellationToken = default);

    /// <summary>Datos del reporte de campania (PLANTA_ReporteCampania_Datos). Usa los mismos parametros que el semanal.</summary>
    Task<JsonElement> ReporteCampaniaAsync(ContextoUsuario contexto, ReporteSemanalParametros parametros, CancellationToken cancellationToken = default);

    /// <summary>Parametros activos de la empresa (PLANTA_ListarParametros).</summary>
    Task<JsonElement> ListarParametrosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>KG de materia prima ingresada a los acopios (PLANTA_ReporteKgIngresadosAcopio).</summary>
    Task<JsonElement> ReporteKgAcopioAsync(ContextoUsuario contexto, ReporteKgAcopioRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reporte diario separado por origen BH (propio) / CAO (externo) (PLANTA_ReporteDiario_PropioExterno).</summary>
    Task<JsonElement> ReporteDiarioPropioExternoAsync(ContextoUsuario contexto, ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Avance por grupo y supervisor (PLANTA_ReporteAvanceGrupo).</summary>
    Task<JsonElement> ReporteAvanceGrupoAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Grupos y cosechadores con avance en el rango de fechas (PLANTA_ReporteAvanceCosechador_Filtros).</summary>
    Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ContextoUsuario contexto, ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Avance por cosechador: jarra blanca, jarra verde y descarte (PLANTA_ReporteAvanceCosechador).</summary>
    Task<JsonElement> ReporteAvanceCosechadorAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los procesos de una fecha para sacar su planilla de empaque (PLANTA_ListarProcesosPlanillaEmpaque).</summary>
    Task<JsonElement> ProcesosPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueProcesosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los anotadores de un proceso (PLANTA_ListarAnotadoresPlanillaEmpaque).</summary>
    Task<JsonElement> AnotadoresPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueAnotadoresRequest request, CancellationToken cancellationToken = default);

    /// <summary>Las cajas de cada empacador por recolección (PLANTA_ListarPlanillaEmpaqueProduccion).</summary>
    Task<JsonElement> PlanillaEmpaqueProduccionAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, CancellationToken cancellationToken = default);

    /// <summary>Los empacadores que cada anotador asignó a un puesto (PLANTA_ListarPlanillaEmpaqueTareo).</summary>
    Task<JsonElement> PlanillaEmpaqueTareoAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, CancellationToken cancellationToken = default);
}
