using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IJornadasAnotadorService
{
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPuestosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarPuestosAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarMovimientosStockAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarRecoleccionesCajasResumenAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarStockAnotadorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarProduccionDiaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> CorregirCajasRecoleccionAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarRecoleccionesAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
}
