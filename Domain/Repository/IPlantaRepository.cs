using System.Text.Json;

namespace api_planta.Domain.Repository
{
    public interface IPlantaRepository
    {
        Task<List<JsonElement>> ListarClientesAsync(string json);
        Task<List<JsonElement>> ListarMercadoDestinoAsync(string json);
        Task<List<JsonElement>> ListarFormatosAsync(string json);
    }
}
