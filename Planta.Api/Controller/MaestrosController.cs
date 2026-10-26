using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Catalogos de apimaestra. El frontend NO envia la empresa ni el RUC: se toman del token.
/// Requiere token (politica por defecto); si un maestro es solo para ciertos roles agregar
/// [Authorize(Roles = ...)] en ese endpoint.
/// </summary>
[ApiController]
[Route("api/maestros")]
public sealed class MaestrosController(IMaestrosUseCase maestros) : ControllerBase
{
    [HttpPost("fundos")]
    public async Task<IActionResult> Fundos(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Fundos, ct));

    [HttpPost("cultivos")]
    public async Task<IActionResult> Cultivos(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Cultivos, ct));

    /// <summary>Campañas (lista-campanias). El RUC sale del token.</summary>
    [HttpPost("campanias")]
    public async Task<IActionResult> Campanias(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Campanias, ct));

    [HttpPost("acopios")]
    public async Task<IActionResult> Acopios(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Acopios, ct));

    [HttpPost("variedades")]
    public async Task<IActionResult> Variedades(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Variedades, ct));

    [HttpPost("clientes")]
    public async Task<IActionResult> Clientes(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Clientes, ct));

    [HttpPost("paises")]
    public async Task<IActionResult> Paises(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Paises, ct));

    [HttpPost("calibres")]
    public async Task<IActionResult> Calibres(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Calibres, ct));

    [HttpPost("transporte")]
    public async Task<IActionResult> Transporte(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.Transporte, ct));

    [HttpPost("tipos-clamshell")]
    public async Task<IActionResult> TiposClamshell(CancellationToken ct) => Ok(await maestros.ConsultarAsync(MaestroExterno.TiposClamshell, ct));
}
