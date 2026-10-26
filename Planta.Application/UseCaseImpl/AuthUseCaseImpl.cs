using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>
/// Logica del login de PLANTA: valida contra apimaestra, completa los datos de acopio con
/// PLANTA_GetUsuarioAcopio, crea el JWT y el refresh token y registra la sesion.
/// Solo conoce contratos de Domain; no sabe nada de SQL, HTTP ni JWT.
/// </summary>
public sealed class AuthUseCaseImpl(
    IMaestrosService maestros,
    IUsuarioAcopioRepository usuarioAcopioRepository,
    ITokenService tokenService,
    IAuthRepository authRepository,
    ICurrentUser currentUser) : IAuthUseCase
{
    public async Task<LoginResponse> LoginAsync(string usuario, string password, ClientSessionInfo client, CancellationToken cancellationToken = default)
    {
        var usuarioMaestros = await maestros.ValidarUsuarioAsync(usuario, password, cancellationToken);
        if (usuarioMaestros is null)
        {
            throw new UnauthorizedException("Credenciales invalidas.");
        }

        // El SP recibe el usuario de apimaestra tal cual (sin camelCase) y devuelve los datos de acopio.
        var usuarioAcopio = await usuarioAcopioRepository.GetUsuarioAcopioAsync(JsonSerializer.Serialize(usuarioMaestros), cancellationToken);
        if (usuarioAcopio.Error || usuarioAcopio.Data is null || usuarioAcopio.Data.Count == 0)
        {
            throw new UnauthorizedException("Credenciales invalidas.");
        }

        var user = usuarioAcopio.Data[0];
        var sessionExpiresAtUtc = tokenService.CreateSessionExpiresAtUtc();
        var access = tokenService.CreateAccessToken(user, sessionExpiresAtUtc);
        var refresh = LimitRefreshToSession(tokenService.CreateRefreshToken(), sessionExpiresAtUtc);

        var sessionId = await authRepository.CreateAsync(refresh.Token, user, client, refresh.ExpiresAtUtc, sessionExpiresAtUtc, cancellationToken);

        return new LoginResponse
        {
            SessionId = sessionId,
            User = user,
            AccessToken = access.Token,
            RefreshToken = refresh.Token,
            AccessTokenExpiresAtUtc = access.ExpiresAtUtc,
            RefreshTokenExpiresAtUtc = refresh.ExpiresAtUtc,
            SessionExpiresAtUtc = sessionExpiresAtUtc
        };
    }

    public async Task<RefreshTokenResult> RefreshAsync(string refreshToken, ClientSessionInfo client, CancellationToken cancellationToken = default)
    {
        var session = await authRepository.FindActiveAsync(refreshToken, cancellationToken);
        if (session is null || !string.Equals(session.Client.DeviceId, client.DeviceId, StringComparison.Ordinal))
        {
            throw new UnauthorizedException("Refresh token invalido o vencido.");
        }

        var access = tokenService.CreateAccessToken(session.User, session.SessionExpiresAtUtc);
        var refresh = LimitRefreshToSession(tokenService.CreateRefreshToken(), session.SessionExpiresAtUtc);
        var sessionId = await authRepository.RotateAsync(refreshToken, refresh.Token, session.User, client, refresh.ExpiresAtUtc, cancellationToken);
        if (sessionId is null)
        {
            throw new UnauthorizedException("Refresh token invalido o ya fue utilizado.");
        }

        return new RefreshTokenResult
        {
            SessionId = sessionId.Value,
            AccessToken = access.Token,
            RefreshToken = refresh.Token,
            AccessTokenExpiresAtUtc = access.ExpiresAtUtc,
            RefreshTokenExpiresAtUtc = refresh.ExpiresAtUtc,
            SessionExpiresAtUtc = session.SessionExpiresAtUtc
        };
    }

    public Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default)
        => authRepository.RevokeAsync(refreshToken, "LOGOUT", cancellationToken);

    public Task<IReadOnlyList<ActiveSession>> ListSessionsAsync(CancellationToken cancellationToken = default)
    {
        var (userId, companyId) = RequireIdentity();
        return authRepository.ListActiveAsync(userId, companyId, cancellationToken);
    }

    public async Task RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var (userId, companyId) = RequireIdentity();
        var revoked = await authRepository.RevokeByIdAsync(sessionId, userId, companyId, "CIERRE_REMOTO", cancellationToken);
        if (!revoked)
        {
            throw new NotFoundException("La sesion no existe o ya fue cerrada.");
        }
    }

    public Task LogoutAllAsync(CancellationToken cancellationToken = default)
    {
        var (userId, companyId) = RequireIdentity();
        return authRepository.RevokeAllAsync(userId, companyId, "CIERRE_GLOBAL", cancellationToken);
    }

    // El usuario y la empresa salen del token (ICurrentUser), nunca de lo que envie el cliente.
    private (string UserId, string CompanyId) RequireIdentity()
    {
        var userId = currentUser.UserId;
        var companyId = currentUser.IdEmpresa;
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(companyId))
        {
            throw new UnauthorizedException("El token no contiene la identidad del usuario.");
        }

        return (userId, companyId);
    }

    // El refresh token nunca puede vivir mas que la sesion maxima.
    private static (string Token, DateTime ExpiresAtUtc) LimitRefreshToSession(
        (string Token, DateTime ExpiresAtUtc) refresh,
        DateTime sessionExpiresAtUtc)
        => refresh.ExpiresAtUtc <= sessionExpiresAtUtc
            ? refresh
            : (refresh.Token, sessionExpiresAtUtc);
}
