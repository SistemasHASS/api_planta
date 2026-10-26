using System.ComponentModel.DataAnnotations;

namespace Planta.Domain.Dtos;

public sealed class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;

    [Required]
    public ClientSessionRequest Client { get; set; } = new();
}
