using api_planta.Domain.Repository;
using api_planta.Infrastructure.Persistence;
using System.Text.Json;

namespace api_planta.Infrastructure.RepositoryImpl
{
    public class CatalogosRepositoryImpl : BaseRepository, ICatalogosRepository
    {
        public CatalogosRepositoryImpl(SistemaPaletsDbContext context) : base(context){}
        public async Task<List<JsonElement>> ObtenerCatalogosAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("PALET_ObtenerCatalogos", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerCatalogosOperariosAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("PALET_ObtenerCatalogosOperativos", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }
    }
}