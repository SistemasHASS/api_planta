namespace Planta.Domain.Dtos;

/// <summary>Datos del dispositivo ya normalizados, mas IP y User-Agent tomados de la peticion HTTP.</summary>
public sealed class ClientSessionInfo
{
    public required string DeviceId { get; init; }
    public required string ClientType { get; init; }
    public string? DeviceName { get; init; }
    public string? IpAddress { get; init; }
    public string? UserAgent { get; init; }
}
