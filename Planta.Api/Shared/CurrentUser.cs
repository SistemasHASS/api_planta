using System.Security.Claims;
using Planta.Domain.Constants;
using Planta.Domain.Service;

namespace Planta.Api.Shared;

/// <summary>Implementacion de ICurrentUser: lee los claims del JWT de la peticion actual.</summary>
public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal? Principal => accessor.HttpContext?.User;

    public string? UserId => Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => Principal?.FindFirstValue(ClaimTypes.Name);
    public string? NombreCompleto => Principal?.FindFirstValue(ClaimNames.NombreCompleto);
    public string? Role => Principal?.FindFirstValue(ClaimTypes.Role);
    public string? IdEmpresa => Principal?.FindFirstValue(ClaimNames.IdEmpresa);
    public string? Ruc => Principal?.FindFirstValue(ClaimNames.Ruc);
    public string? DocumentoIdentidad => Principal?.FindFirstValue(ClaimNames.DocumentoIdentidad);
    public string? CodigoAcopio => Principal?.FindFirstValue(ClaimNames.CodigoAcopio);
    public string? SerieGuia => Principal?.FindFirstValue(ClaimNames.SerieGuia);
    public int? IdJornada => int.TryParse(accessor.HttpContext?.Request.Headers["X-Id-Jornada"].ToString(), out var id) && id > 0 ? id : null;
}
