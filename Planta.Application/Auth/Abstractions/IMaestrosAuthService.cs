using Planta.Application.Auth.Models;

namespace Planta.Application.Auth.Abstractions;

public interface IMaestrosAuthService
{
    Task<UsuarioExterno?> ValidarUsuarioAsync(string usuario, string password, string aplicacion);
}
