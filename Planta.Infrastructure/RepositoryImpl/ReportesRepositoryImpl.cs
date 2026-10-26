using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_Reporte*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class ReportesRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IReportesRepository
{
    public Task<JsonElement> ReporteDiarioAsync(ContextoUsuario contexto, ReporteDiarioRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteDiario", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fecha", request.Fecha);
            Agregar(command, "@acopios", request.Acopios);
            Agregar(command, "@idCampana", request.IdProyecto);
        }, cancellationToken);

    public Task<JsonElement> ReporteSemanalFiltrosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteSemanal_Filtros", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", idProyecto);
        }, cancellationToken);

    public Task<JsonElement> ReporteSemanalAsync(ContextoUsuario contexto, ReporteSemanalParametros parametros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteSemanal_Datos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", parametros.IdProyecto);
            Agregar(command, "@semanas", parametros.Semanas);
            Agregar(command, "@variedades", parametros.Variedades);
            Agregar(command, "@formatos", parametros.Formatos);
            Agregar(command, "@destinos", parametros.Destinos);
            Agregar(command, "@clientes", parametros.Clientes);
            Agregar(command, "@consignatarios", parametros.Consignatarios);
        }, cancellationToken);

    public Task<JsonElement> ReporteCampaniaAsync(ContextoUsuario contexto, ReporteSemanalParametros parametros, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteCampania_Datos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", parametros.IdProyecto);
            Agregar(command, "@semanas", parametros.Semanas);
            Agregar(command, "@variedades", parametros.Variedades);
            Agregar(command, "@formatos", parametros.Formatos);
            Agregar(command, "@destinos", parametros.Destinos);
            Agregar(command, "@clientes", parametros.Clientes);
            Agregar(command, "@consignatarios", parametros.Consignatarios);
        }, cancellationToken);

    public Task<JsonElement> ListarParametrosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarParametros", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
        }, cancellationToken);

    public Task<JsonElement> ReporteKgAcopioAsync(ContextoUsuario contexto, ReporteKgAcopioRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteKgIngresadosAcopio", command =>
        {
            Agregar(command, "@ruc", request.Ruc);
            Agregar(command, "@fechaDesde", request.Fecha);
            Agregar(command, "@fechaHasta", request.Fecha);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codFundo", request.CodFundo);
            Agregar(command, "@acopios", request.Acopios);
        }, cancellationToken);

    public Task<JsonElement> ReporteDiarioPropioExternoAsync(ContextoUsuario contexto, ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteDiario_PropioExterno", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fecha", request.Fecha);
            Agregar(command, "@acopios", request.Acopios);
            Agregar(command, "@idCampana", request.IdProyecto);
            Agregar(command, "@origenes", request.Origenes);
        }, cancellationToken);
}
