using System.Text.Json;

namespace Planta.Application.GuiaRemision.Abstractions;

public interface IDocumentosElectronicosService
{
    Task<ApiDocumentosElectronicosResponse> EnviarGuiaRemisionAsync(JsonElement guiaJson, CancellationToken cancellationToken = default);

    Task<ApiDocumentosElectronicosResponse> ConsultarEstadoGuiaRemisionAsync(
        string idempresa,
        string serie,
        string numero,
        CancellationToken cancellationToken = default);
}

public sealed class ApiDocumentosElectronicosResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public EnviarDocumentoResponse? Data { get; set; }
}

public sealed class EnviarDocumentoResponse
{
    public bool Exito { get; set; }
    public Guid DocumentoId { get; set; }
    public int GuiaRemisionId { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public string? TicketBizlinks { get; set; }
    public string? EstadoSunat { get; set; }
    public string? EstadoBizlinks { get; set; }
    public string? CodigoEstadoSunat { get; set; }
    public string? MensajeSunat { get; set; }
    public string? PdfFileUrl { get; set; }
    public string? XmlFileSignUrl { get; set; }
    public string? XmlFileSunatUrl { get; set; }
}
