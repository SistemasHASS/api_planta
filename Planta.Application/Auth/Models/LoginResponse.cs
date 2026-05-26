namespace Planta.Application.Auth.Models;

public sealed class LoginResponse
{
    public string Token { get; set; } = "";
    public UsuarioAcopioDto User { get; set; } = new UsuarioAcopioDto();
}
