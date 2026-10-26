using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Cliente de apimaestra. El navegador nunca la llama: la API propia es el puente.
/// La URL base se configura en appsettings (ExternalApis:Maestros:BaseUrl).
/// </summary>
public sealed class MaestrosServiceImpl(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IMaestrosService
{
    /// <summary>Nombre del HttpClient (se registra en DependencyInjection).</summary>
    public const string HttpClientName = "ApiMaestra";

    private const string GetUsuariosPath = "/api/Maestros/get-usuarios";

    private const string AplicacionPlanta = "PLANTA";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private string Aplicacion => configuration["App:Name"] ?? string.Empty;

    public async Task<UsuarioMaestrosDto?> ValidarUsuarioAsync(string usuario,string password,CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient(HttpClientName);
        var body = new[] { new { usuario, clave = password, aplicacion = Aplicacion } };

        using var response = await client.PostAsJsonAsync(GetUsuariosPath, body, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var element = await JsonSerializer.DeserializeAsync<JsonElement>(stream, JsonOptions, cancellationToken);

            // La respuesta puede venir envuelta en { data: ... } o directa.
            if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var data))
            {
                element = data;
            }

            if (element.ValueKind == JsonValueKind.Array)
            {
                var first = element.EnumerateArray().FirstOrDefault();
                return first.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null
                    ? null
                    : first.Deserialize<UsuarioMaestrosDto>(JsonOptions);
            }

            return element.ValueKind == JsonValueKind.Object
                ? element.Deserialize<UsuarioMaestrosDto>(JsonOptions)
                : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>Endpoint y cuerpo de cada catalogo, igual que los usaba la API anterior.</summary>
    public async Task<JsonElement> ConsultarAsync(
        MaestroExterno maestro,
        string idEmpresa,
        string ruc,
        string? usuario = null,
        bool formatoSp = false,
        CancellationToken cancellationToken = default)
    {
        object lista = maestro switch
        {
            MaestroExterno.Fundos => await LeerAsync<FundoExterno>("/api/Maestros/get-fundos", new[] { new { idempresa = idEmpresa } }, cancellationToken),
            MaestroExterno.Cultivos => await LeerAsync<CultivoExterno>("/api/Maestros/get-cultivos", new[] { new { idempresa = idEmpresa } }, cancellationToken),
            MaestroExterno.Campanias => await LeerAsync<CampaniaExterna>("/api/maestros/lista-campanias", new[] { new { ruc, aplicacion = AplicacionPlanta } }, cancellationToken),
            MaestroExterno.Acopios => await LeerAsync<AcopiosExterno>("/api/Maestros/get-acopios", new[] { new { idempresa = idEmpresa, aplicacion = AplicacionPlanta } }, cancellationToken),
            MaestroExterno.Variedades => await LeerAsync<VariedadExterna>("/api/Maestros/get-variedades", new[] { new { idempresa = idEmpresa, aplicacion = AplicacionPlanta } }, cancellationToken),
            MaestroExterno.Clientes => await LeerAsync<ClienteExterno>("/api/Maestros/clientes/listado", new[] { new { idempresa = idEmpresa, aplicacion = AplicacionPlanta } }, cancellationToken),
            MaestroExterno.Paises => await LeerAsync<PaisExterno>("/api/maestros/lista-paises", Array.Empty<object>(), cancellationToken),
            MaestroExterno.Calibres => await LeerAsync<CalibreExterno>("/api/maestros/lista-calibres", Array.Empty<object>(), cancellationToken),
            MaestroExterno.Transporte => await LeerAsync<TransporteExterno>("/api/maestros/lista-transporte", Array.Empty<object>(), cancellationToken),
            MaestroExterno.TiposClamshell => await LeerAsync<TipoClamshellExterno>("/api/maestros/lista-tiposClamshell", Array.Empty<object>(), cancellationToken),
            MaestroExterno.Empresas => await LeerAsync<EmpresaExterna>("/api/Maestros/get_empresas", new { }, cancellationToken),
            MaestroExterno.Usuarios => await LeerUsuariosAsync(usuario ?? string.Empty, ruc, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(maestro), maestro, null)
        };

        // Al frontend: camelCase (como respondia la API anterior). A un SP: nombres por defecto, que es lo que leen.
        return JsonSerializer.SerializeToElement(lista, formatoSp ? SpJsonOptions : JsonSerializerOptions.Web);
    }

    private static readonly JsonSerializerOptions SpJsonOptions = new();

    /// <summary>Lee la lista de un catalogo; acepta la respuesta directa o envuelta en { data } y elementos que vengan como texto JSON.</summary>
    private async Task<List<T>> LeerAsync<T>(string path, object body, CancellationToken cancellationToken)
    {
        var element = await PostAsync(path, body, cancellationToken);
        return ConvertirLista<T>(element);
    }

    /// <summary>lista-usuarios responde [{ error, data: [...], mensaje }]; se devuelven los usuarios del primer sobre.</summary>
    private async Task<List<ListaUsuariosExterno>> LeerUsuariosAsync(string usuario, string ruc, CancellationToken cancellationToken)
    {
        var element = await PostAsync("/api/maestros/lista-usuarios", new[] { new { usuario, ruc, aplicacion = AplicacionPlanta } }, cancellationToken);
        if (element.ValueKind == JsonValueKind.Array)
        {
            element = element.EnumerateArray().FirstOrDefault();
        }

        return element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var data)
            ? ConvertirLista<ListaUsuariosExterno>(data)
            : [];
    }

    private async Task<JsonElement> PostAsync(string path, object body, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(HttpClientName);
        using var response = await client.PostAsJsonAsync(path, body, JsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        return document.RootElement.Clone();
    }

    private static List<T> ConvertirLista<T>(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty("data", out var data))
        {
            element = data;
        }

        var lista = new List<T>();
        var items = element.ValueKind == JsonValueKind.Array ? element.EnumerateArray().ToList() : [element];
        foreach (var item in items)
        {
            var parsed = item.ValueKind switch
            {
                JsonValueKind.String when !string.IsNullOrWhiteSpace(item.GetString()) => JsonSerializer.Deserialize<T>(item.GetString()!, JsonOptions),
                JsonValueKind.Object => item.Deserialize<T>(JsonOptions),
                _ => default
            };
            if (parsed is not null)
            {
                lista.Add(parsed);
            }
        }

        return lista;
    }
}
