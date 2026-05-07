using api_planta.Domain.Repository;
using api_planta.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
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

        public async Task<List<JsonElement>> SincronizarCategoriasAsync(string tabla, string json)
        {
            var parametros = new Dictionary<string, object?>
            {
                { "@Tabla", tabla },
                { "@Json", json }
            };

            return await EjecutarStoredProcedureAsync("SP_SincronizarCatalogo", parametros, result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                JsonElement data;
                if (result.IsDBNull(1))
                {
                    data = JsonSerializer.Deserialize<JsonElement>("null");
                }
                else
                {
                    var dataStr = Convert.ToString(result.GetValue(1));
                    data = string.IsNullOrWhiteSpace(dataStr)
                        ? JsonSerializer.Deserialize<JsonElement>("null")
                        : JsonSerializer.Deserialize<JsonElement>(dataStr);
                }

                var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));

                var payload = new { error, data, mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });
        }
    }
}