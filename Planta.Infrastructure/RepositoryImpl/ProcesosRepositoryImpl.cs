using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_*Proceso*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class ProcesosRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IProcesosRepository
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarProcesosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarProcesos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@fechaDesde", request.FechaDesde);
            Agregar(command, "@fechaHasta", request.FechaHasta);
            Agregar(command, "@turno", request.Turno);
            Agregar(command, "@estado", request.Estado);
        }, cancellationToken);

    public Task<JsonElement> ListarTodosAsync(ContextoUsuario contexto, ListarProcesosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarProcesosTodos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@fechaDesde", request.FechaDesde);
            Agregar(command, "@fechaHasta", request.FechaHasta);
            Agregar(command, "@turno", request.Turno);
            Agregar(command, "@codigoAcopio", request.CodigoAcopio);
            Agregar(command, "@estado", request.Estado);
        }, cancellationToken);

    public Task<JsonElement> BuscarAsync(ContextoUsuario contexto, BuscarProcesoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_BuscarProceso", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@turno", request.Turno);
            Agregar(command, "@fecha", request.Fecha);
        }, cancellationToken);

    public Task<JsonElement> ListarSupervisoresDisponiblesAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarSupervisoresDisponibles", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idproyecto", request.IdProyecto);
            Agregar(command, "@fecha", request.Fecha);
        }, cancellationToken);

    public Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarPersonalLogisticaDisponibles", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idproyecto", request.IdProyecto);
            Agregar(command, "@fecha", request.Fecha);
        }, cancellationToken);

    public Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarTipoProcesoEmpacadoPorAcopio", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@idProyecto", idProyecto);
        }, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarProcesoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarProceso", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", contexto.CodigoAcopio);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@modo", request.Modo);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
            Agregar(command, "@jsonProceso", request.JsonProceso);
            Agregar(command, "@jsonDprocesoLogisticos", request.JsonDprocesoLogisticos);
            Agregar(command, "@jsonDprocesoSupervisores", request.JsonDprocesoSupervisores);
        }, cancellationToken);
}
