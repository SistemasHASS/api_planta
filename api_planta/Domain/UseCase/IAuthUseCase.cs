using api_planta.Domain.DTOs.Auth;

namespace api_planta.Domain.UseCase;

public interface IAuthUseCase
{
    Task<LoginResponse> LoginAsync(string usuario, string password);
}
