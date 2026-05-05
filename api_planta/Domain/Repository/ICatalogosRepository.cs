
using System.Text.Json;

namespace api_planta.Domain.Repository
{
    public interface ICatalogosRepository
    {
        Task<List<JsonElement>> ObtenerCatalogosAsync(string json);
        Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json);
    }
}
