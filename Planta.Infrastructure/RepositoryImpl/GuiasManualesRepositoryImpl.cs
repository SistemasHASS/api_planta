using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*GuiaRemisionManual*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class GuiasManualesRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IGuiasManualesRepository
{
    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_GuardarGuiaRemisionManual", command => AgregarConJson(command, contexto, idProyecto, jsonGuias), cancellationToken);

    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarGuiasRemisionManual", command => AgregarFiltros(command, contexto, filtros), cancellationToken);

    public Task<JsonElement> ListarParaExcelAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarGuiasRemisionManualExcel", command => AgregarFiltros(command, contexto, filtros), cancellationToken);

    public Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_GetGuiaRemisionManual", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
        }, cancellationToken);

    public Task<JsonElement> EditarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_EditarGuiaRemisionManual", command => AgregarConJson(command, contexto, idProyecto, jsonGuias), cancellationToken);

    public Task<JsonElement> EliminarAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_EliminarGuiaRemisionManual", command =>
        {
            AgregarGuia(command, contexto, idProyecto, codigoGuiaRemision);
            Agregar(command, "@idRol", contexto.Rol);
        }, cancellationToken);

    public Task<JsonElement> EmitirAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_EmitirGuiaRemisionManual", command => AgregarGuia(command, contexto, idProyecto, codigoGuiaRemision), cancellationToken);

    public Task<JsonElement> ActualizarEstadoSunatAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoEstadoSunat, EnvioDocumentoElectronico? envio, bool enviadoBizlinks, string respuestaBizlinks, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ActualizarEstadoSunatGuiaRemisionManual", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@codigoGuiaRemision", codigoGuiaRemision);
            Agregar(command, "@codigoEstadoSunat", codigoEstadoSunat);
            Agregar(command, "@estadoSunat", string.Empty);
            Agregar(command, "@pdfFileUrl", envio?.PdfFileUrl);
            Agregar(command, "@xmlFileSignUrl", envio?.XmlFileSignUrl);
            Agregar(command, "@xmlFileSunatUrl", envio?.XmlFileSunatUrl);
            Agregar(command, "@enviadoBizlinks", enviadoBizlinks);
            Agregar(command, "@respuestaBizlinks", respuestaBizlinks);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    public Task<JsonElement> AnularAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoAcopio, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_AnularGuiaRemisionManual", command =>
        {
            AgregarGuia(command, contexto with { CodigoAcopio = codigoAcopio }, idProyecto, codigoGuiaRemision);
        }, cancellationToken);

    private static void AgregarConJson(System.Data.IDbCommand command, ContextoUsuario contexto, string idProyecto, string jsonGuias)
    {
        Agregar(command, "@idempresa", contexto.IdEmpresa);
        Agregar(command, "@ruc", contexto.Ruc);
        Agregar(command, "@idProyecto", idProyecto);
        Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
        Agregar(command, "@usuario", contexto.Usuario);
        Agregar(command, "@idRol", contexto.Rol);
        Agregar(command, "@Json", jsonGuias);
    }

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
