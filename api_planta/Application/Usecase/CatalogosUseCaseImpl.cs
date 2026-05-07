using api_planta.Domain.UseCase;
using api_planta.Domain.Services;
using System.Text.Json;

namespace api_planta.Application.Usecase
{
    public class CatalogosUseCaseImpl : ICatalogosUsaCase
    {
        private readonly ICatalogosService _service;

        public CatalogosUseCaseImpl(ICatalogosService service)
        {
            _service = service;
        }

        public async Task<List<JsonElement>> ObtenerCatalogosAsync(string json)
        {
            return await _service.ObtenerCatalogosAsync(json);
        }
        
        public async Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json)
        {
            return await _service.ObtenerCatalogosOperariosAsync(json);
        }

        public async Task<List<JsonElement>> SincronizarCategoriasAsync(string tabla, string json)
        {
            return await _service.SincronizarCategoriasAsync(tabla, json);
        }
    }
}