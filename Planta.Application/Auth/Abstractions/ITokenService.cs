using Planta.Application.Auth.Models;

namespace Planta.Application.Auth.Abstractions;

public interface ITokenService
{
    string CrearToken(UsuarioAcopioDto user);
}
