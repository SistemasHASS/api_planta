using System.Security.Claims;

namespace api_planta.Api.Security;

public interface ICurrentUserContext
{
    ClaimsPrincipal Principal { get; }
    bool IsAuthenticated { get; }

    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }

    string? IdEmpresa { get; }
    string? Ruc { get; }
}
