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
        public async Task<List<JsonElement>> ListarVehiculosAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANS_listado_vehiculos",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> ListarLocalidadAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANS_listado_localidades",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }
        public async Task<List<JsonElement>> ListarConductoresAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANS_listado_conductor",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> GuardarViajesAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANSPORTE_registrarviajes",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> ReporteViajesAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANSPORTE_reporteviajes",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> ReporteViajesDetalladoAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANSPORTE_reporteviajesdetallado",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }

        public async Task<List<JsonElement>> RecuperarViajeAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANSPORTE_recuperarviajes",
                json,
                result =>
                {
                    var jsonString = result.GetString(0);
                    return JsonSerializer.Deserialize<JsonElement>(jsonString);
                },
                parametrosRequeridos: true);

            return lista;
        }
        public async Task<List<JsonElement>> RecuperarViajeControladorAsync(string json)
        {
            var lista = await EjecutarStoredProcedureAsync<JsonElement>(
                "TRANSPORTE_recuperarviajescontrolador",
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
