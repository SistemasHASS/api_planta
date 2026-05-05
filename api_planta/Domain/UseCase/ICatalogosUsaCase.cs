
using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface ICatalogosUsaCase
    {
        Task<List<JsonElement>> ObtenerCatalogosAsync(string json);
        Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json);
    }
}