using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>
/// Contrato del caso de uso de autenticacion. El controller solo conoce esta interfaz;
/// la logica vive en Application/UseCaseImpl/AuthUseCaseImpl.
/// Las operaciones de sesiones (listar, cerrar una, cerrar todas) toman usuario y empresa del token.
/// </summary>
public interface IAuthUseCase
{
    Task<LoginResponse> LoginAsync(string usuario, string password, ClientSessionInfo client, CancellationToken cancellationToken = default);
    Task<RefreshTokenResult> RefreshAsync(string refreshToken, ClientSessionInfo client, CancellationToken cancellationToken = default);
    Task LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActiveSession>> ListSessionsAsync(CancellationToken cancellationToken = default);
    Task RevokeSessionAsync(Guid sessionId, CancellationToken cancellationToken = default);
    Task LogoutAllAsync(CancellationToken cancellationToken = default);
}
