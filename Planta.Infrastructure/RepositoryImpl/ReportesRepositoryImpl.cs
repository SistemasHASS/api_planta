using System.Data;
using System.Globalization;
using System.Text.Json;
using Planta.Domain.Constants;
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

    public Task<JsonElement> ReporteAvanceCosechadorAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteAvanceCosechador", command =>
        {
            Agregar(command, "@ruc", request.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@fechaDesde", request.FechaDesde);
            Agregar(command, "@fechaHasta", NuloSiVacio(request.FechaHasta));
            Agregar(command, "@dniSupervisor", NuloSiVacio(request.DniSupervisor));
            Agregar(command, "@grupos", ComoJson(request.Grupos));
            Agregar(command, "@codFundo", NuloSiVacio(request.CodFundo));
            Agregar(command, "@dniCosechadores", ComoJson(request.DniCosechadores));
        }, cancellationToken);

    public Task<JsonElement> ReporteAvanceGrupoAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteAvanceGrupo", command =>
        {
            Agregar(command, "@ruc", request.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@fechaDesde", request.FechaDesde);
            Agregar(command, "@fechaHasta", NuloSiVacio(request.FechaHasta));
            Agregar(command, "@grupos", ComoJson(request.Grupos));
            Agregar(command, "@dniSupervisores", ComoJson(request.DniSupervisores));
        }, cancellationToken);

    public Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ContextoUsuario contexto, ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ReporteAvanceCosechador_Filtros", command =>
        {
            Agregar(command, "@ruc", request.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@fechaDesde", request.FechaDesde);
            Agregar(command, "@fechaHasta", NuloSiVacio(request.FechaHasta));
            Agregar(command, "@grupos", ComoJson(request.Grupos));
        }, cancellationToken);

    /// <summary>Lista vacia = null (el SP lo entiende como "todos"); si no, arreglo JSON.</summary>
    private static string? ComoJson(List<string>? valores)
        => valores is { Count: > 0 } ? JsonSerializer.Serialize(valores.Select(v => v.Trim()).Where(v => v.Length > 0)) : null;

    /// <summary>El administrador y el monitor solo ven procesos CERRADOS en Tareo y Cajas Empaque; el anotador ve abiertos y cerrados.</summary>
    private static bool PlanillaSoloCerrados(ContextoUsuario contexto) => !string.Equals(contexto.Rol, Roles.Anotador, StringComparison.OrdinalIgnoreCase);

    /// <summary>El anotador solo trabaja con el acopio de su token; los demás usan el que piden (vacío = todas las plantas en la lista de procesos).</summary>
    private static string? PlanillaAcopio(ContextoUsuario contexto, string? pedido)
        => string.Equals(contexto.Rol, Roles.Anotador, StringComparison.OrdinalIgnoreCase) ? contexto.CodigoAcopio : pedido;

    private static string? NuloSiVacio(string? valor) => string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();

    /// <summary>SP: base-datos/sp/PLANTA_ListarProcesosPlanillaEmpaque.sql (los procesos de un rango de fechas, con sus cajas y supervisores; el SP valida Flujo_Empaque). Las fechas ya vienen validadas (AAAA-MM-DD).</summary>
    public Task<JsonElement> ProcesosPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueProcesosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarProcesosPlanillaEmpaque", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fechaDesde", DateTime.ParseExact(request.FechaDesde, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@fechaHasta", DateTime.ParseExact(request.FechaHasta, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", NuloSiVacio(PlanillaAcopio(contexto, request.CodigoAcopio)));
            Agregar(command, "@soloCerrados", PlanillaSoloCerrados(contexto), DbType.Boolean);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarAnotadoresPlanillaEmpaque.sql (los anotadores de un proceso: los que registraron cajas o tomaron una línea).</summary>
    public Task<JsonElement> AnotadoresPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueAnotadoresRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarAnotadoresPlanillaEmpaque", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", PlanillaAcopio(contexto, request.CodigoAcopio));
            Agregar(command, "@idProceso", request.IdProceso);
            Agregar(command, "@soloCerrados", PlanillaSoloCerrados(contexto), DbType.Boolean);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarPlanillaEmpaqueProduccion.sql (las cajas de cada empacador por recolección; anotador vacío = todos).</summary>
    public Task<JsonElement> PlanillaEmpaqueProduccionAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarPlanillaEmpaqueProduccion", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", PlanillaAcopio(contexto, request.CodigoAcopio));
            Agregar(command, "@idProceso", request.IdProceso);
            Agregar(command, "@idUsuarioAnotador", NuloSiVacio(request.IdUsuarioAnotador));
            Agregar(command, "@soloCerrados", PlanillaSoloCerrados(contexto), DbType.Boolean);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarPlanillaEmpaqueTareo.sql (los empacadores que cada anotador asignó a un puesto; anotador vacío = todos).</summary>
    public Task<JsonElement> PlanillaEmpaqueTareoAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarPlanillaEmpaqueTareo", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", PlanillaAcopio(contexto, request.CodigoAcopio));
            Agregar(command, "@idProceso", request.IdProceso);
            Agregar(command, "@idUsuarioAnotador", NuloSiVacio(request.IdUsuarioAnotador));
            Agregar(command, "@soloCerrados", PlanillaSoloCerrados(contexto), DbType.Boolean);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarHistoricoEmpacadores.sql (los empacadores asignados a un puesto en un rango de fechas de proceso, con sus totales). Las fechas ya vienen validadas (AAAA-MM-DD).</summary>
    public Task<JsonElement> HistoricoEmpacadoresAsync(ContextoUsuario contexto, HistoricoEmpacadoresRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarHistoricoEmpacadores", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fechaDesde", DateTime.ParseExact(request.FechaDesde, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@fechaHasta", DateTime.ParseExact(request.FechaHasta, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", NuloSiVacio(PlanillaAcopio(contexto, request.CodigoAcopio)));
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarHistoricoEmpacadorDetalle.sql (cada asignación de un empacador: jornada, proceso, línea o curo, puesto y cajas).</summary>
    public Task<JsonElement> HistoricoEmpacadorDetalleAsync(ContextoUsuario contexto, HistoricoEmpacadorDetalleRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarHistoricoEmpacadorDetalle", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@numeroDocumento", request.NumeroDocumento);
            Agregar(command, "@fechaDesde", DateTime.ParseExact(request.FechaDesde, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@fechaHasta", DateTime.ParseExact(request.FechaHasta, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", NuloSiVacio(PlanillaAcopio(contexto, request.CodigoAcopio)));
        }, cancellationToken);
}
