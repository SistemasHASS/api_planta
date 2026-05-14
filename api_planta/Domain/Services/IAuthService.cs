
using System.Text.Json;
using api_planta.Domain.DTOs.Auth;

namespace api_planta.Domain.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json);
    }
}