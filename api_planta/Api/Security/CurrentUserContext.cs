using System.Security.Claims;

namespace api_planta.Api.Security;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal Principal => _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public string? UserId => Principal.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => Principal.FindFirstValue(ClaimTypes.Name);

    public string? Role => Principal.FindFirstValue(ClaimTypes.Role);

    public string? IdEmpresa => Principal.FindFirstValue("IdEmpresa");
    public string? Ruc => Principal.FindFirstValue("Ruc");
    
    public string? IdAcopio => Principal.FindFirstValue("IdAcopio");
    public string? AcopioNombre => Principal.FindFirstValue("AcopioNombre");

    public string? SerieGuia => Principal.FindFirstValue("SerieGuia");
}
