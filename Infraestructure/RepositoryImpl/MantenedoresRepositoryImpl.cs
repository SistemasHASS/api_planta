using api_planta.Domain.Repository;
using api_planta.Infraestructure.Data;
using System.Text.Json;

namespace api_planta.Infraestructure.RepositoryImpl
{
    public class MantenedoresRepositoryImpl : BaseRepository, IMantenedoresRepository
    {
        private readonly ApplicationDbContext context;

        public MantenedoresRepositoryImpl(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> EliminarVehiculoAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<JsonElement>> ListarLineasProduccionAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "PLANTA_listarlineasproduccion",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> CrudLineaProduccionAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "PLANTA_sincronizarlineasproduccion",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }
    }
}
