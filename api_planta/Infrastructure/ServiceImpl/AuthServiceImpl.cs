

using System.Text.Json;
using api_planta.Domain.DTOs.Auth;
using api_planta.Domain.Repository;
using api_planta.Domain.Services;

namespace api_planta.Infrastructure.ServiceImpl
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        
        public AuthServiceImpl(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json)
        {
            return await _authRepository.GetUsuarioAcopioAsync(json);
        }

    }
}