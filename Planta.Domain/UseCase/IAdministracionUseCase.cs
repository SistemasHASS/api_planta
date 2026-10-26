using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Listados de administracion que el dispositivo descarga. Empresa y RUC salen del token.</summary>
public interface IAdministracionUseCase
{
    Task<JsonElement> ListarMatricesCompatibilidadAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarReglasSobrepesoAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCorrelativosDocumentosAsync(CancellationToken cancellationToken = default);
    /// <summary>Reporte "Códigos caja": combinaciones de la empresa con nombres y descripción completa.</summary>
    Task<JsonElement> ListarCodigosCajaAsync(CancellationToken cancellationToken = default);
    /// <summary>Excel de códigos caja con solo las filas indicadas (las que se ven en la tabla).</summary>
    Task<ArchivoGenerado> ExportarCodigosCajaExcelAsync(ExportarCodigosCajaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAcopioConfigAsync(SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarUsuariosAcopioAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarUsuariosAsync(SincronizarUsuariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarDestinatariosAsync(SincronizarDestinatariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarConsignatariosAsync(SincronizarConsignatariosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarVariedadesAsync(SincronizarVariedadesRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarCorrelativosDocumentosAsync(SincronizarCorrelativosRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarCatalogoOperativoAsync(SincronizarCatalogoOperativoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarReglasSobrepesoAsync(SincronizarReglasSobrepesoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarMatricesCompatibilidadAsync(SincronizarMatricesCompatibilidadRequest request, CancellationToken cancellationToken = default);
}
