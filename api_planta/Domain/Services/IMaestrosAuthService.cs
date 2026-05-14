using api_planta.Domain.DTOs.Auth;

namespace api_planta.Domain.Services;

public interface IMaestrosAuthService
{
    Task<UsuarioExterno?> ValidarUsuarioAsync(string usuario, string password, string aplicacion);
    Task<ListaUsuariosResponse?> ObtenerListaUsuariosAsync(string usuario, string ruc, string aplicacion);
}
