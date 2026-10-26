using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>Consulta los datos de acopio del usuario (SP PLANTA_GetUsuarioAcopio).</summary>
public interface IUsuarioAcopioRepository
{
    /// <param name="json">Usuario de apimaestra serializado tal cual (ver UsuarioMaestrosDto).</param>
    Task<RespuestaSp<List<UsuarioDto>>> GetUsuarioAcopioAsync(string json, CancellationToken cancellationToken = default);
}
