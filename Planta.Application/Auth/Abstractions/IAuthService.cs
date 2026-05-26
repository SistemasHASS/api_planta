using Planta.Application.Auth.Models;

namespace Planta.Application.Auth.Abstractions;

public interface IAuthService
{
    Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json);
}
