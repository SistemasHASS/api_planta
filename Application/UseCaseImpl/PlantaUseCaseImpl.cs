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

        public async Task<List<JsonElement>> ListarClientesAsync(string json)
        {
            return await this.transporteRepository.ListarClientesAsync(json);
        }
        public async Task<List<JsonElement>> ListarMercadoDestinoAsync(string json)
        {
            return await this.transporteRepository.ListarMercadoDestinoAsync(json);
        }
        public async Task<List<JsonElement>> ListarFormatosAsync(string json)
        {
            return await this.transporteRepository.ListarFormatosAsync(json);
        }

    }
}
