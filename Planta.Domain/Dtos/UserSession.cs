namespace Planta.Domain.Dtos;

/// <summary>Sesion activa encontrada por su refresh token.</summary>
public sealed class UserSession
{
    public required Guid SessionId { get; init; }
    public required UsuarioDto User { get; init; }
    public required ClientSessionInfo Client { get; init; }
    public required DateTime SessionExpiresAtUtc { get; init; }
}
