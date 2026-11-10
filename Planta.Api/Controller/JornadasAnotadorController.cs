using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Jornadas del anotador (ANPLA): abrir una jornada sobre un proceso abierto de su acopio y cerrarla.
/// Devuelve la respuesta del SP: { error, data, mensaje }. Empresa, RUC, usuario, rol y acopio salen del token.
/// </summary>
[ApiController]
[Authorize(Roles = Roles.Anotador)]
[Route("api/jornadas-anotador")]
public sealed class JornadasAnotadorController(IJornadasAnotadorUseCase jornadas) : ControllerBase
{
    /// <summary>Jornada abierta del anotador (o todas las suyas si soloAbierta = false).</summary>
    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] ListarJornadasAnotadorRequest request, CancellationToken ct)
        => Ok(await jornadas.ListarAsync(request, ct));

    /// <summary>modo "nuevo" abre la jornada; modo "editado" la cierra.</summary>
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] SincronizarJornadaAnotadorRequest request, CancellationToken ct)
        => Ok(await jornadas.SincronizarAsync(request, ct));

    /// <summary>Líneas del acopio del anotador: libres, mías u ocupadas por otro, en el proceso de su jornada abierta.</summary>
    [HttpPost("lineas/listar")]
    public async Task<IActionResult> ListarLineas(CancellationToken ct)
        => Ok(await jornadas.ListarLineasAsync(ct));

    /// <summary>modo "nuevo" toma una línea; modo "editado" la suelta.</summary>
    [HttpPost("lineas/sincronizar")]
    public async Task<IActionResult> SincronizarLineas([FromBody] SincronizarJornadaAnotadorLineaRequest request, CancellationToken ct)
        => Ok(await jornadas.SincronizarLineasAsync(request, ct));

    /// <summary>Busca un trabajador activo de la empresa por su número de documento (lo que trae el QR).</summary>
    [HttpPost("trabajador/buscar")]
    public async Task<IActionResult> BuscarTrabajador([FromBody] BuscarTrabajadorRequest request, CancellationToken ct)
        => Ok(await jornadas.BuscarTrabajadorAsync(request, ct));

    /// <summary>Empacadores asignados a los puestos de las líneas del acopio en la jornada abierta.</summary>
    [HttpPost("puestos/listar")]
    public async Task<IActionResult> ListarPuestos(CancellationToken ct)
        => Ok(await jornadas.ListarPuestosAsync(ct));

    /// <summary>modo "nuevo" asigna un empacador a un puesto (reemplaza al que estaba); "mover" lo cambia de puesto; "editado" lo quita; "vaciar" libera toda la línea.</summary>
    [HttpPost("puestos/sincronizar")]
    public async Task<IActionResult> SincronizarPuestos([FromBody] SincronizarJornadaAnotadorPuestoRequest request, CancellationToken ct)
        => Ok(await jornadas.SincronizarPuestosAsync(request, ct));

    /// <summary>Tipos de empaque que la matriz de compatibilidad asocia al formato elegido.</summary>
    [HttpPost("tipos-empaque/listar")]
    public async Task<IActionResult> ListarTiposEmpaque([FromBody] ListarTiposEmpaquePorMatrizRequest request, CancellationToken ct)
        => Ok(await jornadas.ListarTiposEmpaquePorMatrizAsync(request, ct));

    /// <summary>Recolecciones de cajas de la jornada abierta del acopio (la pantalla se queda con la abierta del usuario).</summary>
    [HttpPost("recolecciones/listar")]
    public async Task<IActionResult> ListarRecolecciones(CancellationToken ct)
        => Ok(await jornadas.ListarRecoleccionesAsync(ct));

    /// <summary>Cajas registradas por empacador en la recolección abierta del anotador (suma por empacador).</summary>
    [HttpPost("recolecciones/cajas/resumen")]
    public async Task<IActionResult> ResumenRecoleccionesCajas(CancellationToken ct)
        => Ok(await jornadas.ListarRecoleccionesCajasResumenAsync(ct));

    /// <summary>Stock actual del anotador: sus cajas que Logística aún no consumió en palets, por formato y tipo de empaque.</summary>
    [HttpPost("stock")]
    public async Task<IActionResult> StockAnotador(CancellationToken ct)
        => Ok(await jornadas.ListarStockAnotadorAsync(ct));

    /// <summary>Producción del día: cajas de todos los empacadores en el proceso de la jornada abierta, una fila por anotación.</summary>
    [HttpPost("produccion")]
    public async Task<IActionResult> ProduccionDia(CancellationToken ct)
        => Ok(await jornadas.ListarProduccionDiaAsync(ct));

    /// <summary>Corrige la cantidad de cajas de una anotación de la producción del día (no deja quitar cajas que ya están en palets).</summary>
    [HttpPost("produccion/corregir")]
    public async Task<IActionResult> CorregirCajasRecoleccion([FromBody] CorregirCajasRecoleccionRequest request, CancellationToken ct)
        => Ok(await jornadas.CorregirCajasRecoleccionAsync(request, ct));

    /// <summary>Historial de recolecciones del anotador (idRecoleccion = 0 las lista; con un id devuelve el detalle de cajas de esa recolección).</summary>
    [HttpPost("recolecciones/cajas/historial")]
    public async Task<IActionResult> HistorialRecoleccionesCajas([FromBody] ListarRecoleccionesCajasHistorialRequest request, CancellationToken ct)
        => Ok(await jornadas.ListarRecoleccionesCajasHistorialAsync(request, ct));

    /// <summary>modo "nuevo" registra las cajas de un empacador (formato, tipo de empaque y cantidad) en la recolección abierta del anotador.</summary>
    [HttpPost("recolecciones/cajas/sincronizar")]
    public async Task<IActionResult> SincronizarRecoleccionesCajas([FromBody] SincronizarRecoleccionCajaRequest request, CancellationToken ct)
        => Ok(await jornadas.SincronizarRecoleccionesCajasAsync(request, ct));

    /// <summary>modo "nuevo" inicia una recolección del anotador; "editado" cierra la recolección id (solo su dueño).</summary>
    [HttpPost("recolecciones/sincronizar")]
    public async Task<IActionResult> SincronizarRecolecciones([FromBody] SincronizarJornadaAnotadorRecoleccionRequest request, CancellationToken ct)
        => Ok(await jornadas.SincronizarRecoleccionesAsync(request, ct));
}
