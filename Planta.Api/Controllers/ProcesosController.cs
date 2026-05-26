

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Security;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/procesos")]
[ApiController]
public sealed class ProcesosController(ILogger<CatalogosController> logger, ICurrentUserContext _currentUser, IProcesosUseCase procesosUseCase) : ControllerBase
{
    [HttpGet("get-procesos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAcopios([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.AcopioId))
            {
                return BadRequest("AcopioId is required");
            }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var idCultivo = root.TryGetProperty("idCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var idProyecto = root.TryGetProperty("idProyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;


            var result = await procesosUseCase.ListarProcesosAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, idProyecto!, idCultivo!, _currentUser.AcopioId!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetAcopios");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }
}