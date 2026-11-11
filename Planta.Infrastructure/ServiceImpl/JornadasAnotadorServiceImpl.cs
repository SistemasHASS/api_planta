using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class JornadasAnotadorServiceImpl(IJornadasAnotadorRepository repository) : IJornadasAnotadorService
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.ListarAsync(contexto, json, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarAsync(contexto, json, cancellationToken);

    /// <summary>El SP devuelve las líneas manuales y, con el parametro Flujo_Empaque activo, los curos de las máquinas (lo decide el SP, no el backend).</summary>
    public Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarLineasAsync(contexto, cancellationToken);

    public Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarLineasAsync(contexto, json, cancellationToken);

    public Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.BuscarTrabajadorAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarPuestosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarPuestosAsync(contexto, cancellationToken);

    public Task<JsonElement> SincronizarPuestosAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarPuestosAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.ListarTiposEmpaquePorMatrizAsync(contexto, json, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarRecoleccionesCajasAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarRecoleccionesAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.ListarRecoleccionesCajasHistorialAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasResumenAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarRecoleccionesCajasResumenAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarStockAnotadorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarStockAnotadorAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarProduccionDiaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarProduccionDiaAsync(contexto, cancellationToken);

    public Task<JsonElement> CorregirCajasRecoleccionAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.CorregirCajasRecoleccionAsync(contexto, json, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarRecoleccionesAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarMovimientosStockAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.ListarMovimientosStockAsync(contexto, json, cancellationToken);
}
