using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Flujo del anotador (ANPLA). El cuerpo llega como JSON y se manda tal cual al SP (@Json), que lo valida y lo convierte a filas: aquí no se arman objetos.
/// Empresa, RUC, usuario, rol, acopio y nombre salen del token y viajan como parámetros del SP. Devuelve la respuesta del SP: { error, data, mensaje }.
/// </summary>
[ApiController]
[Authorize(Roles = Roles.Anotador)]
[Route("api/jornadas-anotador")]
public sealed class JornadasAnotadorController(IJornadasAnotadorUseCase jornadas) : ControllerBase
{
    /// <summary>Jornada abierta del anotador. JSON: [{ "soloAbierta": true }].</summary>
    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.ListarAsync(body.GetRawText(), ct));

    /// <summary>modo "nuevo" abre la jornada; modo "editado" la cierra. JSON: [{ "id", "modo", "idProyecto", "idProceso" }].</summary>
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.SincronizarAsync(body.GetRawText(), ct));

    /// <summary>Líneas del acopio del anotador: libres, mías u ocupadas por otro, en el proceso de su jornada abierta.</summary>
    [HttpPost("lineas/listar")]
    public async Task<IActionResult> ListarLineas(CancellationToken ct)
        => Ok(await jornadas.ListarLineasAsync(ct));

    /// <summary>modo "nuevo" toma una línea; modo "editado" la suelta. JSON: [{ "id", "modo", "idLinea" | "idMaquina", "curo" }].</summary>
    [HttpPost("lineas/sincronizar")]
    public async Task<IActionResult> SincronizarLineas([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.SincronizarLineasAsync(body.GetRawText(), ct));

    /// <summary>Busca un trabajador activo de la empresa por su número de documento (lo que trae el QR). JSON: [{ "numeroDocumento" }].</summary>
    [HttpPost("trabajador/buscar")]
    public async Task<IActionResult> BuscarTrabajador([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.BuscarTrabajadorAsync(body.GetRawText(), ct));

    /// <summary>Empacadores asignados a los puestos de las líneas del acopio en la jornada abierta.</summary>
    [HttpPost("puestos/listar")]
    public async Task<IActionResult> ListarPuestos(CancellationToken ct)
        => Ok(await jornadas.ListarPuestosAsync(ct));

    /// <summary>modo "nuevo" asigna un empacador a un puesto; "mover" lo cambia de puesto; "editado" lo quita; "vaciar" libera la línea. JSON: [{ "id", "modo", "idLinea" | "idMaquina", "curo", "puesto", "numeroDocumento", "nombreCompleto", "apellidoPaterno", "apellidoMaterno" }].</summary>
    [HttpPost("puestos/sincronizar")]
    public async Task<IActionResult> SincronizarPuestos([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.SincronizarPuestosAsync(body.GetRawText(), ct));

    /// <summary>Tipos de empaque que la matriz de compatibilidad asocia al formato elegido. JSON: [{ "codigoCultivo", "formatoId" }].</summary>
    [HttpPost("tipos-empaque/listar")]
    public async Task<IActionResult> ListarTiposEmpaque([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.ListarTiposEmpaquePorMatrizAsync(body.GetRawText(), ct));

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

    /// <summary>Corrige la cantidad (y, si se envía, el formato y el tipo de empaque) de una anotación; exige la observación (motivo). JSON: [{ "id", "cantidadCajas", "formatoId", "tipoEmpaqueId", "calibreNombre", "observaciones" }].</summary>
    [HttpPost("produccion/corregir")]
    public async Task<IActionResult> CorregirCajasRecoleccion([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.CorregirCajasRecoleccionAsync(body.GetRawText(), ct));

    /// <summary>Historial de recolecciones del anotador (idRecoleccion = 0 las lista; con un id devuelve el detalle de esa recolección). JSON: [{ "idRecoleccion" }].</summary>
    [HttpPost("recolecciones/cajas/historial")]
    public async Task<IActionResult> HistorialRecoleccionesCajas([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.ListarRecoleccionesCajasHistorialAsync(body.GetRawText(), ct));

    /// <summary>Movimientos de stock de las cajas del anotador en un día (entradas, correcciones, ajustes y salidas a palet) con su resumen por tipo. JSON: [{ "fecha": "AAAA-MM-DD", "tipos": ["ENTRADA", "SALIDA_PALET"] }].</summary>
    [HttpPost("movimientos-stock/listar")]
    public async Task<IActionResult> ListarMovimientosStock([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.ListarMovimientosStockAsync(body.GetRawText(), ct));

    /// <summary>modo "nuevo" registra las cajas de un empacador; "anular" lo quita de la recolección. JSON: [{ "id", "modo", "idLinea" | "idMaquina", "curo", "puesto", "formatoId", "tipoEmpaqueId", "cantidadCajas", "calibreNombre" }].</summary>
    [HttpPost("recolecciones/cajas/sincronizar")]
    public async Task<IActionResult> SincronizarRecoleccionesCajas([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.SincronizarRecoleccionesCajasAsync(body.GetRawText(), ct));

    /// <summary>modo "nuevo" inicia una recolección del anotador; "editado" cierra la recolección id (solo su dueño). JSON: [{ "id", "modo" }].</summary>
    [HttpPost("recolecciones/sincronizar")]
    public async Task<IActionResult> SincronizarRecolecciones([FromBody] JsonElement body, CancellationToken ct)
        => Ok(await jornadas.SincronizarRecoleccionesAsync(body.GetRawText(), ct));
}
