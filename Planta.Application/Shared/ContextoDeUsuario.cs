using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;

namespace Planta.Application.Shared;

/// <summary>Arma el ContextoUsuario con los datos del token. Empresa y RUC son obligatorios; el resto se exige solo donde hace falta.</summary>
public static class ContextoDeUsuario
{
    public static ContextoUsuario Crear(ICurrentUser usuario, bool exigirUsuarioYRol = false, bool exigirAcopio = false)
    {
        var contexto = new ContextoUsuario(
            Requerir(usuario.IdEmpresa, "la empresa"),
            Requerir(usuario.Ruc, "el RUC"),
            exigirUsuarioYRol ? Requerir(usuario.UserName, "el usuario") : usuario.UserName?.Trim() ?? string.Empty,
            exigirUsuarioYRol ? Requerir(usuario.Role, "el rol") : usuario.Role?.Trim() ?? string.Empty,
            exigirAcopio ? Requerir(usuario.CodigoAcopio, "el codigo de acopio") : usuario.CodigoAcopio?.Trim() ?? string.Empty);
        return contexto;
    }

    private static string Requerir(string? valor, string nombre)
    {
        var texto = valor?.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            throw new BadRequestException($"No se encontro {nombre} en el token del usuario.");
        }

        return texto;
    }
}
