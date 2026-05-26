using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth.Models;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class MaestrosAuthServiceImpl(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IMaestrosAuthService
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<UsuarioExterno?> ValidarUsuarioAsync(string usuario, string password, string aplicacion)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var urlLogin = $"{urlMaestros}/api/Maestros/get-usuarios";

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var body = new[]
        {
            new
            {
                usuario,
                clave = password,
                aplicacion
            }
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await client.PostAsync(urlLogin, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        try
        {
            var element = JsonSerializer.Deserialize<JsonElement>(responseBody, JsonOptions);

            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var dataElement))
            {
                element = dataElement;
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                var first = element.EnumerateArray().FirstOrDefault();
                if (first.ValueKind == JsonValueKind.Undefined || first.ValueKind == JsonValueKind.Null)
                {
                    return null;
                }

                return first.Deserialize<UsuarioExterno>(JsonOptions);
            }

            if (element.ValueKind == JsonValueKind.Object)
            {
                return element.Deserialize<UsuarioExterno>(JsonOptions);
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UsuarioAcopioDto>?> GetUsuarioAcopioAsync(string json)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var urlGetUsuarioAcopio = $"{urlMaestros}/api/Maestros/get-usuario-acopio";

        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await client.PostAsync(urlGetUsuarioAcopio, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        try
        {
            var element = JsonSerializer.Deserialize<JsonElement>(responseBody, JsonOptions);

            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var dataElement))
            {
                element = dataElement;
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                var list = new List<UsuarioAcopioDto>();
                foreach (var item in element.EnumerateArray())
                {
                    list.Add(item.Deserialize<UsuarioAcopioDto>(JsonOptions)!);
                }
                return list;
            }

            if (element.ValueKind == JsonValueKind.Object)
            {
                return [element.Deserialize<UsuarioAcopioDto>(JsonOptions)!];
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

}
