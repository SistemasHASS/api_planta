

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.Proceso.Abstractions;
using Planta.Application.GuiaRemision.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/guias-remision")]
[ApiController]
public sealed class GuiasRemisionController(
    ILogger<GuiasRemisionController> logger,
    ICurrentUserContext _currentUser,
    IProcesosUseCase procesosUseCase,
    IGuiasRemisionUseCase guiasRemisionUseCase) : ControllerBase
{
    public class SincronizarGuiasRemisionRequest
    {
        public string? IdProyecto { get; set; }
        public JsonElement? Guias { get; set; }
        public JsonElement? Detalles { get; set; }
    }

    [HttpPost("sincronizar-guias-remision")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarGuiasRemision([FromBody] SincronizarGuiasRemisionRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Role))
            {
                return BadRequest("IdRol is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(request?.IdProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var jsonGuias = ControllerJsonHelper.ExtractJson(request?.Guias);
            var jsonDetalles = ControllerJsonHelper.ExtractJson(request?.Detalles);

            var result = await guiasRemisionUseCase.SincronizarGuiasRemisionAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                request!.IdProyecto!,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonGuias,
                jsonDetalles
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarGuiasRemision");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("listar-guias-remision")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ListarGuiasRemision(
        [FromQuery] string idProyecto,
        [FromQuery] string? estado = null,
        [FromQuery] string? fechaDesde = null,
        [FromQuery] string? fechaHasta = null,
        [FromQuery] string? texto = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Role))
            {
                return BadRequest("IdRol is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var result = await guiasRemisionUseCase.ListarGuiasRemisionAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                _currentUser.Role!,
                estado,
                fechaDesde,
                fechaHasta,
                texto
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ListarGuiasRemision");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("get-procesos-guia")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetProcesosGuia()
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }

            var result = await procesosUseCase.ListarProcesosAbiertosConPaletsCerradosAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                _currentUser.CodigoAcopio!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetProcesosGuia");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }
}
