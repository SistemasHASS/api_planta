using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*GuiaRemision*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class GuiasRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IGuiasRepository
{
    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarGuiasRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@Json", jsonGuias);
        }, cancellationToken);

    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarGuiasRemision", command => AgregarFiltros(command, contexto, filtros), cancellationToken);

    public Task<JsonElement> ListarParaExcelAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarGuiasRemisionExcel", command => AgregarFiltros(command, contexto, filtros), cancellationToken);

    public Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_GetGuiaRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
        }, cancellationToken);

    public Task<JsonElement> EliminarAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_DeleteGuiaRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
        }, cancellationToken);

    public Task<JsonElement> EmitirAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_EmitirGuiaRemision", command => AgregarGuia(command, contexto, idProyecto, codigoGuiaRemision), cancellationToken);

    public Task<JsonElement> CalcularSobrepesoAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_CalcularSobrepesoPorGuiaRemision", command => AgregarGuia(command, contexto, idProyecto, codigoGuiaRemision), cancellationToken);

    public Task<JsonElement> ActualizarEstadoSunatAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoEstadoSunat, EnvioDocumentoElectronico? envio, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ActualizarEstadoSunatGuiaRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
            Agregar(command, "@estado", string.Empty);
            Agregar(command, "@codigoEstadoSunat", codigoEstadoSunat);
            Agregar(command, "@estadoSunat", string.Empty);
            Agregar(command, "@pdfFileUrl", envio?.PdfFileUrl);
            Agregar(command, "@xmlFileSignUrl", envio?.XmlFileSignUrl);
            Agregar(command, "@xmlFileSunatUrl", envio?.XmlFileSunatUrl);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    public Task<JsonElement> AnularAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoAcopio, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_AnularGuiaRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", codigoAcopio);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    public Task<JsonElement> EditarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_EditarGuiaRemision", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@Json", jsonGuias);
        }, cancellationToken);

    public Task<JsonElement> ListarCodigosCajaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarCodigosCaja", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
        }, cancellationToken);

    /// <summary>Procesos abiertos del acopio que tienen palets cerrados sin guia. Este SP devuelve (error, mensaje, data).</summary>
    public Task<JsonElement> ListarProcesosConPaletsCerradosAsync(ContextoUsuario contexto, ProcesosGuiaRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarProcesosAbiertosConPaletsCerrados", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        }, cancellationToken, mensajeAntesDeData: true);

    private static void AgregarFiltros(System.Data.IDbCommand command, ContextoUsuario contexto, ListarGuiasRequest filtros)
    {
        Agregar(command, "@idempresa", contexto.IdEmpresa);
        Agregar(command, "@ruc", contexto.Ruc);
        Agregar(command, "@idProyecto", filtros.IdProyecto);
        Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        Agregar(command, "@usuario", contexto.Usuario);
        Agregar(command, "@idRol", contexto.Rol);
        Agregar(command, "@estado", filtros.Estado);
        Agregar(command, "@fechaDesde", filtros.FechaDesde);
        Agregar(command, "@fechaHasta", filtros.FechaHasta);
        Agregar(command, "@texto", filtros.Texto);
    }

    private static void AgregarGuia(System.Data.IDbCommand command, ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision)
    {
        Agregar(command, "@idempresa", contexto.IdEmpresa);
        Agregar(command, "@ruc", contexto.Ruc);
        Agregar(command, "@idProyecto", idProyecto);
        Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
        Agregar(command, "@usuario", contexto.Usuario);
    }
}
