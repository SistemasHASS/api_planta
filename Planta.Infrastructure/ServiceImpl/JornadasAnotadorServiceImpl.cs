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

    public Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarLineasAsync(contexto, cancellationToken);

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

    public Task<JsonElement> SincronizarRecoleccionesAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => repository.SincronizarRecoleccionesAsync(contexto, json, cancellationToken);
}
