namespace api_planta.Domain.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = "";
    public UsuarioAcopioDto User { get; set; } = new UsuarioAcopioDto();
}


