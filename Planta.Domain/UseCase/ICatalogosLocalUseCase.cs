using System.Text.Json;
using Planta.Domain.Constants;

namespace Planta.Domain.UseCase;

/// <summary>Catalogos de la base de datos propia. Empresa y RUC salen del token.</summary>
public interface ICatalogosLocalUseCase
{
    Task<JsonElement> ListarGrupoClienteAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, string codigoCultivo, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDestinatariosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarConsignatariosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarVariedadesAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAcopiosAsync(string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerEstablecimientoEmisorAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarEstablecimientosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default);
}
