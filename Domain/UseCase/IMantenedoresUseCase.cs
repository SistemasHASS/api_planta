using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface IMantenedoresUseCase
    {
        Task<bool> EliminarVehiculoAsync(int id);
        Task<List<JsonElement>> ListarLineasProduccionAsync(string json);
        Task<List<JsonElement>> CrudLineaProduccionAsync(string json);
    }
}
