using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*JornadasAnotador.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class JornadasAnotadorRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IJornadasAnotadorRepository
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarJornadasAnotadorRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotador", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@soloAbierta", request.SoloAbierta);
        }, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotador", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
        }, cancellationToken);

    public Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorLineas", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarJornadasAnotadorLineasConMaquinas.sql (solo líneas de máquinas, con curos y puestos por curo).</summary>
    public Task<JsonElement> ListarLineasConMaquinasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorLineasConMaquinas", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_GetParametro.sql (parametro activo de la empresa por nombre).</summary>
    public Task<JsonElement> ObtenerParametroAsync(ContextoUsuario contexto, string idParametro, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_GetParametro", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idparametro", idParametro);
        }, cancellationToken);

    public Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarJornadasAnotadorLineas", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
        }, cancellationToken);

    public Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string numeroDocumento, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_BuscarTrabajador", command =>
        {
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@numeroDocumento", numeroDocumento);
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
        }, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string codigoCultivo, int formatoId, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTiposEmpaquePorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", codigoCultivo);
            Agregar(command, "@formatoId", formatoId);
        }, cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarRecoleccionesCajas", command =>
        {
            Agregar(command, "@Json", json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@rol", contexto.Rol);
        }, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarJornadasAnotadorRecolecciones", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, int idRecoleccion, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarRecoleccionesCajasHistorial", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@idRecoleccion", idRecoleccion);
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
        }, cancellationToken);
}
