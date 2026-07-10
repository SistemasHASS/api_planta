using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.GuiaRemisionManual.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/guias-remision-manual")]
[ApiController]
public sealed class GuiasRemisionManualController(
    ILogger<GuiasRemisionManualController> logger,
    ICurrentUserContext _currentUser,
    IGuiasRemisionManualUseCase guiasRemisionManualUseCase) : ControllerBase
{
    public class SincronizarGuiasRemisionManualRequest
    {
        public string? IdProyecto { get; set; }
        public JsonElement? Guias { get; set; }
    }

    [HttpPost("sincronizar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarGuiasRemisionManual([FromBody] SincronizarGuiasRemisionManualRequest request)
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

            var result = await guiasRemisionManualUseCase.SincronizarGuiasRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                request!.IdProyecto!,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonGuias
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
            logger.LogError(ex, "Error interno en SincronizarGuiasRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("listar-guias-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ListarGuiasRemisionManual(
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var result = await guiasRemisionManualUseCase.ListarGuiasRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
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
            logger.LogError(ex, "Error interno en ListarGuiasRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("get-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.GetGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.Role!,
                codigoGuiaRemision
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
            logger.LogError(ex, "Error interno en GetGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpPost("editar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EditarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromBody] object json)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (json == null)
            {
                return BadRequest("Json is required");
            }

            var jsonString = System.Text.Json.JsonSerializer.Serialize(json);

            var result = await guiasRemisionManualUseCase.EditarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonString
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
            logger.LogError(ex, "Error interno en EditarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("eliminar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EliminarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.EliminarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                codigoGuiaRemision,
                _currentUser.UserName!,
                _currentUser.Role!
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
            logger.LogError(ex, "Error interno en EliminarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("emitir-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EmitirGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.EmitirGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
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
            logger.LogError(ex, "Error interno en EmitirGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("reenviar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ReenviarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.ReenviarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
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
            logger.LogError(ex, "Error interno en ReenviarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("consultar-estado-sunat-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ConsultarEstadoSunatGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.ConsultarEstadoSunatGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
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
            logger.LogError(ex, "Error interno en ConsultarEstadoSunatGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("anular-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> AnularGuiaRemisionManual([FromQuery] string idProyecto, [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.AnularGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
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
            logger.LogError(ex, "Error interno en AnularGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }
}
