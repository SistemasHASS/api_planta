using Microsoft.AspNetCore.Mvc;
using api_planta.Api.Utils;
using api_planta.Api.Security;
using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace api_planta.Api.Controllers
{
    [Route("api/administracion")]
    [ApiController]
    public class AdministracionController : ControllerBase
    {
        private readonly IAdministracionUseCase _useCase;
        private readonly ICurrentUserContext _currentUser;
        private readonly ILogger<AdministracionController> _logger;

        public AdministracionController(IAdministracionUseCase useCase, ICurrentUserContext currentUser, ILogger<AdministracionController> logger)
        {
            _useCase = useCase;
            _currentUser = currentUser;
            _logger = logger;
        }

        [HttpGet("matrices-compatibilidad/listar")]
        [Authorize]
        public async Task<IActionResult> ListarMatricesCompatibilidad([FromQuery] string? json = null)
        {
            json ??= "{}";
            _logger.LogInformation(
                "[Administracion/matrices-compatibilidad/listar] UserId: {UserId}, Role: {Role}, JSON: {Json}",
                _currentUser.UserId,
                _currentUser.Role,
                json);
            try
            {
                Console.WriteLine($"[DEBUG] JSON: {_currentUser.UserId} - {_currentUser.Role}");
                var resultado = await _useCase.ListarMatricesCompatibilidadAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "matrices-compatibilidad/listar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Administracion/matrices-compatibilidad/listar] Error");
                return StatusCode(500, new { success = false, message = "Error al listar matrices de compatibilidad", details = ex.Message });
            }
        }

        [HttpPost("matrices-compatibilidad/sincronizar")]
        public async Task<IActionResult> SincronizarMatrizCompatibilidad([FromBody] JsonElement? body = null)
        {
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Administracion/matrices-compatibilidad/sincronizar] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.SincronizarMatrizCompatibilidadAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "matrices-compatibilidad/sincronizar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Administracion/matrices-compatibilidad/sincronizar] Error");
                return StatusCode(500, new { success = false, message = "Error al sincronizar matriz de compatibilidad", details = ex.Message });
            }
        }

        [HttpGet("usuarios/listar")]
        [Authorize]
        public async Task<IActionResult> ListarUsuarios([FromQuery] string? json = null)
        {
            json ??= "{}";
            _logger.LogInformation("[Administracion/usuario/listar] JSON: {Json}", json);
            try
            {
                if (_currentUser?.Role != "ADPLA")
                {
                    return StatusCode(403, new { success = false, message = "No tienes permisos para listar usuarios" });
                }
                var resultado = await _useCase.ListarUsuariosAsync(_currentUser.UserName ?? "", _currentUser.Ruc ?? "");
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "usuario/listar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Administracion/usuario/listar] Error");
                return StatusCode(500, new { success = false, message = "Error al listar usuarios", details = ex.Message });
            }
        }

        [HttpPost("usuarios/sincronizar")]
        public async Task<IActionResult> SincronizarUsuarios([FromBody] JsonElement? body = null)
        {
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Administracion/usuario/sincronizar] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.SincronizarUsuariosAsync(_currentUser.UserId ?? "",json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "usuario/sincronizar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Administracion/usuario/sincronizar] Error");
                return StatusCode(500, new { success = false, message = "Error al sincronizar usuarios", details = ex.Message });
            }
        }

        [HttpPost("usuarios/reset-password")]
        public async Task<IActionResult> ResetearPasswordUsuario([FromBody] JsonElement? body = null)
        {
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Administracion/usuarios/reset-password] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ResetearPasswordUsuarioAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "usuarios/reset-password");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Administracion/usuarios/reset-password] Error");
                return StatusCode(500, new { success = false, message = "Error al resetear contraseña", details = ex.Message });
            }
        }
    }
}