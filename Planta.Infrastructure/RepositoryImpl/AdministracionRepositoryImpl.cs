using System.Data;
using System.Globalization;
using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_Listar_*.sql. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class AdministracionRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IAdministracionRepository
{
    public Task<JsonElement> ListarMatricesCompatibilidadAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_Listar_MatrizCompatibilidad", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
        }, cancellationToken);

    public Task<JsonElement> ListarReglasSobrepesoAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_Listar_ReglasSobrepeso", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idProyecto", request.IdProyecto);
            Agregar(command, "@codigoCultivo", request.CodigoCultivo);
        }, cancellationToken);

    public Task<JsonElement> ListarCorrelativosDocumentosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarCorrelativosDocumentos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_GetParametro.sql (parametro activo de la empresa por nombre).</summary>
    public Task<JsonElement> ObtenerParametroAsync(ContextoUsuario contexto, string idParametro, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_GetParametro", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@idparametro", idParametro);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarCodigosCajaDetalle.sql (combinaciones de códigos caja con los nombres de la base propia).</summary>
    public Task<JsonElement> ListarCodigosCajaDetalleAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarCodigosCajaDetalle", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ExportarCodigosCajaDetalle.sql (solo las combinaciones cuyos id vienen en @ids, dentro de la empresa).</summary>
    public Task<JsonElement> ExportarCodigosCajaDetalleAsync(ContextoUsuario contexto, string idsJson, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ExportarCodigosCajaDetalle", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@ids", idsJson);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarAcopio_Config.sql (guarda la serie de guia y los tipos de proceso de cada acopio).</summary>
    public Task<JsonElement> SincronizarAcopioConfigAsync(ContextoUsuario contexto, SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarAcopio_Config", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@json", request.Json);
            Agregar(command, "@json_detalle", request.JsonDetalle);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarAcopio_ConfigConTarea.sql (igual que el anterior y ademas guarda los kilos por tarea de cada tipo de proceso).</summary>
    public Task<JsonElement> SincronizarAcopioConfigConTareaAsync(ContextoUsuario contexto, SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarAcopio_ConfigConTarea", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@json", request.Json);
            Agregar(command, "@json_detalle", request.JsonDetalle);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListaUsuariosAcopio.sql (agrega a cada usuario su acopio y la serie de guia).</summary>
    public Task<JsonElement> ListarUsuariosAcopioAsync(string jsonUsuarios, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListaUsuariosAcopio", command =>
        {
            Agregar(command, "@json", jsonUsuarios);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarUsuarios.sql (asigna acopio y serie a un usuario).</summary>
    public Task<JsonElement> SincronizarUsuariosAsync(ContextoUsuario contexto, SincronizarUsuariosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarUsuarios", command =>
        {
            Agregar(command, "@json", request.Json);
            Agregar(command, "@usuarioCre", contexto.Usuario);
            Agregar(command, "@IdempresaBack", contexto.IdEmpresa);
            Agregar(command, "@RucBack", contexto.Ruc);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarDestinatarios.sql (crea o actualiza el destinatario de un cliente).</summary>
    public Task<JsonElement> SincronizarDestinatariosAsync(ContextoUsuario contexto, SincronizarDestinatariosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarDestinatarios", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@idRol", contexto.Rol);
            Agregar(command, "@Json", request.Json);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarConsignatario.sql (grupo cliente y códigos rancho de un consignatario).</summary>
    public Task<JsonElement> SincronizarConsignatariosAsync(ContextoUsuario contexto, SincronizarConsignatariosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarConsignatario", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@json", request.Json);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarVariedadEs_Ensayo.sql (crea variedades de ensayo, edita su nombre o la procedencia de cualquier variedad).</summary>
    public Task<JsonElement> SincronizarVariedadesAsync(ContextoUsuario contexto, SincronizarVariedadesRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarVariedadEs_Ensayo", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
            Agregar(command, "@json", request.Json);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarCorrelativosDocumentos.sql (crea una serie de guia o la activa/desactiva).</summary>
    public Task<JsonElement> SincronizarCorrelativosDocumentosAsync(ContextoUsuario contexto, SincronizarCorrelativosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarCorrelativosDocumentos", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarCatalogo.sql (crea o actualiza, incluye activar o desactivar, un registro de una tabla operativa).</summary>
    public Task<JsonElement> SincronizarCatalogoOperativoAsync(ContextoUsuario contexto, SincronizarCatalogoOperativoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarCatalogo", command =>
        {
            Agregar(command, "@Tabla", request.Tabla.Trim());
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@Idempresa", contexto.IdEmpresa);
            Agregar(command, "@Ruc", contexto.Ruc);
            Agregar(command, "@Usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarReglasSobrepeso.sql (crea o actualiza reglas, incluye activar o desactivar).</summary>
    public Task<JsonElement> SincronizarReglasSobrepesoAsync(ContextoUsuario contexto, SincronizarReglasSobrepesoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarReglasSobrepeso", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarMatrizCompatibilidad.sql (crea o actualiza combinaciones, incluye activar o desactivar).</summary>
    public Task<JsonElement> SincronizarMatricesCompatibilidadAsync(ContextoUsuario contexto, SincronizarMatricesCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarMatrizCompatibilidad", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarAcopioLineas.sql (líneas de un acopio).</summary>
    public Task<JsonElement> ListarAcopioLineasAsync(ContextoUsuario contexto, ListarAcopioLineasRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarAcopioLineas", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", (request.CodigoAcopio ?? "").Trim());
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarAcopioLineas.sql (crea o actualiza líneas, incluye activar/desactivar).</summary>
    public Task<JsonElement> SincronizarAcopioLineasAsync(ContextoUsuario contexto, SincronizarAcopioLineasRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarAcopioLineas", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarAcopioMaquinas.sql (máquinas de un acopio).</summary>
    public Task<JsonElement> ListarAcopioMaquinasAsync(ContextoUsuario contexto, ListarAcopioMaquinasRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarAcopioMaquinas", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@codigoAcopio", (request.CodigoAcopio ?? "").Trim());
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarAcopioMaquinas.sql (crea o actualiza máquinas, incluye activar/desactivar).</summary>
    public Task<JsonElement> SincronizarAcopioMaquinasAsync(ContextoUsuario contexto, SincronizarAcopioMaquinasRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarAcopioMaquinas", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarAcopioMaquinasConLineas.sql (igual que el anterior y ademas crea o actualiza la línea de cada máquina).</summary>
    public Task<JsonElement> SincronizarAcopioMaquinasConLineasAsync(ContextoUsuario contexto, SincronizarAcopioMaquinasRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarAcopioMaquinasConLineas", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarKilosPago.sql (combinaciones de la matriz por formato, tipo de empaque y categoría, con el calibre, el peso real del formato y los kilos de pago; el SP valida Flujo_Empaque).</summary>
    public Task<JsonElement> ListarKilosPagoAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarKilosPago", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_SincronizarKilosPago.sql (crea o actualiza los kilos de pago de cada combinación; el SP valida Flujo_Empaque).</summary>
    public Task<JsonElement> SincronizarKilosPagoAsync(ContextoUsuario contexto, SincronizarKilosPagoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_SincronizarKilosPago", command =>
        {
            Agregar(command, "@Json", request.Json);
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@usuario", contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarControlInterno.sql (cajas y kilos de pago por jornada, empacador y producto; el SP valida Flujo_Empaque). Las fechas ya vienen validadas (AAAA-MM-DD).</summary>
    public Task<JsonElement> ListarControlInternoAsync(ContextoUsuario contexto, ControlInternoRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarControlInterno", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fechaDesde", DateTime.ParseExact(request.FechaDesde, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@fechaHasta", DateTime.ParseExact(request.FechaHasta, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", request.CodigoAcopio);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarHistorialKilos.sql (cajas, kilos de pago, kilos reales y meta de tarea por empacador en un día; el SP valida Flujo_Empaque). La fecha ya viene validada (AAAA-MM-DD).</summary>
    public Task<JsonElement> ListarHistorialKilosAsync(ContextoUsuario contexto, HistorialKilosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarHistorialKilos", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fecha", DateTime.ParseExact(request.Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", request.CodigoAcopio);
            // El anotador solo ve lo que él registró; el administrador, todo.
            Agregar(command, "@usuario", contexto.Rol == Roles.Admin ? string.Empty : contexto.Usuario);
        }, cancellationToken);

    /// <summary>SP: base-datos/sp/PLANTA_ListarHistorialKilosAdmin.sql (como el historial de kilos, pero de todos los anotadores: una fila por acopio + anotador + empacador).</summary>
    public Task<JsonElement> ListarHistorialKilosAdminAsync(ContextoUsuario contexto, HistorialKilosRequest request, CancellationToken cancellationToken = default)
        => EjecutarSobreAsync("PLANTA_ListarHistorialKilosAdmin", command =>
        {
            Agregar(command, "@idempresa", contexto.IdEmpresa);
            Agregar(command, "@ruc", contexto.Ruc);
            Agregar(command, "@fecha", DateTime.ParseExact(request.Fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture), DbType.Date);
            Agregar(command, "@codigoAcopio", request.CodigoAcopio);
        }, cancellationToken);
}
