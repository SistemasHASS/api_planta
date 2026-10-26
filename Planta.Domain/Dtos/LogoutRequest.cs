using System.ComponentModel.DataAnnotations;

namespace Planta.Domain.Dtos;

public sealed class LogoutRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
