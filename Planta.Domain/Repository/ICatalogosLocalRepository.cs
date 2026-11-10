using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>
/// Catalogos de la base de datos propia (SP PLANTA_Listar*). Cada metodo llama a un SP y devuelve su respuesta { error, data, mensaje }.
/// Los SP ya existian y no se modifican.
/// </summary>
public interface ICatalogosLocalRepository
{
    Task<JsonElement> ListarGrupoClienteAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, ContextoUsuario contexto, string codigoCultivo, CancellationToken cancellationToken = default);

    // Los cuatro siguientes reciben tambien lo que devolvio apimaestra, como texto JSON.
    Task<JsonElement> ListarDestinatariosAsync(ContextoUsuario contexto, string jsonClientes, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarConsignatariosAsync(ContextoUsuario contexto, string jsonClientes, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarVariedadesAsync(ContextoUsuario contexto, string jsonVariedades, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAcopiosAsync(ContextoUsuario contexto, string idProyecto, string jsonAcopios, CancellationToken cancellationToken = default);
    /// <summary>Igual que ListarAcopiosAsync pero cada tipo de proceso trae sus kilos por tarea (solo con el parametro Flujo_Empaque activo).</summary>
    Task<JsonElement> ListarAcopiosConTareaAsync(ContextoUsuario contexto, string idProyecto, string jsonAcopios, CancellationToken cancellationToken = default);
    /// <summary>Parametro activo de la empresa por nombre (PLANTA_GetParametro).</summary>
    Task<JsonElement> ObtenerParametroAsync(ContextoUsuario contexto, string idParametro, CancellationToken cancellationToken = default);

    // Datos para las guias de remision
    Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerEstablecimientoEmisorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarEstablecimientosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    // Ubigeo (departamento, provincia, distrito) para los puntos de partida y llegada
    Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default);
}
