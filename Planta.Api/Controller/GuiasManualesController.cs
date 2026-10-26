using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Guias de remision manuales (se escribe todo el detalle a mano, sin palets) y su emision ante SUNAT.
/// Devuelve la respuesta del SP: { error, data, mensaje }. Empresa, RUC, usuario, rol y acopio salen del token. Requiere token.
/// </summary>
[ApiController]
[Route("api/guias-manuales")]
public sealed class GuiasManualesController(IGuiasManualesUseCase guiasManuales) : ControllerBase
{
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] SincronizarGuiasRequest request, CancellationToken ct)
        => Ok(await guiasManuales.SincronizarAsync(request, ct));

    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] ListarGuiasRequest request, CancellationToken ct)
        => Ok(await guiasManuales.ListarAsync(request, ct));

    /// <summary>Excel con las guias manuales filtradas (solo administrador y monitor).</summary>
    [HttpPost("exportar-excel")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Monitor}")]
    public async Task<IActionResult> ExportarExcel([FromBody] ExportarGuiasRequest request, CancellationToken ct)
    {
        var archivo = await guiasManuales.ExportarExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    [HttpPost("obtener")]
    public async Task<IActionResult> Obtener([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.ObtenerAsync(request, ct));

    [HttpPost("editar")]
    public async Task<IActionResult> Editar([FromBody] EditarGuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.EditarAsync(request, ct));

    [HttpPost("eliminar")]
    public async Task<IActionResult> Eliminar([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.EliminarAsync(request, ct));

    /// <summary>Cierra la guia y la envia a SUNAT.</summary>
    [HttpPost("emitir")]
    public async Task<IActionResult> Emitir([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.EmitirAsync(request, ct));

    /// <summary>Reenvia a SUNAT una guia CERRADA que aun no fue enviada.</summary>
    [HttpPost("reenviar")]
    public async Task<IActionResult> Reenviar([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.ReenviarAsync(request, ct));

    [HttpPost("consultar-estado-sunat")]
    public async Task<IActionResult> ConsultarEstadoSunat([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.ConsultarEstadoSunatAsync(request, ct));

    [HttpPost("anular")]
    public async Task<IActionResult> Anular([FromBody] AnularGuiaRequest request, CancellationToken ct)
        => Ok(await guiasManuales.AnularAsync(request, ct));
}
