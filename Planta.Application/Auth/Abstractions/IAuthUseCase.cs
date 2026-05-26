using Planta.Application.Auth.Models;

namespace Planta.Application.Auth.Abstractions;

public interface IAuthUseCase
{
    Task<LoginResponse> LoginAsync(string usuario, string password);
}
