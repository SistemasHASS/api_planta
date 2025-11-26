using System.Text.Json;

namespace api_planta.Domain.Repository
{
    public interface IPlantaRepository
    {
        Task<List<JsonElement>> ListarVehiculosAsync(string json);
        Task<List<JsonElement>> ListarLocalidadAsync(string json);
        Task<List<JsonElement>> ListarConductoresAsync(string json);
        Task<List<JsonElement>> GuardarViajesAsync(string json);
        Task<List<JsonElement>> ReporteViajesAsync(string json);
        Task<List<JsonElement>> ReporteViajesDetalladoAsync(string json);
        Task<List<JsonElement>> RecuperarViajeAsync(string json);
        Task<List<JsonElement>> RecuperarViajeControladorAsync(string json);
    }
}
