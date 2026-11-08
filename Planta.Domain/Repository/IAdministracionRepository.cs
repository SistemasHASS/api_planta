using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>Listados de administracion que el operador descarga al dispositivo (matrices, reglas de sobrepeso, correlativos).</summary>
public interface IAdministracionRepository
{
    Task<JsonElement> ListarMatricesCompatibilidadAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarReglasSobrepesoAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCorrelativosDocumentosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerParametroAsync(ContextoUsuario contexto, string idParametro, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCodigosCajaDetalleAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    /// <summary>idsJson = arreglo JSON con los id de las combinaciones a exportar (las que se ven en la tabla).</summary>
    Task<JsonElement> ExportarCodigosCajaDetalleAsync(ContextoUsuario contexto, string idsJson, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAcopioConfigAsync(ContextoUsuario contexto, SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default);
    /// <summary>jsonUsuarios = lista de usuarios de la apimaestra (texto JSON); el SP les agrega el acopio y la serie.</summary>
    Task<JsonElement> ListarUsuariosAcopioAsync(string jsonUsuarios, CancellationToken cancellationToken = default);
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
}
