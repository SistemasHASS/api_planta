namespace Planta.Domain.Service;

/// <summary>
/// Usuario autenticado, leido de los claims del JWT. Es la unica fuente confiable de empresa,
/// RUC, usuario y rol: el frontend nunca debe enviarlos. La implementacion vive en Api/Shared.
/// </summary>
public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    string? NombreCompleto { get; }
    string? Role { get; }
    string? IdEmpresa { get; }
    string? Ruc { get; }
    string? DocumentoIdentidad { get; }
    string? CodigoAcopio { get; }
    string? SerieGuia { get; }
    /// <summary>Jornada elegida por el anotador (header X-Id-Jornada); null si no se envió.</summary>
    int? IdJornada { get; }
}
