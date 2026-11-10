using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IAdministracionService
{
    Task<JsonElement> ListarMatricesCompatibilidadAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarReglasSobrepesoAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCorrelativosDocumentosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerParametroAsync(ContextoUsuario contexto, string idParametro, CancellationToken cancellationToken = default);
    /// <summary>Reporte "Códigos caja": el SP trae los nombres de la base propia y aquí se completan los de apimaestra y catálogos.</summary>
    Task<JsonElement> ListarCodigosCajaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    /// <summary>Excel de códigos caja con solo las filas cuyo id viene en la solicitud.</summary>
    Task<ArchivoGenerado> ExportarCodigosCajaExcelAsync(ContextoUsuario contexto, ExportarCodigosCajaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAcopioConfigAsync(ContextoUsuario contexto, SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarUsuariosAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarUsuariosAsync(ContextoUsuario contexto, SincronizarUsuariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarDestinatariosAsync(ContextoUsuario contexto, SincronizarDestinatariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarConsignatariosAsync(ContextoUsuario contexto, SincronizarConsignatariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarVariedadesAsync(ContextoUsuario contexto, SincronizarVariedadesRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarCorrelativosDocumentosAsync(ContextoUsuario contexto, SincronizarCorrelativosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarCatalogoOperativoAsync(ContextoUsuario contexto, SincronizarCatalogoOperativoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarReglasSobrepesoAsync(ContextoUsuario contexto, SincronizarReglasSobrepesoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarMatricesCompatibilidadAsync(ContextoUsuario contexto, SincronizarMatricesCompatibilidadRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> ListarAcopioLineasAsync(ContextoUsuario contexto, ListarAcopioLineasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAcopioLineasAsync(ContextoUsuario contexto, SincronizarAcopioLineasRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> ListarAcopioMaquinasAsync(ContextoUsuario contexto, ListarAcopioMaquinasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAcopioMaquinasAsync(ContextoUsuario contexto, SincronizarAcopioMaquinasRequest request, CancellationToken cancellationToken = default);


    Task<JsonElement> ListarKilosPagoAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarKilosPagoAsync(ContextoUsuario contexto, SincronizarKilosPagoRequest request, CancellationToken cancellationToken = default);
    /// <summary>Excel de control interno en pantalla: cajas y kilos de pago por jornada, empacador y producto (solo con Flujo_Empaque activo).</summary>
    Task<JsonElement> ListarControlInternoAsync(ContextoUsuario contexto, ControlInternoRequest request, CancellationToken cancellationToken = default);
    /// <summary>El mismo informe como archivo Excel.</summary>
    Task<ArchivoGenerado> ExportarControlInternoExcelAsync(ContextoUsuario contexto, ControlInternoRequest request, CancellationToken cancellationToken = default);
    /// <summary>Historial de kilos de los empacadores en un día (solo con Flujo_Empaque activo). El anotador solo ve su acopio.</summary>
    Task<JsonElement> ListarHistorialKilosAsync(ContextoUsuario contexto, HistorialKilosRequest request, CancellationToken cancellationToken = default);
    /// <summary>Historial de kilos de todos los anotadores y sus empacadores en un día (administrador, con Flujo_Empaque activo).</summary>
    Task<JsonElement> ListarHistorialKilosAdminAsync(ContextoUsuario contexto, HistorialKilosRequest request, CancellationToken cancellationToken = default);
}
