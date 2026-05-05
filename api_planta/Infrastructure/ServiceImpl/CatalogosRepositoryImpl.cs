
using api_planta.Domain.Repository;
using api_planta.Domain.Services;
using System.Text.Json;

namespace api_planta.Infrastructure.ServiceImpl
{
    public class CatalogosServiceImpl : ICatalogosService
    {
        private readonly ICatalogosRepository _repository;

        public CatalogosServiceImpl(ICatalogosRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JsonElement>> ObtenerCatalogosAsync(string json)
        {
            return await _repository.ObtenerCatalogosAsync(json);
        }

        public async Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json)
        {
            return await _repository.ObtenerCatalogosOperariosAsync(json);
        }
    }
}