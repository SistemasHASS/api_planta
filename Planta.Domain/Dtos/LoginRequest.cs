using System.ComponentModel.DataAnnotations;

namespace Planta.Domain.Dtos;

public sealed class LoginRequest
{
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public ClientSessionRequest Client { get; set; } = new();
}
