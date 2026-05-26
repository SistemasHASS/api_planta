


using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.Administracion.Abstractions;
using Planta.Application.Catalogos.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/administracion")]
[ApiController]

public sealed class AdministracionController(ILogger<AdministracionController> logger, ICurrentUserContext _currentUser, IAdministracionUseCase administracionUseCase ,ICatalogosUseCase catalogosUseCase) : ControllerBase
{
    [HttpGet("usuarios/listar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
     public async Task<IActionResult> GetListaUsuarios()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }

            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            var result = await catalogosUseCase.GetListaUsuariosAsync(_currentUser.UserName, _currentUser.Ruc);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetListaUsuarios");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpPost("usuarios/sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarUsuarios([FromBody] JsonElement? body =null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }

            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("Idempresa is required");
            }
            var json = ControllerJsonHelper.ExtractJson(body);
            Console.WriteLine(json);
            var result = await administracionUseCase.SincronizarUsuariosAsync(json, _currentUser.IdEmpresa, _currentUser.Ruc, _currentUser.UserName);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarUsuarios");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("matrices-compatibilidad/listar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ListarMatricesCompatibilidad([FromQuery] string json)
    {
        try
        {
             if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("Idempresa is required");
            }

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var idCultivo = root.TryGetProperty("idCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var idProyecto = root.TryGetProperty("idProyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;


            var result = await administracionUseCase.ListarMatricesCompatibilidadAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idProyecto, idCultivo);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ListarMatricesCompatibilidad");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpPost("matrices-compatibilidad/sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarMatrizCompatibilidad([FromBody] JsonElement? body = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("Idempresa is required");
            }
            
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            
            var json = ControllerJsonHelper.ExtractJson(body);
            Console.WriteLine(json);
            var result = await administracionUseCase.SincronizarMatrizCompatibilidadAsync(json, _currentUser.IdEmpresa, _currentUser.Ruc, _currentUser.UserName);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarMatrizCompatibilidad");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }


    [HttpGet("reglas-sobrepeso/listar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ListarReglasSobrePeso([FromQuery] string json)
    {
        try
        {
             if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("Idempresa is required");
            }

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var idProyecto = root.TryGetProperty("idProyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;


            var result = await administracionUseCase.ListarReglasSobrepesoAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idProyecto, codigoCultivo);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ListarReglasSobrePeso");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }


    [HttpPost("reglas-sobrepeso/sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarReglasSobrepeso([FromBody] JsonElement? body = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("Idempresa is required");
            }
            
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            
            var json = ControllerJsonHelper.ExtractJson(body);
            Console.WriteLine(json);
            var result = await administracionUseCase.SincronizarReglasSobrepesoAsync(json, _currentUser.IdEmpresa, _currentUser.Ruc, _currentUser.UserName);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarReglasSobrepeso");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

}