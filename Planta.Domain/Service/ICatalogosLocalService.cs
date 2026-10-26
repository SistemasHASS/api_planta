using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

/// <summary>Catalogos de la base de datos propia. Destinatarios, consignatarios, variedades y acopios se combinan con lo que devuelve apimaestra.</summary>
public interface ICatalogosLocalService
{
    Task<JsonElement> ListarGrupoClienteAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, ContextoUsuario contexto, string codigoCultivo, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDestinatariosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarConsignatariosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarVariedadesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAcopiosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerEstablecimientoEmisorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarEstablecimientosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default);
}
