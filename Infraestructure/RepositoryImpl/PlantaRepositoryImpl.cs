using api_planta.Domain.Repository;
using api_planta.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_planta.Infraestructure.RepositoryImpl
{
    public class PlantaRepositoryImpl : BaseRepository, IPlantaRepository
    {
        private readonly ApplicationDbContext context;
        public PlantaRepositoryImpl(ApplicationDbContext context) : base(context) {
            this.context = context;
        }
        public async Task<List<JsonElement>> ListarClientesAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "MAESTRO_importarClientes",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> ListarMercadoDestinoAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "MAESTRO_importarMercadoDestinos",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }
        public async Task<List<JsonElement>> ListarFormatosAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "MAESTRO_importarEnvase",
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
