using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Listados de administracion que el dispositivo descarga. Empresa y RUC salen del token.</summary>
public interface IAdministracionUseCase
{
    Task<JsonElement> ListarMatricesCompatibilidadAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarReglasSobrepesoAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCorrelativosDocumentosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerParametroAsync(ParametroRequest request, CancellationToken cancellationToken = default);
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

    /// <summary>Líneas (activas e inactivas) de un acopio.</summary>
    Task<JsonElement> ListarAcopioLineasAsync(ListarAcopioLineasRequest request, CancellationToken cancellationToken = default);
    /// <summary>Crea o actualiza líneas de un acopio (incluye activar/desactivar).</summary>
    Task<JsonElement> SincronizarAcopioLineasAsync(SincronizarAcopioLineasRequest request, CancellationToken cancellationToken = default);

    /// <summary>Máquinas (activas e inactivas) de un acopio.</summary>
    Task<JsonElement> ListarAcopioMaquinasAsync(ListarAcopioMaquinasRequest request, CancellationToken cancellationToken = default);
    /// <summary>Crea o actualiza máquinas de un acopio (incluye activar/desactivar).</summary>
    Task<JsonElement> SincronizarAcopioMaquinasAsync(SincronizarAcopioMaquinasRequest request, CancellationToken cancellationToken = default);
}
