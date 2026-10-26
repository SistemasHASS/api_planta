using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IPaletsService
{
    Task<JsonElement> ListarPorProcesoAsync(ContextoUsuario contexto, string idProceso, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string jsonPalets, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarDPaletsAsync(ContextoUsuario contexto, string jsonDPalets, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorPaletAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCodigosRanchoPorLugarAsync(ContextoUsuario contexto, CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default);

    /// <summary>Arma el PDF de la ficha de composicion: datos del SP + nombres de clientes, paises, variedades, empresa y acopio desde apimaestra.</summary>
    Task<ArchivoGenerado> GenerarFichaComposicionAsync(ContextoUsuario contexto, string idPalet, string? webRootPath, CancellationToken cancellationToken = default);

    Task<JsonElement> ListarDestinosPorMatrizAsync(ContextoUsuario contexto, DestinosPorMatrizRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarFormatosPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPresentacionesPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposCajaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposClamshellPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
}
