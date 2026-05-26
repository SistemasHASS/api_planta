using Planta.Application.Auth.Models;

namespace Planta.Application.Auth.Abstractions;

public interface IAuthRepository
{
    Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json);
}
