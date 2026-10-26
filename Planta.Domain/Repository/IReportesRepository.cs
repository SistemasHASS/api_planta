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
}
