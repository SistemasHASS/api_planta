using System.Text.Json.Nodes;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

/// <summary>
/// Acceso a ApiDocumentosElectronicos (emision de guias de remision ante SUNAT). Nunca lanza por fallas de red:
/// devuelve Success = false con el motivo en Message.
/// </summary>
public interface IDocumentosElectronicosService
{
    /// <summary>Envia la guia (con nombres de propiedad en PascalCase, como espera esa API).</summary>
    Task<RespuestaDocumentosElectronicos> EnviarGuiaAsync(JsonNode guia, CancellationToken cancellationToken = default);

    Task<RespuestaDocumentosElectronicos> ConsultarEstadoGuiaAsync(string idEmpresa, string serie, string numero, CancellationToken cancellationToken = default);
}
