using System.Text.Json;
using Planta.Domain.Constants;

namespace Planta.Domain.UseCase;

/// <summary>
/// Catalogos de apimaestra para la empresa del usuario autenticado.
/// La empresa y el RUC salen del token (ICurrentUser); el frontend nunca los envia.
/// </summary>
public interface IMaestrosUseCase
{
    Task<JsonElement> ConsultarAsync(MaestroExterno maestro, CancellationToken cancellationToken = default);
}
