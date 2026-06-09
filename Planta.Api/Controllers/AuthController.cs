using Microsoft.AspNetCore.Mvc;
using Planta.Api.DTOs;
using Planta.Application.Auth.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/auth")]
[ApiController]
public sealed class AuthController(ILogger<AuthController> logger, IAuthUseCase authUseCase, IWebHostEnvironment env) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await authUseCase.LoginAsync(request.Usuario, request.Password);
            Response.Cookies.Append("access_token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = env.IsProduction(),
                SameSite = env.IsProduction() ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            });

            return Ok(new
            {
                user = result.User
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", request.Usuario);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en login");
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
            Secure = env.IsProduction(),
            SameSite = env.IsProduction() ? SameSiteMode.None : SameSiteMode.Lax
        });

        return Ok(new
        {
            message = "Sesión cerrada correctamente"
        });
    }
}
