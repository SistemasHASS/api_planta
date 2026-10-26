namespace Planta.Domain.Dtos;

/// <summary>Datos del usuario autenticado que los SP necesitan. Salen siempre del token, nunca del frontend.</summary>
public sealed record ContextoUsuario(string IdEmpresa, string Ruc, string Usuario, string Rol, string CodigoAcopio);
