namespace Planta.Domain.Dtos;

/// <summary>Fila del listado de sesiones activas (solo administradores).</summary>
public sealed class ActiveSession
{
    public required Guid SessionId { get; init; }
    public string? Username { get; init; }
    public string? FullName { get; init; }
    public required string ClientType { get; init; }
    public string? DeviceName { get; init; }
    public string? IpAddress { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime LastUsedAt { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required DateTime SessionExpiresAt { get; init; }
}
