
using System.Text.Json;
using Planta.Application.GuiaRemision.Abstractions;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class GuiasRemisionRepository : BaseRepository, IGuiasRemisionRepository
{
    public GuiasRemisionRepository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<List<JsonElement>> SincronizarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarGuiasRemision",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@usuario", usuario },
                { "@idRol", idRol },
                { "@Json", json }
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

    public async Task<List<JsonElement>> ListarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string? estado,
        string? fechaDesde,
        string? fechaHasta,
        string? texto)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarGuiasRemision",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@usuario", usuario },
                { "@idRol", idRol },
                { "@estado", estado },
                { "@fechaDesde", fechaDesde },
                { "@fechaHasta", fechaHasta },
                { "@texto", texto }
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

    public async Task<List<JsonElement>> GetGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_GetGuiaRemision",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@codigoGuiaRemision", codigoGuiaRemision }
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

    public async Task<List<JsonElement>> EliminarGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario,
        string idRol)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_DeleteGuiaRemision",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@codigoGuiaRemision", codigoGuiaRemision },
                { "@usuario", usuario },
                { "@idRol", idRol }
        }
        , result =>
        {
            var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));
            var mensaje = result.FieldCount > 1 && !result.IsDBNull(1) ? Convert.ToString(result.GetValue(1)) : null;

            var payload = new { error, data = JsonSerializer.Deserialize<JsonElement>("null"), mensaje };
            var payloadJson = JsonSerializer.Serialize(payload);
            return JsonSerializer.Deserialize<JsonElement>(payloadJson);
        });
    }

    public async Task<List<JsonElement>> EmitirGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        var parametros = new Dictionary<string, object?>
        {
            { "@idempresa", idempresa },
            { "@ruc", ruc },
            { "@idProyecto", idProyecto },
            { "@codigoAcopio", codigoAcopio },
            { "@codigoGuiaRemision", codigoGuiaRemision },
            { "@usuario", usuario }
        };

        var emitirResult = await EjecutarStoredProcedureAsync("PLANTA_EmitirGuiaRemision",
            parametros,
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));
                var mensaje = result.IsDBNull(1) ? null : Convert.ToString(result.GetValue(1));

                var payload = new { error, data = JsonSerializer.Deserialize<JsonElement>("null"), mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });

        if (emitirResult.Count == 0)
        {
            var fallback = new { error = true, data = JsonSerializer.Deserialize<JsonElement>("null"), mensaje = "No se obtuvo respuesta del SP de emisión." };
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(fallback)) };
        }

        if (emitirResult[0].GetProperty("error").GetBoolean())
        {
            return emitirResult;
        }

        // Fire-and-forget: ejecutar cálculo de sobrepeso sin esperar respuesta
        _ = EjecutarStoredProcedureAsync("PLANTA_CalcularSobrepesoPorGuiaRemision",
            parametros,
            result =>
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

        return emitirResult;
    }

    public async Task<List<JsonElement>> AnularGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_AnularGuiaRemision",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@codigoGuiaRemision", codigoGuiaRemision },
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

    public async Task<List<JsonElement>> ListarCodigosCajaAsync(
        string idempresa,
        string ruc)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarCodigosCaja",
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
}
