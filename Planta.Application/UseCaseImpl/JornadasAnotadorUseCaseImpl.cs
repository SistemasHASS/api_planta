using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>
/// Toma los datos del usuario del token y delega: el JSON del cliente se pasa al SP sin convertirlo en objetos ni validarlo aquí
/// (el SP valida y convierte a filas). Lo que sale del token (acopio, nombre, usuario, rol) viaja como parámetros.
/// </summary>
public sealed class JornadasAnotadorUseCaseImpl(IJornadasAnotadorService jornadas, ICurrentUser currentUser) : IJornadasAnotadorUseCase
{
    public Task<JsonElement> ListarAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.ListarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> SincronizarAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.SincronizarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> ListarLineasAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarLineasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> SincronizarLineasAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.SincronizarLineasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> BuscarTrabajadorAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.BuscarTrabajadorAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), json, cancellationToken);

    public Task<JsonElement> ListarPuestosAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarPuestosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> SincronizarPuestosAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.SincronizarPuestosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.ListarTiposEmpaquePorMatrizAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), json, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.SincronizarRecoleccionesCajasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarRecoleccionesAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.ListarRecoleccionesCajasHistorialAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> ListarMovimientosStockAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.ListarMovimientosStockAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasResumenAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarRecoleccionesCajasResumenAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> ListarStockAnotadorAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarStockAnotadorAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> ListarProduccionDiaAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarProduccionDiaAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> CorregirCajasRecoleccionAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.CorregirCajasRecoleccionAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesAsync(string json, CancellationToken cancellationToken = default)
        => jornadas.SincronizarRecoleccionesAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), json, cancellationToken);

}
