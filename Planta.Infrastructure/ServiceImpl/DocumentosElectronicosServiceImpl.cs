using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Planta.Domain.Dtos;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Cliente de ApiDocumentosElectronicos. La URL base se configura en appsettings
/// (ExternalApis:DocumentosElectronicos:BaseUrl); si falta, las llamadas responden Success = false.
/// </summary>
public sealed class DocumentosElectronicosServiceImpl(IHttpClientFactory httpClientFactory, ILogger<DocumentosElectronicosServiceImpl> logger) : IDocumentosElectronicosService
{
    /// <summary>Nombre del HttpClient (se registra en DependencyInjection solo si hay URL).</summary>
    public const string HttpClientName = "ApiDocumentosElectronicos";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public Task<RespuestaDocumentosElectronicos> EnviarGuiaAsync(JsonNode guia, CancellationToken cancellationToken = default)
        => LlamarAsync(
            "POST api/documentos/guias-remision",
            (cliente, ct) => cliente.PostAsJsonAsync("api/documentos/guias-remision", guia, ct),
            cancellationToken);

    public Task<RespuestaDocumentosElectronicos> ConsultarEstadoGuiaAsync(string idEmpresa, string serie, string numero, CancellationToken cancellationToken = default)
    {
        var ruta = $"api/documentos/estado/{Uri.EscapeDataString(idEmpresa)}/{Uri.EscapeDataString(serie)}/{Uri.EscapeDataString(numero)}";
        return LlamarAsync($"GET {ruta}", (cliente, ct) => cliente.GetAsync(ruta, ct), cancellationToken);
    }

    private async Task<RespuestaDocumentosElectronicos> LlamarAsync(string descripcion, Func<HttpClient, CancellationToken, Task<HttpResponseMessage>> enviar, CancellationToken cancellationToken)
    {
        var cliente = httpClientFactory.CreateClient(HttpClientName);
        if (cliente.BaseAddress is null)
        {
            return Fallo("No se configuro la URL de ApiDocumentosElectronicos (ExternalApis:DocumentosElectronicos:BaseUrl).");
        }

        try
        {
            using var respuesta = await enviar(cliente, cancellationToken);
            var cuerpo = await respuesta.Content.ReadAsStringAsync(cancellationToken);
            logger.LogInformation("ApiDocumentosElectronicos {Llamada} -> {Estado}", descripcion, (int)respuesta.StatusCode);

            // 400: la API explica el motivo dentro del cuerpo.
            if (respuesta.StatusCode == HttpStatusCode.BadRequest)
            {
                return Leer(cuerpo) ?? Fallo(cuerpo);
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                return Fallo($"Error del servicio de documentos electronicos: {(int)respuesta.StatusCode} - {cuerpo}");
            }

            return Leer(cuerpo) ?? Fallo("Respuesta vacia del servicio de documentos electronicos.");
        }
        catch (TaskCanceledException)
        {
            return Fallo("Tiempo de espera agotado al comunicarse con el servicio de documentos electronicos.");
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning(ex, "Fallo la comunicacion con ApiDocumentosElectronicos ({Llamada})", descripcion);
            return Fallo($"Error de comunicacion con el servicio de documentos electronicos: {ex.Message}");
        }
    }

    private static RespuestaDocumentosElectronicos? Leer(string cuerpo)
    {
        try
        {
            return JsonSerializer.Deserialize<RespuestaDocumentosElectronicos>(cuerpo, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static RespuestaDocumentosElectronicos Fallo(string mensaje) => new() { Success = false, Message = mensaje };
}
