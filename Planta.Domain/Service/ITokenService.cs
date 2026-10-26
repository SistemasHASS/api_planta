using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

/// <summary>Crea los tokens de sesion. Implementacion JWT en Infrastructure/ServiceImpl.</summary>
public interface ITokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateAccessToken(UsuarioDto user, DateTime sessionExpiresAtUtc);
    (string Token, DateTime ExpiresAtUtc) CreateRefreshToken();
    DateTime CreateSessionExpiresAtUtc();
}
