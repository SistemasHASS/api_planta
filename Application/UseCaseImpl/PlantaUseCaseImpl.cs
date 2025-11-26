using api_planta.Domain.Repository;
using api_planta.Domain.UseCase;
using System.Text.Json;

namespace api_planta.Application.UseCaseImpl
{
    public class PlantaUseCaseImpl : IPlantaUseCase
    {
        private readonly IPlantaRepository transporteRepository;
        public PlantaUseCaseImpl(IPlantaRepository transporteRepository)
        {
            this.transporteRepository = transporteRepository;
        }
        public async Task<List<JsonElement>> ListarVehiculosAsync(string json)
        {
            return await this.transporteRepository.ListarVehiculosAsync(json);
        }

        public async Task<List<JsonElement>> ListarLocalidadAsync(string json)
        {
            return await this.transporteRepository.ListarLocalidadAsync(json);
        }

        public async Task<List<JsonElement>> ListarConductoresAsync(string json)
        {
            return await this.transporteRepository.ListarConductoresAsync(json);
        }

        public async Task<List<JsonElement>> GuardarViajesAsync(string json)
        {
            return await this.transporteRepository.GuardarViajesAsync(json);
        }
        public async Task<List<JsonElement>> ReporteViajesAsync(string json)
        {
            return await this.transporteRepository.ReporteViajesAsync(json);
        }

        public async Task<List<JsonElement>> ReporteViajesDetalladoAsync(string json)
        {
            return await this.transporteRepository.ReporteViajesDetalladoAsync(json);
        }

        public async Task<List<JsonElement>> RecuperarViajeAsync(string json)
        {
            return await this.transporteRepository.RecuperarViajeAsync(json);
        }
        public async Task<List<JsonElement>> RecuperarViajeControladorAsync(string json)
        {
            return await this.transporteRepository.RecuperarViajeControladorAsync(json);
        }
    }
}
