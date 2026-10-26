namespace Planta.Domain.Dtos;

public sealed class RefreshTokenResult
{
    public required Guid SessionId { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTime AccessTokenExpiresAtUtc { get; init; }
    public required DateTime RefreshTokenExpiresAtUtc { get; init; }
    public required DateTime SessionExpiresAtUtc { get; init; }
}
