using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Guias de remision (cada guia agrupa palets cerrados) y su emision ante SUNAT.
/// Devuelve la respuesta del SP: { error, data, mensaje }. Empresa, RUC, usuario, rol y acopio salen del token. Requiere token.
/// </summary>
[ApiController]
[Route("api/guias")]
public sealed class GuiasController(IGuiasUseCase guias) : ControllerBase
{
    /// <summary>Crea guias con sus palets (el cuerpo trae el JSON que lee el SP).</summary>
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] SincronizarGuiasRequest request, CancellationToken ct)
        => Ok(await guias.SincronizarAsync(request, ct));

    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] ListarGuiasRequest request, CancellationToken ct)
        => Ok(await guias.ListarAsync(request, ct));

    /// <summary>Excel con las guias filtradas (solo administrador y monitor).</summary>
    [HttpPost("exportar-excel")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Monitor}")]
    public async Task<IActionResult> ExportarExcel([FromBody] ExportarGuiasRequest request, CancellationToken ct)
    {
        var archivo = await guias.ExportarExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Detalle de una guia con nombres de catalogo completados.</summary>
    [HttpPost("obtener")]
    public async Task<IActionResult> Obtener([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guias.ObtenerAsync(request, ct));

    [HttpPost("eliminar")]
    public async Task<IActionResult> Eliminar([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guias.EliminarAsync(request, ct));

    /// <summary>Emite la guia ante SUNAT y guarda el estado devuelto.</summary>
    [HttpPost("emitir")]
    public async Task<IActionResult> Emitir([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guias.EmitirAsync(request, ct));

    [HttpPost("consultar-estado-sunat")]
    public async Task<IActionResult> ConsultarEstadoSunat([FromBody] GuiaRequest request, CancellationToken ct)
        => Ok(await guias.ConsultarEstadoSunatAsync(request, ct));

    [HttpPost("anular")]
    public async Task<IActionResult> Anular([FromBody] AnularGuiaRequest request, CancellationToken ct)
        => Ok(await guias.AnularAsync(request, ct));

    [HttpPost("editar")]
    public async Task<IActionResult> Editar([FromBody] EditarGuiaRequest request, CancellationToken ct)
        => Ok(await guias.EditarAsync(request, ct));

    [HttpPost("codigos-caja")]
    public async Task<IActionResult> CodigosCaja(CancellationToken ct)
        => Ok(await guias.ListarCodigosCajaAsync(ct));

    /// <summary>Procesos abiertos que ya tienen palets cerrados (los que se pueden despachar en una guia).</summary>
    [HttpPost("procesos")]
    public async Task<IActionResult> Procesos([FromBody] ProcesosGuiaRequest request, CancellationToken ct)
        => Ok(await guias.ListarProcesosConPaletsCerradosAsync(request, ct));
}
