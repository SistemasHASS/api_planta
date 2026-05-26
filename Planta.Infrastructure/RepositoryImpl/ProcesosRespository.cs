

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class ProcesosRespository : BaseRepository, IProcesosRepository
{

    public ProcesosRespository(SistemaPaletsDbContext context) : base(context)
    {
    }

      public async Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarProcesos", 
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@idCultivo", idCultivo },
                { "@acopioId", acopioId }
        }
        , result =>
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






