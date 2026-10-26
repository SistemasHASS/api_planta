using System.ComponentModel.DataAnnotations;

namespace Planta.Domain.Dtos;

/// <summary>Datos del dispositivo que el cliente envia en login y refresh.</summary>
public sealed class ClientSessionRequest
{
    [Required]
    [StringLength(100)]
    public string DeviceId { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(WEB|ANDROID|IOS|DESKTOP)$")]
    public string ClientType { get; set; } = "WEB";

    [StringLength(200)]
    public string? DeviceName { get; set; }
}
