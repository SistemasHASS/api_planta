namespace Planta.Domain.Dtos;

/// <summary>Datos del usuario autenticado que los SP necesitan. Salen siempre del token, nunca del frontend.
/// IdJornada es la unica excepcion: la jornada abierta que el anotador eligio en pantalla (header X-Id-Jornada); null = la mas reciente del acopio.</summary>
public sealed record ContextoUsuario(string IdEmpresa, string Ruc, string Usuario, string Rol, string CodigoAcopio, string NombreUsuario = "", int? IdJornada = null);
