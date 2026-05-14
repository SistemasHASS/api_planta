using System.Text.Json;
using api_planta.Domain.DTOs.Auth;
using api_planta.Domain.Services;
using api_planta.Domain.UseCase;

namespace api_planta.Application.Usecase;

public class AuthUseCaseImpl : IAuthUseCase
{
    private readonly IMaestrosAuthService _maestrosAuthService;
    private readonly ITokenService _tokenService;

    private readonly IAuthService _authService;

    public AuthUseCaseImpl(IMaestrosAuthService maestrosAuthService, ITokenService tokenService, IAuthService authService)
    {
        _maestrosAuthService = maestrosAuthService;
        _tokenService = tokenService;
        _authService = authService;
    }

    public async Task<LoginResponse> LoginAsync(string usuario, string password)
    {
        var user = await _maestrosAuthService.ValidarUsuarioAsync(usuario, password, "PLANTA");
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var usuarioAcopio = await _authService.GetUsuarioAcopioAsync(JsonSerializer.Serialize(user));

        if (usuarioAcopio.Error)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }
        
        var token = _tokenService.CrearToken(usuarioAcopio.Data[0]);
        
        return new LoginResponse
        {
            Token = token,
            User = usuarioAcopio.Data[0]
        };
    }
}
