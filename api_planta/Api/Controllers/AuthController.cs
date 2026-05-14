using api_planta.Api.DTOs;
using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace api_planta.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthUseCase _authUseCase;

        public AuthController(ILogger<AuthController> logger, IAuthUseCase authUseCase)
        {
            _logger = logger;
            _authUseCase = authUseCase;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _authUseCase.LoginAsync(request.Usuario, request.Password);
                Response.Cookies.Append("access_token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                });

                return Ok(new
                {
                    user = result.User
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", request.Usuario);
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno en login");
                return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // true en producción HTTPS
                SameSite = SameSiteMode.Lax
            });

            return Ok(new
            {
                message = "Sesión cerrada correctamente"
            });
        }
    }
}
