

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.Catalogos.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/catalogos")]
[ApiController]
public sealed class CatalogosController(ILogger<CatalogosController> logger, ICurrentUserContext _currentUser, ICatalogosUseCase catalogosUseCase) : ControllerBase
{
    [HttpGet("get-acopios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetAcopios()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            var result = await catalogosUseCase.GetAcopiosAsync(_currentUser.IdEmpresa!);
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

    [HttpGet("get-fundos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetFundos()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }

            var result = await catalogosUseCase.GetFundosAsync(_currentUser.IdEmpresa!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetFundos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-formatos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetFormatos([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetFormatosAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetFormatos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-clientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetClientes()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }

            var result = await catalogosUseCase.GetClientesAsync(_currentUser.IdEmpresa!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetClientes");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-variedades")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetVariedades()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }

            var result = await catalogosUseCase.GetVariedadesAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetVariedades");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-cultivos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetCultivos()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }

            var result = await catalogosUseCase.GetCultivosAsync(_currentUser.IdEmpresa!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetCultivos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-campanias")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetCampanias()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            var result = await catalogosUseCase.GetCampaniasAsync(_currentUser.Ruc!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetCampanias");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-paises")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPaises()
    {
        try
        {
            var result = await catalogosUseCase.GetPaisesAsync();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetPaises");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-calibres")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetCalibres()
    {
        try
        {
            var result = await catalogosUseCase.GetCalibresAsync();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetCalibres");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-transporte")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTransporte()
    {
        try
        {
            var result = await catalogosUseCase.GetTransportesAsync();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTransporte");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tiposClamshell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposClamshell([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetTiposClamshellAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposClamshell");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tiposEmpaques")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposEmpaques([FromQuery] string json)
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
            
            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetTiposEmpaquesAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposEmpaques");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-categoria")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetCategoria([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetCategoriaAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetCategoria");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tiposEmpaqueGuia")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposEmpaqueGuia([FromQuery] string json)
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

             var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetTiposEmpaqueGuiaAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposEmpaqueGuia");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-presentaciones")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPresentaciones([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetPresentacionesAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetPresentaciones");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tiposCajas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposCajas([FromQuery] string json)
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
             var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetTipoCajaAsync(_currentUser.IdEmpresa, _currentUser.Ruc, codigoCultivo ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposCajas");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-lugaresProduccion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetLugaresProduccion([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

             var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetLugaresProduccionAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? "");
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetLugaresProduccion");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-conductores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetConductores([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

             var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetConductoresAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetConductores");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-vehiculos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetVehiculos([FromQuery] string json)
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
            
            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;

            var result = await catalogosUseCase.GetVehiculosAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetVehiculos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-transportistas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTransportistas([FromQuery] string json)
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
            
            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

             var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;
            
            var result = await catalogosUseCase.GetTransportistasAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTransportistas");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-supervisores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetSupervisores([FromQuery] string json)
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
            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;
            var result = await catalogosUseCase.GetSupervisoresAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetSupervisores");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-personalLogistico")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPersonalLogistico([FromQuery] string json)
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

            var doc = JsonDocument.Parse(json); 
            var root = doc.RootElement;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;


            var result = await catalogosUseCase.GetPersonalLogisticoAsync(_currentUser.IdEmpresa, _currentUser.Ruc, idproyecto ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetPersonalLogistico");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-listaUsuarios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    public class SincronizarCatalogoRequest
    {
        public string? Tabla { get; set; }
        public JsonElement? Json { get; set; }
    }

    [HttpPost("sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarCatalogos([FromBody] SincronizarCatalogoRequest request)
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

            var tabla = request?.Tabla;
            var json = ControllerJsonHelper.ExtractJson(request?.Json);

            if (string.IsNullOrWhiteSpace(tabla))
                return BadRequest(new { error = true, mensaje = "El campo 'tabla' es requerido." });

            var result = await catalogosUseCase.SincronizarCatalogosAsync(tabla, json, _currentUser.IdEmpresa, _currentUser.Ruc, _currentUser.UserName);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarCatalogos");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }


}