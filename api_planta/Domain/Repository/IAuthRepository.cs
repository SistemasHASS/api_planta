
using System.Text.Json;
using api_planta.Domain.DTOs.Auth;

namespace api_planta.Domain.Repository
{
    public interface IAuthRepository
    {
        Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json);
    }
}