using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>
/// Palets y sus detalles (SP PLANTA_*Palet*, PLANTA_*Por*Matriz*). Cada metodo llama a un SP y devuelve su respuesta { error, data, mensaje }.
/// Las validaciones de negocio viven en el SP, que ya existia y no se modifica.
/// </summary>
public interface IPaletsRepository
{
    Task<JsonElement> ListarPorProcesoAsync(ContextoUsuario contexto, string idProceso, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string jsonPalets, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarDPaletsAsync(ContextoUsuario contexto, string jsonDPalets, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorPaletAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCodigosRanchoPorLugarAsync(ContextoUsuario contexto, CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerDatosFichaComposicionAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default);

    // Matriz de compatibilidad: cada paso filtra las opciones del siguiente.
    Task<JsonElement> ListarDestinosPorMatrizAsync(ContextoUsuario contexto, DestinosPorMatrizRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarFormatosPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPresentacionesPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposCajaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposClamshellPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    /// <summary>Calibre (texto) y tipo de empaque de la fila de la matriz que coincide con la combinacion completa.</summary>
    Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
}
