using System.Net.Http.Headers;
using System.Linq;
using System.Text;
using System.Text.Json;
using api_planta.Domain.DTOs.Auth;
using api_planta.Domain.Services;

namespace api_planta.Infrastructure.ServiceImpl;

public class MaestrosServiceImpl : IMaestrosAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public MaestrosServiceImpl(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<UsuarioExterno?> ValidarUsuarioAsync(string usuario, string password, string aplicacion)
    {
        var urlMaestros = _configuration["Jwt:UrlMaestros"] ?? "";
        var urlLogin = $"{urlMaestros}/api/Maestros/get-usuarios";

        var client = _httpClientFactory.CreateClient();
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
                    return null;

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

    public async Task<ListaUsuariosResponse?> ObtenerListaUsuariosAsync(string usuario, string ruc, string aplicacion)
    {
        var urlMaestros = _configuration["Jwt:UrlMaestros"] ?? "";
        var urlLista = $"{urlMaestros}/api/Maestros/lista-usuarios";

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var body = new[]
        {
            new
            {
                usuario,
                ruc,
                aplicacion
            }
        };

        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await client.PostAsync(urlLista, content);

        var responseBody = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            return new ListaUsuariosResponse
            {
                Error = true,
                Data = [],
                Mensaje = $"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}"
            };
        }

        try
        {
            using var doc = JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            var item = root.ValueKind == JsonValueKind.Array
                ? root.EnumerateArray().FirstOrDefault()
                : root;

            if (item.ValueKind == JsonValueKind.Undefined || item.ValueKind == JsonValueKind.Null)
            {
                return new ListaUsuariosResponse
                {
                    Error = true,
                    Data = [],
                    Mensaje = "La respuesta vino vacía."
                };
            }

            var error = item.TryGetProperty("error", out var errorElement)
                && errorElement.GetBoolean();

            var mensaje = item.TryGetProperty("mensaje", out var mensajeElement)
                ? mensajeElement.GetString() ?? ""
                : "";

            var data = new List<UsuarioExterno>();

            if (item.TryGetProperty("data", out var dataElement))
            {
                if (dataElement.ValueKind == JsonValueKind.Array)
                {
                    data = dataElement.Deserialize<List<UsuarioExterno>>(JsonOptions) ?? [];
                }
                else if (dataElement.ValueKind == JsonValueKind.String)
                {
                    var dataString = dataElement.GetString();

                    if (!string.IsNullOrWhiteSpace(dataString) && dataString != "[]")
                    {
                        data = JsonSerializer.Deserialize<List<UsuarioExterno>>(dataString, JsonOptions) ?? [];
                    }
                }
            }

            return new ListaUsuariosResponse
            {
                Error = error,
                Data = data,
                Mensaje = mensaje
            };

        }
        catch (Exception ex)
        {
            return new ListaUsuariosResponse
            {
                Error = true,
                Data = [],
                Mensaje = $"Error deserializando respuesta: {ex.Message}"
            };
        }

    }



}
