using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Endpoints de autenticacion. El controller solo traduce HTTP a casos de uso:
/// no tiene logica ni try/catch (los errores los convierte ExceptionMiddleware).
/// </summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthUseCase auth, ICurrentUser currentUser) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        => Ok(await auth.LoginAsync(request.Usuario, request.Password, ObtenerCliente(request.Client), ct));

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResult>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
        => Ok(await auth.RefreshAsync(request.RefreshToken, ObtenerCliente(request.Client), ct));

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request, CancellationToken ct)
    {
        await auth.LogoutAsync(request.RefreshToken, ct);
        return NoContent();
    }

    /// <summary>Datos del usuario autenticado, leidos del token (sirve para probar que el JWT funciona).</summary>
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        currentUser.UserId,
        currentUser.UserName,
        currentUser.Role,
        currentUser.IdEmpresa,
        currentUser.Ruc,
        currentUser.DocumentoIdentidad,
        currentUser.CodigoAcopio,
        currentUser.SerieGuia
    });

    [Authorize]
    [HttpGet("sessions")]
    public async Task<ActionResult<IReadOnlyList<ActiveSession>>> Sessions(CancellationToken ct)
        => Ok(await auth.ListSessionsAsync(ct));

    [Authorize]
    [HttpGet("sessions/{sessionId:guid}")]
    public async Task<IActionResult> RevokeSession(Guid sessionId, CancellationToken ct)
    {
        await auth.RevokeSessionAsync(sessionId, ct);
        return NoContent();
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll(CancellationToken ct)
    {
        await auth.LogoutAllAsync(ct);
        return NoContent();
    }

    private ClientSessionInfo ObtenerCliente(ClientSessionRequest client) => new()
    {
        DeviceId = client.DeviceId.Trim(),
        ClientType = client.ClientType.Trim().ToUpperInvariant(),
        DeviceName = client.DeviceName?.Trim(),
        IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
        UserAgent = Request.Headers.UserAgent.ToString()
    };
}
