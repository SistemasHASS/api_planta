
using api_planta.Domain.Repository;
using System.Text.Json;

namespace api_planta.Domain.Services
{
    public interface ICatalogosService
    {
        Task<List<JsonElement>> ObtenerCatalogosAsync(string json);
        Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json);
        Task<List<JsonElement>> SincronizarCategoriasAsync(string tabla, string json);
    }
}