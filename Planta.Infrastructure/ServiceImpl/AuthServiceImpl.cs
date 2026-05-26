using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth.Models;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class AuthServiceImpl(IAuthRepository authRepository) : IAuthService
{
    public Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json)
        => authRepository.GetUsuarioAcopioAsync(json);
}
