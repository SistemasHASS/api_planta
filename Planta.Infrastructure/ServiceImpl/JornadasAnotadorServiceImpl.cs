using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class JornadasAnotadorServiceImpl(IJornadasAnotadorRepository repository) : IJornadasAnotadorService
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarJornadasAnotadorRequest request, CancellationToken cancellationToken = default)
        => repository.ListarAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarAsync(contexto, json, cancellationToken);

    /// <summary>Con el parametro Flujo_Empaque activo el anotador ve las máquinas (líneas de máquina con sus curos y puestos); sin el parametro, las líneas de siempre.</summary>
    public async Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var (activo, errorParametro) = FlujoEmpaqueParametro.Leer(await repository.ObtenerParametroAsync(contexto, "Flujo_Empaque", cancellationToken));
        if (errorParametro is { } error)
        {
            return error;
        }

        return activo
            ? await repository.ListarLineasConMaquinasAsync(contexto, cancellationToken)
            : await repository.ListarLineasAsync(contexto, cancellationToken);
    }

    public Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarLineasAsync(contexto, json, cancellationToken);

    public Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string numeroDocumento, CancellationToken cancellationToken = default)
        => repository.BuscarTrabajadorAsync(contexto, numeroDocumento, cancellationToken);

    public Task<JsonElement> ListarPuestosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarPuestosAsync(contexto, cancellationToken);

    public Task<JsonElement> SincronizarPuestosAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarPuestosAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string codigoCultivo, int formatoId, CancellationToken cancellationToken = default)
        => repository.ListarTiposEmpaquePorMatrizAsync(contexto, codigoCultivo, formatoId, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarRecoleccionesCajasAsync(contexto, json, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarRecoleccionesAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, int idRecoleccion, CancellationToken cancellationToken = default)
        => repository.ListarRecoleccionesCajasHistorialAsync(contexto, idRecoleccion, cancellationToken);

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
}
