

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Planta.Application.Catalogos.Models;
using Planta.Application.Maestros.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class MaestrosServiceImpl(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IMaestrosService
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public sealed class ListaUsuariosResponse
    {
        public bool Error { get; set; }
        public List<ListaUsuariosExterno>? Data { get; set; }
        public string Mensaje { get; set; } = "";
    }



    private HttpClient CreateClient()
    {
        var client = httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    private async Task<JsonElement?> PostAndGetJsonElementAsync(string url, object body)
    {
        var client = CreateClient();
        var json = JsonSerializer.Serialize(body);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await client.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<JsonElement>(responseBody, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private static JsonElement UnwrapDataIfPresent(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var dataElement))
        {
            return dataElement;
        }

        return element;
    }

    public async Task<IReadOnlyList<AcopiosExterno>?> GetAcopiosAsync(string idempresa)
    {   
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var urlGetAcopios = $"{urlMaestros}/api/Maestros/get-acopios";

        var body = new[]
        {
        new
        {
            idempresa,
            aplicacion="PLANTA",
        }
        };

        var elementNullable = await PostAndGetJsonElementAsync(urlGetAcopios, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);

        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<AcopiosExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<AcopiosExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<AcopiosExterno>(JsonOptions);
            return single is null ? null : new List<AcopiosExterno> { single };
        }

        return null;

    }

    public async Task<IReadOnlyList<FundoExterno>?> GetFundosAsync(string idempresa)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var urlGetFundos = $"{urlMaestros}/api/Maestros/get-fundos";

        var body = new[]
        {
            new
            {
                idempresa
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(urlGetFundos, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);

        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<FundoExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<FundoExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<FundoExterno>(JsonOptions);
            return single is null ? null : new List<FundoExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<FormatoExterno>?> GetFormatosAsync(string idempresa)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/Maestros/formato/listado";

        var body = new[]
        {
            new
            {
                idempresa,
                aplicacion = "PLANTA"
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<FormatoExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<FormatoExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<FormatoExterno>(JsonOptions);
            return single is null ? null : new List<FormatoExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<ClienteExterno>?> GetClientesAsync(string idempresa)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/Maestros/clientes/listado";

        var body = new[]
        {
            new
            {
                idempresa,
                aplicacion = "PLANTA"
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<ClienteExterno>();
            foreach (var item in element.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String)
                {
                    var jsonStr = item.GetString();
                    if (!string.IsNullOrWhiteSpace(jsonStr))
                    {
                        var parsed = JsonSerializer.Deserialize<ClienteExterno>(jsonStr, JsonOptions);
                        if (parsed is not null)
                        {
                            list.Add(parsed);
                        }
                    }

                    continue;
                }

                var direct = item.Deserialize<ClienteExterno>(JsonOptions);
                if (direct is not null)
                {
                    list.Add(direct);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.String)
        {
            var jsonStr = element.GetString();
            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return null;
            }

            var single = JsonSerializer.Deserialize<ClienteExterno>(jsonStr, JsonOptions);
            return single is null ? null : new List<ClienteExterno> { single };
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<ClienteExterno>(JsonOptions);
            return single is null ? null : new List<ClienteExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<VariedadExterna>?> GetVariedadesAsync(string idempresa)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/Maestros/get-variedades";

        var body = new[]
        {
            new
            {
                idempresa,
                aplicacion = "PLANTA"
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<VariedadExterna>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<VariedadExterna>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<VariedadExterna>(JsonOptions);
            return single is null ? null : new List<VariedadExterna> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<CultivoExterno>?> GetCultivosAsync(string idempresa)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/Maestros/get-cultivos";

        var body = new[]
        {
            new
            {
                idempresa
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<CultivoExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<CultivoExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<CultivoExterno>(JsonOptions);
            return single is null ? null : new List<CultivoExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<CampaniaExterna>?> GetCampaniasAsync(string ruc)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-campanias";

        var body = new[]
        {
            new
            {
                ruc,
                aplicacion = "PLANTA"
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<CampaniaExterna>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<CampaniaExterna>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<CampaniaExterna>(JsonOptions);
            return single is null ? null : new List<CampaniaExterna> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<PaisExterno>?> GetPaisesAsync()
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-paises";

        var elementNullable = await PostAndGetJsonElementAsync(url, Array.Empty<object>());
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<PaisExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<PaisExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<PaisExterno>(JsonOptions);
            return single is null ? null : new List<PaisExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<CalibreExterno>?> GetCalibresAsync()
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-calibres";

        var elementNullable = await PostAndGetJsonElementAsync(url, Array.Empty<object>());
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<CalibreExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<CalibreExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<CalibreExterno>(JsonOptions);
            return single is null ? null : new List<CalibreExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<TransporteExterno>?> GetTransportesAsync()
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-transporte";

        var elementNullable = await PostAndGetJsonElementAsync(url, Array.Empty<object>());
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<TransporteExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<TransporteExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<TransporteExterno>(JsonOptions);
            return single is null ? null : new List<TransporteExterno> { single };
        }

        return null;
    }

    public async Task<IReadOnlyList<TipoClamshellExterno>?> GetTiposClamshellAsync()
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-tiposClamshell";

        var elementNullable = await PostAndGetJsonElementAsync(url, Array.Empty<object>());
        if (elementNullable is null)
        {
            return null;
        }

        var element = UnwrapDataIfPresent(elementNullable.Value);
        if (element.ValueKind == JsonValueKind.Array)
        {
            var list = new List<TipoClamshellExterno>();
            foreach (var item in element.EnumerateArray())
            {
                var parsed = item.Deserialize<TipoClamshellExterno>(JsonOptions);
                if (parsed is not null)
                {
                    list.Add(parsed);
                }
            }

            return list;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var single = element.Deserialize<TipoClamshellExterno>(JsonOptions);
            return single is null ? null : new List<TipoClamshellExterno> { single };
        }

        return null;
    }

    public async Task<List<ListaUsuariosExterno>?> GetListaUsuariosAsync(string usuario, string ruc)
    {
        var urlMaestros = configuration["Jwt:UrlMaestros"] ?? "";
        var url = $"{urlMaestros}/api/maestros/lista-usuarios";

        var body = new[]
        {
            new
            {
                usuario,
                ruc,
                aplicacion = "PLANTA"
            }
        };

        var elementNullable = await PostAndGetJsonElementAsync(url, body);
        if (elementNullable is null)
        {
            return null;
        }

        var element = elementNullable.Value;
        if (element.ValueKind == JsonValueKind.Array)
        {
            var envelopes = element.Deserialize<List<ListaUsuariosResponse>>(JsonOptions);
            var first = envelopes?.FirstOrDefault();
            return first?.Data;
        }

        if (element.ValueKind == JsonValueKind.Object)
        {
            var env = element.Deserialize<ListaUsuariosResponse>(JsonOptions);
            return env?.Data;
        }
        
        return null;
    }

    

}
