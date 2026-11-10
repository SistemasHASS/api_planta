using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Listados de administracion que el dispositivo descarga para trabajar sin conexion.
/// Devuelve la respuesta del SP: { error, data, mensaje }. Empresa y RUC salen del token. Requiere token.
/// </summary>
[ApiController]
[Route("api/administracion")]
public sealed class AdministracionController(IAdministracionUseCase administracion) : ControllerBase
{
    [HttpPost("matrices-compatibilidad")]
    public async Task<IActionResult> MatricesCompatibilidad([FromBody] ListarAdministracionRequest request, CancellationToken ct)
        => Ok(await administracion.ListarMatricesCompatibilidadAsync(request, ct));

    [HttpPost("reglas-sobrepeso")]
    public async Task<IActionResult> ReglasSobrepeso([FromBody] ListarAdministracionRequest request, CancellationToken ct)
        => Ok(await administracion.ListarReglasSobrepesoAsync(request, ct));

    [HttpPost("correlativos-documentos")]
    public async Task<IActionResult> CorrelativosDocumentos(CancellationToken ct)
        => Ok(await administracion.ListarCorrelativosDocumentosAsync(ct));

    /// <summary>Parametro activo de la empresa por nombre (PLANTA_Parametros). El dispositivo lo guarda al guardar Parametros.</summary>
    [HttpPost("parametro")]
    public async Task<IActionResult> Parametro([FromBody] ParametroRequest request, CancellationToken ct)
        => Ok(await administracion.ObtenerParametroAsync(request, ct));

    /// <summary>Reporte "Códigos caja": combinaciones de PLANTA_CodigosCaja de la empresa con el nombre de cada código y la descripción completa.</summary>
    [HttpPost("codigos-caja")]
    public async Task<IActionResult> CodigosCaja(CancellationToken ct)
        => Ok(await administracion.ListarCodigosCajaAsync(ct));

    /// <summary>Excel de códigos caja con solo las filas que la persona ve en la tabla (solo administrador).</summary>
    [HttpPost("codigos-caja/exportar-excel")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ExportarCodigosCaja([FromBody] ExportarCodigosCajaRequest request, CancellationToken ct)
    {
        var archivo = await administracion.ExportarCodigosCajaExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Guarda la serie de guia y los tipos de proceso empacado de uno o varios acopios.</summary>
    [HttpPost("acopios-config/sincronizar")]
    public async Task<IActionResult> SincronizarAcopioConfig([FromBody] SincronizarAcopioConfigRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarAcopioConfigAsync(request, ct));

    /// <summary>Usuarios de la aplicacion con su acopio y serie de guia asignados.</summary>
    [HttpPost("usuarios/listar")]
    public async Task<IActionResult> ListarUsuarios(CancellationToken ct)
        => Ok(await administracion.ListarUsuariosAcopioAsync(ct));

    /// <summary>Asigna acopio y serie de guia a un usuario.</summary>
    [HttpPost("usuarios/sincronizar")]
    public async Task<IActionResult> SincronizarUsuarios([FromBody] SincronizarUsuariosRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarUsuariosAsync(request, ct));

    /// <summary>Crea o actualiza (incluye activar o desactivar) destinatarios.</summary>
    [HttpPost("destinatarios/sincronizar")]
    public async Task<IActionResult> SincronizarDestinatarios([FromBody] SincronizarDestinatariosRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarDestinatariosAsync(request, ct));

    /// <summary>Asigna el grupo cliente y los códigos rancho de un consignatario.</summary>
    [HttpPost("consignatarios/sincronizar")]
    public async Task<IActionResult> SincronizarConsignatarios([FromBody] SincronizarConsignatariosRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarConsignatariosAsync(request, ct));

    /// <summary>Crea variedades de ensayo o edita su nombre y la procedencia de cualquier variedad.</summary>
    [HttpPost("variedades/sincronizar")]
    public async Task<IActionResult> SincronizarVariedades([FromBody] SincronizarVariedadesRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarVariedadesAsync(request, ct));

    /// <summary>Crea una serie de guia (GRE) o la activa/desactiva.</summary>
    [HttpPost("correlativos-documentos/sincronizar")]
    public async Task<IActionResult> SincronizarCorrelativosDocumentos([FromBody] SincronizarCorrelativosRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarCorrelativosDocumentosAsync(request, ct));

    /// <summary>Crea o actualiza (incluye activar o desactivar) conductores, vehículos, transportistas, supervisores o personal de logística.</summary>
    [HttpPost("catalogos-operativos/sincronizar")]
    public async Task<IActionResult> SincronizarCatalogoOperativo([FromBody] SincronizarCatalogoOperativoRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarCatalogoOperativoAsync(request, ct));

    /// <summary>Crea o actualiza reglas de sobrepeso (incluye activar o desactivar).</summary>
    [HttpPost("reglas-sobrepeso/sincronizar")]
    public async Task<IActionResult> SincronizarReglasSobrepeso([FromBody] SincronizarReglasSobrepesoRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarReglasSobrepesoAsync(request, ct));

    /// <summary>Crea o actualiza combinaciones de la matriz de compatibilidad (incluye activar o desactivar).</summary>
    [HttpPost("matrices-compatibilidad/sincronizar")]
    public async Task<IActionResult> SincronizarMatricesCompatibilidad([FromBody] SincronizarMatricesCompatibilidadRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarMatricesCompatibilidadAsync(request, ct));

    /// <summary>Líneas (activas e inactivas) de un acopio (solo administrador).</summary>
    [HttpPost("acopio-lineas/listar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ListarAcopioLineas([FromBody] ListarAcopioLineasRequest request, CancellationToken ct)
        => Ok(await administracion.ListarAcopioLineasAsync(request, ct));

    /// <summary>Crea o actualiza líneas de un acopio (incluye activar/desactivar). Solo administrador.</summary>
    [HttpPost("acopio-lineas/sincronizar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SincronizarAcopioLineas([FromBody] SincronizarAcopioLineasRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarAcopioLineasAsync(request, ct));

    /// <summary>Máquinas (activas e inactivas) de un acopio. Solo administrador.</summary>
    [HttpPost("acopio-maquinas/listar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ListarAcopioMaquinas([FromBody] ListarAcopioMaquinasRequest request, CancellationToken ct)
        => Ok(await administracion.ListarAcopioMaquinasAsync(request, ct));

    /// <summary>Crea o actualiza máquinas de un acopio (incluye activar/desactivar). Solo administrador.</summary>
    [HttpPost("acopio-maquinas/sincronizar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SincronizarAcopioMaquinas([FromBody] SincronizarAcopioMaquinasRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarAcopioMaquinasAsync(request, ct));

    /// <summary>Kilos de pago por formato y tipo de empaque (con el peso real del formato). Solo administrador y con Flujo_Empaque activo.</summary>
    [HttpPost("kilos-pago/listar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ListarKilosPago(CancellationToken ct)
        => Ok(await administracion.ListarKilosPagoAsync(ct));

    /// <summary>Guarda los kilos de pago por formato y tipo de empaque. Solo administrador y con Flujo_Empaque activo.</summary>
    [HttpPost("kilos-pago/sincronizar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SincronizarKilosPago([FromBody] SincronizarKilosPagoRequest request, CancellationToken ct)
        => Ok(await administracion.SincronizarKilosPagoAsync(request, ct));

    /// <summary>Excel de control interno en pantalla: cajas y kilos de pago por jornada, empacador y producto. Solo administrador y con Flujo_Empaque activo.</summary>
    [HttpPost("control-interno/listar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ListarControlInterno([FromBody] ControlInternoRequest request, CancellationToken ct)
        => Ok(await administracion.ListarControlInternoAsync(request, ct));

    /// <summary>El mismo informe como archivo Excel. Solo administrador y con Flujo_Empaque activo.</summary>
    [HttpPost("control-interno/exportar-excel")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ExportarControlInterno([FromBody] ControlInternoRequest request, CancellationToken ct)
    {
        var archivo = await administracion.ExportarControlInternoExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Historial de kilos de los empacadores en un día: cajas, kilos de pago, kilos reales y meta de tarea. Administrador (todos los acopios) y anotador (su acopio); con Flujo_Empaque activo.</summary>
    [HttpPost("historial-kilos/listar")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Anotador}")]
    public async Task<IActionResult> ListarHistorialKilos([FromBody] HistorialKilosRequest request, CancellationToken ct)
        => Ok(await administracion.ListarHistorialKilosAsync(request, ct));

    /// <summary>Historial de kilos de todos los anotadores y sus empacadores en un día (solo administrador y con Flujo_Empaque activo).</summary>
    [HttpPost("historial-kilos-admin/listar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ListarHistorialKilosAdmin([FromBody] HistorialKilosRequest request, CancellationToken ct)
        => Ok(await administracion.ListarHistorialKilosAdminAsync(request, ct));
}
