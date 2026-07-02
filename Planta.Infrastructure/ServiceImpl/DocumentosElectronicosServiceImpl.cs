using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Planta.Application.GuiaRemision.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class DocumentosElectronicosServiceImpl(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IDocumentosElectronicosService
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiDocumentosElectronicosResponse> EnviarGuiaRemisionAsync(JsonElement guiaJson, CancellationToken cancellationToken = default)
    {
        var baseUrl = configuration["ApiDocumentosElectronicos:Url"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "No se configuró la URL de ApiDocumentosElectronicos."
            };
        }

        var url = $"{baseUrl.TrimEnd('/')}/api/documentos/guias-remision";

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var json = JsonSerializer.Serialize(guiaJson);
        Console.WriteLine($"[DocumentosElectronicos] RequestBody: {json}");
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            using var response = await client.PostAsync(url, content, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            Console.WriteLine($"[DocumentosElectronicos] POST {url} -> Status: {(int)response.StatusCode} ({response.StatusCode})");
            Console.WriteLine($"[DocumentosElectronicos] ResponseBody: {responseBody}");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequest = JsonSerializer.Deserialize<ApiDocumentosElectronicosResponse>(responseBody, JsonOptions);
                return badRequest ?? new ApiDocumentosElectronicosResponse
                {
                    Success = false,
                    Message = responseBody
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ApiDocumentosElectronicosResponse
                {
                    Success = false,
                    Message = $"Error del servicio de documentos electrónicos: {(int)response.StatusCode} - {responseBody}"
                };
            }

            var resultado = JsonSerializer.Deserialize<ApiDocumentosElectronicosResponse>(responseBody, JsonOptions);
            return resultado ?? new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "Respuesta vacía del servicio de documentos electrónicos."
            };
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "Timeout al comunicarse con el servicio de documentos electrónicos."
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = $"Error de comunicación con el servicio de documentos electrónicos: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = $"Error inesperado: {ex.Message}"
            };
        }
    }

    public async Task<ApiDocumentosElectronicosResponse> ConsultarEstadoGuiaRemisionAsync(
        string idempresa,
        string serie,
        string numero,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = configuration["ApiDocumentosElectronicos:Url"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "No se configuró la URL de ApiDocumentosElectronicos."
            };
        }

        var url = $"{baseUrl.TrimEnd('/')}/api/documentos/estado/{Uri.EscapeDataString(idempresa)}/{Uri.EscapeDataString(serie)}/{Uri.EscapeDataString(numero)}";

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            using var response = await client.GetAsync(url, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            Console.WriteLine($"[DocumentosElectronicos] GET {url} -> Status: {(int)response.StatusCode} ({response.StatusCode})");
            Console.WriteLine($"[DocumentosElectronicos] ResponseBody: {responseBody}");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var badRequest = JsonSerializer.Deserialize<ApiDocumentosElectronicosResponse>(responseBody, JsonOptions);
                return badRequest ?? new ApiDocumentosElectronicosResponse
                {
                    Success = false,
                    Message = responseBody
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ApiDocumentosElectronicosResponse
                {
                    Success = false,
                    Message = $"Error del servicio de documentos electrónicos: {(int)response.StatusCode} - {responseBody}"
                };
            }

            var resultado = JsonSerializer.Deserialize<ApiDocumentosElectronicosResponse>(responseBody, JsonOptions);
            return resultado ?? new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "Respuesta vacía del servicio de documentos electrónicos."
            };
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = "Timeout al comunicarse con el servicio de documentos electrónicos."
            };
        }
        catch (HttpRequestException ex)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = $"Error de comunicación con el servicio de documentos electrónicos: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new ApiDocumentosElectronicosResponse
            {
                Success = false,
                Message = $"Error inesperado: {ex.Message}"
            };
        }
    }
}
