using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Palets (cada palet agrupa cajas segun sus combinaciones) y la matriz de compatibilidad que guia su armado.
/// Devuelve la respuesta del SP: { error, data, mensaje }. Empresa, RUC, usuario, rol y acopio salen del token. Requiere token.
/// </summary>
[ApiController]
[Route("api/palets")]
public sealed class PaletsController(IPaletsUseCase palets, IWebHostEnvironment entorno) : ControllerBase
{
    /// <summary>Palets de un proceso.</summary>
    [HttpPost("por-proceso")]
    public async Task<IActionResult> PorProceso([FromBody] PaletsPorProcesoRequest request, CancellationToken ct)
        => Ok(await palets.ListarPorProcesoAsync(request, ct));

    /// <summary>Crea, edita o cierra palets (el cuerpo trae el JSON de palets que lee el SP).</summary>
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] SincronizarPaletsRequest request, CancellationToken ct)
        => Ok(await palets.SincronizarAsync(request, ct));

    [HttpPost("sincronizar-dpalets")]
    public async Task<IActionResult> SincronizarDPalets([FromBody] SincronizarDPaletsRequest request, CancellationToken ct)
        => Ok(await palets.SincronizarDPaletsAsync(request, ct));

    /// <summary>Detalle de todos los palets del acopio del usuario.</summary>
    [HttpPost("dpalets-por-acopio")]
    public async Task<IActionResult> DPaletsPorAcopio(CancellationToken ct)
        => Ok(await palets.ListarDPaletsPorAcopioAsync(ct));

    [HttpPost("dpalets-por-palet")]
    public async Task<IActionResult> DPaletsPorPalet([FromBody] PaletRequest request, CancellationToken ct)
        => Ok(await palets.ListarDPaletsPorPaletAsync(request, ct));

    [HttpPost("codigos-rancho-por-lugar")]
    public async Task<IActionResult> CodigosRanchoPorLugar([FromBody] CodigosRanchoPorLugarRequest request, CancellationToken ct)
        => Ok(await palets.ListarCodigosRanchoPorLugarAsync(request, ct));

    /// <summary>PDF con la composicion del palet.</summary>
    [HttpPost("ficha-composicion")]
    public async Task<IActionResult> FichaComposicion([FromBody] PaletRequest request, CancellationToken ct)
    {
        var archivo = await palets.GenerarFichaComposicionAsync(request, entorno.WebRootPath, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    // Matriz de compatibilidad: cada consulta devuelve las opciones validas del siguiente paso.

    [HttpPost("matriz/destinos")]
    public async Task<IActionResult> MatrizDestinos([FromBody] DestinosPorMatrizRequest request, CancellationToken ct)
        => Ok(await palets.ListarDestinosPorMatrizAsync(request, ct));

    [HttpPost("matriz/formatos")]
    public async Task<IActionResult> MatrizFormatos([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ListarFormatosPorMatrizAsync(request, ct));

    [HttpPost("matriz/tipos-empaque-guia")]
    public async Task<IActionResult> MatrizTiposEmpaqueGuia([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ListarTiposEmpaqueGuiaPorMatrizAsync(request, ct));

    [HttpPost("matriz/presentaciones")]
    public async Task<IActionResult> MatrizPresentaciones([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ListarPresentacionesPorMatrizAsync(request, ct));

    [HttpPost("matriz/tipos-caja")]
    public async Task<IActionResult> MatrizTiposCaja([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ListarTiposCajaPorMatrizAsync(request, ct));

    [HttpPost("matriz/tipos-clamshell")]
    public async Task<IActionResult> MatrizTiposClamshell([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ListarTiposClamshellPorMatrizAsync(request, ct));

    /// <summary>Calibre (texto) y tipo de empaque de la matriz para la combinacion completa elegida en el modal de cajas.</summary>
    [HttpPost("matriz/calibre-empaque")]
    public async Task<IActionResult> MatrizCalibreEmpaque([FromBody] MatrizCompatibilidadRequest request, CancellationToken ct)
        => Ok(await palets.ObtenerCalibreEmpaquePorMatrizAsync(request, ct));
}
