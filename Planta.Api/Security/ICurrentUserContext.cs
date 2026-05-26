using System.Security.Claims;

namespace Planta.Api.Security;

public interface ICurrentUserContext
{
    ClaimsPrincipal Principal { get; }
    bool IsAuthenticated { get; }

    string? UserId { get; }
    string? UserName { get; }
    string? Role { get; }

    string? IdEmpresa { get; }
    string? Ruc { get; }
    string? AcopioId { get; }
    string?SerieGuia{  get;}

}
