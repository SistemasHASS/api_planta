using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IAdministracionService
{
    Task<JsonElement> ListarMatricesCompatibilidadAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarReglasSobrepesoAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCorrelativosDocumentosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
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
}
