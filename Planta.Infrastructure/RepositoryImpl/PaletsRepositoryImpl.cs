using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*Palet*.sql y PLANTA_*PorMatriz*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class PaletsRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IPaletsRepository
{
    public Task<JsonElement> ListarPorProcesoAsync(ContextoUsuario contexto, string idProceso, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarPaletsForProceso", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idproceso", idProceso);
        }, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string jsonPalets, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarPalets", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@Json", jsonPalets);
        }, cancellationToken);

    public Task<JsonElement> SincronizarDPaletsAsync(ContextoUsuario contexto, string jsonDPalets, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarDPalets", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@json", jsonDPalets);
        }, cancellationToken);

    public Task<JsonElement> ListarDPaletsPorAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarDPalets_Por_Acopio", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio ?? "");
        }, cancellationToken);

    public Task<JsonElement> ListarDPaletsPorPaletAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarDPalets_Por_Palet", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idPalet", idPalet);
        }, cancellationToken);

    public Task<JsonElement> ListarCodigosRanchoPorLugarAsync(ContextoUsuario contexto, CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarCodigosRanchoPorLugarProduccion", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idproyecto", request.IdProyecto);
            Agregar(command, "@idLugaresDeProduccion", request.IdLugaresDeProduccion);
        }, cancellationToken);

    public Task<JsonElement> ObtenerDatosFichaComposicionAsync(ContextoUsuario contexto, string idPalet, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_PDF_FichaComposicionPalet", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idPalet", idPalet);
        }, cancellationToken);

    public Task<JsonElement> ListarDestinosPorMatrizAsync(ContextoUsuario contexto, DestinosPorMatrizRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarDestinos_Por_MatrizCompatibilidad", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
        }, cancellationToken);

    public Task<JsonElement> ListarFormatosPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarFormatosPorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
        }, cancellationToken);

    public Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTiposEmpaqueGuiaPorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
            Agregar(command, "@formatoId", request.FormatoId);
        }, cancellationToken);

    public Task<JsonElement> ListarPresentacionesPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarPresentacionesPorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
            Agregar(command, "@formatoId", request.FormatoId);
            Agregar(command, "@tipoEmpaqueGuiaId", request.TipoEmpaqueGuiaId);
        }, cancellationToken);

    public Task<JsonElement> ListarTiposCajaPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTiposCajaPorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
            Agregar(command, "@formatoId", request.FormatoId);
            Agregar(command, "@tipoEmpaqueGuiaId", request.TipoEmpaqueGuiaId);
            Agregar(command, "@presentacionId", request.PresentacionId);
        }, cancellationToken);

    public Task<JsonElement> ListarTiposClamshellPorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTiposClamshellPorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
            Agregar(command, "@formatoId", request.FormatoId);
            Agregar(command, "@tipoEmpaqueGuiaId", request.TipoEmpaqueGuiaId);
            Agregar(command, "@tipoCajaId", request.TipoCajaId);
            Agregar(command, "@presentacionId", request.PresentacionId);
        }, cancellationToken);

    public Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(ContextoUsuario contexto, MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ObtenerCalibreEmpaquePorMatriz", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@documentoConsignatario", request.DocumentoConsignatario);
            Agregar(command, "@destinoId", request.DestinoId);
            Agregar(command, "@formatoId", request.FormatoId);
            Agregar(command, "@tipoEmpaqueGuiaId", request.TipoEmpaqueGuiaId);
            Agregar(command, "@tipoCajaId", request.TipoCajaId);
            Agregar(command, "@tipoClamshellId", request.TipoClamshellId);
            Agregar(command, "@presentacionId", request.PresentacionId);
        }, cancellationToken);
}
