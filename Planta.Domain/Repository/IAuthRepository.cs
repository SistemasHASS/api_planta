using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>
/// Contrato de persistencia de sesiones. Guarda solo el hash del refresh token, nunca el valor original.
/// La implementacion (SQL Server) vive en Infrastructure/RepositoryImpl.
/// </summary>
public interface IAuthRepository
{
    Task<Guid> CreateAsync(string refreshToken, UsuarioDto user, ClientSessionInfo client, DateTime refreshExpiresAtUtc, DateTime sessionExpiresAtUtc, CancellationToken cancellationToken = default);
    Task<UserSession?> FindActiveAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<Guid?> RotateAsync(string refreshToken, string newRefreshToken, UsuarioDto user, ClientSessionInfo client, DateTime expiresAtUtc, CancellationToken cancellationToken = default);
    Task RevokeAsync(string refreshToken, string reason, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ActiveSession>> ListActiveAsync(string userId, string companyId, CancellationToken cancellationToken = default);
    Task<bool> RevokeByIdAsync(Guid sessionId, string userId, string companyId, string reason, CancellationToken cancellationToken = default);
    Task RevokeAllAsync(string userId, string companyId, string reason, CancellationToken cancellationToken = default);
}
