using System.Text.Json;
using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth.Models;

namespace Planta.Application.Auth;

public sealed class AuthUseCase(IMaestrosAuthService maestrosAuthService, ITokenService tokenService, IAuthService authService): IAuthUseCase
{
    public async Task<LoginResponse> LoginAsync(string usuario, string password)
    {
        var user = await maestrosAuthService.ValidarUsuarioAsync(usuario, password, "PLANTA");
        if (user is null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var usuarioAcopio = await authService.GetUsuarioAcopioAsync(JsonSerializer.Serialize(user));

        if (usuarioAcopio.Error || usuarioAcopio.Data is null || usuarioAcopio.Data.Count == 0)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var token = tokenService.CrearToken(usuarioAcopio.Data[0]);

        return new LoginResponse
        {
            Token = token,
            User = usuarioAcopio.Data[0]
        };
    }
}
