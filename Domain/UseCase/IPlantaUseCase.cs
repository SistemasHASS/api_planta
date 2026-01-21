using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface IPlantaUseCase
    {
        Task<List<JsonElement>> ListarClientesAsync(string json);
        Task<List<JsonElement>> ListarMercadoDestinoAsync(string json);
        Task<List<JsonElement>> ListarFormatosAsync(string json);

    }
}
