using api_planta.Domain.Repository;
using api_planta.Domain.UseCase;
using System.Text.Json;

namespace api_planta.Application.UseCaseImpl
{
    public class MantenedoresUseCaseImpl : IMantenedoresUseCase
    {
        private readonly IMantenedoresRepository mantenedoresRepository;

        public MantenedoresUseCaseImpl(IMantenedoresRepository mantenedoresRepository)
        {
            this.mantenedoresRepository = mantenedoresRepository;
        }

        public async Task<bool> EliminarVehiculoAsync(int id)
        {
            return await this.mantenedoresRepository.EliminarVehiculoAsync(id);
        }

        public async Task<List<JsonElement>> ListarLineasProduccionAsync(string json)
        {
            return await this.mantenedoresRepository.ListarLineasProduccionAsync(json);
        }

        public async Task<List<JsonElement>> CrudLineaProduccionAsync(string json)
        {
            return await this.mantenedoresRepository.CrudLineaProduccionAsync(json);
        }

        public async Task<List<JsonElement>> SincronizarConfiguracionLineasAsync(string json)
        {
            return await this.mantenedoresRepository.SincronizarConfiguracionLineasAsync(json);
        }

        public async Task<List<JsonElement>> ListarConfiguracionLineasProduccionAsync(string json)
        {
            return await this.mantenedoresRepository.ListarConfiguracionLineasProduccionAsync(json);
        }
    }
}
