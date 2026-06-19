

using System.Text.Json;
using Planta.Application.Administracion.Abstractions;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class AdministracionRepository : BaseRepository, IAdministracionRepository
{

    public AdministracionRepository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<List<JsonElement>> SincronizarReglasSobrepesoAsync(string json, string idempresa, string ruc,string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarReglasSobrepeso", 
        new Dictionary<string, object?>
        {
                { "@json", json },
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@usuario", usuario }
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

    public async Task<List<JsonElement>> ListarReglasSobrepesoAsync(string idempresa, string ruc, string? idProyecto=null, string? codigoCultivo = null)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_Listar_ReglasSobrepeso",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoCultivo", codigoCultivo }
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

    public async Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json, string idempresa, string ruc,string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarMatrizCompatibilidad", 
        new Dictionary<string, object?>
        {
                { "@json", json },
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@usuario", usuario }
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

    public async Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string idempresa, string ruc, string? idProyecto=null, string? idCultivo = null)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_Listar_MatrizCompatibilidad",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoCultivo", idCultivo }
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


    public async Task<List<JsonElement>> ListarCorrelativosDocumentosAsync(string idempresa, string ruc)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarCorrelativosDocumentos",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc }
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

    public async Task<List<JsonElement>> SincronizarCorrelativosDocumentosAsync(string json, string idempresa, string ruc, string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarCorrelativosDocumentos",
        new Dictionary<string, object?>
        {
                { "@Json", json },
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@usuario", usuario }
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

    public async Task<List<JsonElement>> SincronizarUsuariosAsync(string json, string idempresa, string ruc, string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarUsuarios",
        new Dictionary<string, object?>
        {
                { "@json", json },
                { "@usuarioCre", usuario },
                { "@IdempresaBack", idempresa },
                { "@RucBack", ruc },
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