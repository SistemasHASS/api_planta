
using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface ICatalogosUsaCase
    {
        Task<List<JsonElement>> ObtenerCatalogosAsync(string json);
        Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json);
        Task<List<JsonElement>> SincronizarCategoriasAsync(string tabla, string json);
    }
}