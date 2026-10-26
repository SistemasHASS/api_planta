namespace Planta.Domain.Dtos;

/// <summary>Respuesta de ApiDocumentosElectronicos (emision de guias ante SUNAT).</summary>
public sealed class RespuestaDocumentosElectronicos
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public EnvioDocumentoElectronico? Data { get; set; }
}

public sealed class EnvioDocumentoElectronico
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
