using api_planta.Domain.Repository;
using api_planta.Infrastructure.Persistence;
using System.Text.Json;

namespace api_planta.Infrastructure.RepositoryImpl
{
    public class PaletRepositoryImpl : BaseRepository, IPaletRepository
    {
        public PaletRepositoryImpl(SistemaPaletsDbContext context) : base(context) { }

        public async Task<List<JsonElement>> CrearPaletAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_Crear", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerPaletsPorProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerPorProceso", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerPaletPorIdAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerPorId", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> AgregarCajasAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_AgregarCajas", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> CerrarPaletSaldoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_CerrarSaldo", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ReabrirPaletAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_Reabrir", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> EliminarPaletAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_Eliminar", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> EditarCajasAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_EditarCajas", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> EliminarComposicionAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_EliminarComposicion", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerDestinosPorConsignatarioAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerDestinosPorConsignatario", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerConsignatariosPorDestinoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("PALET_Consignatarios", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerFormatosAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerFormatos", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerTiposEmpaqueGuiaAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerTiposEmpaqueGuia", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerCalibresAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerCalibres", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerPresentacionesAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerPresentaciones", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerTiposDesdeMatrizAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerTiposDesdeMatriz", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerCodigosRanchoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerCodigosRancho", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> VerificarDriscollAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_VerificarDriscoll", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerVariedadesPorConsignatarioAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("PALET_Variedades", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }


        public async Task<List<JsonElement>> ObtenerConfigTipoProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Palet_ObtenerConfigTipoProceso", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        // Procesos
        public async Task<List<JsonElement>> ListarProcesosAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_Listar", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_ObtenerPorId", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> CrearProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_Crear", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> CerrarProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_Cerrar", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ReabrirProcesoAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_Reabrir", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ListarProcesosPorAcopioAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_ListarPorAcopio", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerPersonalDisponibleAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Proceso_ObtenerPersonalDisponible", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }

        public async Task<List<JsonElement>> ObtenerCampaniaActivaAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_Campania_ObtenerActiva", json, result =>
            {
                var jsonString = result.GetString(0);
                return JsonSerializer.Deserialize<JsonElement>(jsonString);
            });
        }
    }
}
