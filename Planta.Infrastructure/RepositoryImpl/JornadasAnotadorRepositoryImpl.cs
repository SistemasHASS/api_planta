using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*JornadasAnotador.sql. Solo llama al SP: el JSON del cliente va en @Json y lo del token (empresa, ruc, usuario, rol, acopio, nombre) como parametros.</summary>
public sealed class JornadasAnotadorRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IJornadasAnotadorRepository
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotador", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@Json", json);
        }, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotador", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@nombreUsuario", contexto.NombreUsuario);
        }, cancellationToken);

    public Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorLineas", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotadorLineas", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@nombreUsuario", contexto.NombreUsuario);
        }, cancellationToken);

    public Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_BuscarTrabajador", command =>
        {
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@Json", json);
        }, cancellationToken);

    public Task<JsonElement> ListarPuestosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorPuestos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> SincronizarPuestosAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotadorPuestos", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTiposEmpaquePorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@Json", json);
        }, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarRecoleccionesCajas", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorRecolecciones", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarRecoleccionesCajasHistorial", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@Json", json);
        }, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasResumenAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarRecoleccionesCajasResumen", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarStockAnotador.sql (cajas del anotador que Logística aún no consumió en palets).</summary>
    public Task<JsonElement> ListarStockAnotadorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarStockAnotador", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarProduccionDia.sql (cajas de todos los empacadores en el proceso de la jornada abierta).</summary>
    public Task<JsonElement> ListarProduccionDiaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarProduccionDia", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_CorregirCajasRecoleccion.sql (cambia la cantidad y, si se envía, el formato y el tipo de empaque de un registro de cajas y deja los AJUSTE de stock).</summary>
    public Task<JsonElement> CorregirCajasRecoleccionAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_CorregirCajasRecoleccion", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotadorRecolecciones", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@nombreUsuario", contexto.NombreUsuario);
        }, cancellationToken);

    public Task<JsonElement> ListarMovimientosStockAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarMovimientosStock", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@Json", json);
        }, cancellationToken);
}
