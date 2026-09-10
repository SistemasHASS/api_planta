using System.Text.Json;
using Planta.Application.GuiaRemisionManual.Abstractions;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class GuiasRemisionManualRepository : BaseRepository, IGuiasRemisionManualRepository
{
    public GuiasRemisionManualRepository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<List<JsonElement>> SincronizarGuiasRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_GuardarGuiaRemisionManual",
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

    public async Task<List<JsonElement>> ListarGuiasRemisionManualAsync(
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
        return await EjecutarStoredProcedureAsync("PLANTA_ListarGuiasRemisionManual",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@usuario", usuario },
                { "@idRol", idRol },
                { "@estado", estado ?? (object)DBNull.Value },
                { "@fechaDesde", fechaDesde ?? (object)DBNull.Value },
                { "@fechaHasta", fechaHasta ?? (object)DBNull.Value },
                { "@texto", texto ?? (object)DBNull.Value }
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

    public async Task<List<JsonElement>> ListarGuiasRemisionManualExcelAsync(
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
        return await EjecutarStoredProcedureAsync("PLANTA_ListarGuiasRemisionManualExcel",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@usuario", usuario },
                { "@idRol", idRol },
                { "@estado", estado ?? (object)DBNull.Value },
                { "@fechaDesde", fechaDesde ?? (object)DBNull.Value },
                { "@fechaHasta", fechaHasta ?? (object)DBNull.Value },
                { "@texto", texto ?? (object)DBNull.Value }
        },
        result =>
        {
            var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));
            var data = result.IsDBNull(1)
                ? JsonSerializer.Deserialize<JsonElement>("null")
                : JsonSerializer.Deserialize<JsonElement>(Convert.ToString(result.GetValue(1)) ?? "null");
            var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));
            var payload = new { error, data, mensaje };
            return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(payload));
        });
    }

    public async Task<List<JsonElement>> GetGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string idRol,
        string codigoGuiaRemision)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_GetGuiaRemisionManual",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@idRol", idRol },
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

    public async Task<List<JsonElement>> EditarGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_EditarGuiaRemisionManual",
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

    public async Task<List<JsonElement>> EliminarGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario,
        string idRol)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_EliminarGuiaRemisionManual",
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

    public async Task<List<JsonElement>> EmitirGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_EmitirGuiaRemisionManual",
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

    public async Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string? codigoEstadoSunat,
        string? estadoSunat,
        string? pdfFileUrl,
        string? xmlFileSignUrl,
        string? xmlFileSunatUrl,
        bool? enviadoBizlinks,
        string? respuestaBizlinks,
        string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ActualizarEstadoSunatGuiaRemisionManual",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
                { "@codigoGuiaRemision", codigoGuiaRemision },
                { "@codigoEstadoSunat", codigoEstadoSunat },
                { "@estadoSunat", estadoSunat },
                { "@pdfFileUrl", pdfFileUrl },
                { "@xmlFileSignUrl", xmlFileSignUrl },
                { "@xmlFileSunatUrl", xmlFileSunatUrl },
                { "@enviadoBizlinks", enviadoBizlinks },
                { "@respuestaBizlinks", respuestaBizlinks },
                { "@usuario", usuario }
        }
        , result =>
        {
            var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));
            var mensaje = result.IsDBNull(1) ? null : Convert.ToString(result.GetValue(1));

            var payload = new { error, mensaje };
            var payloadJson = JsonSerializer.Serialize(payload);
            return JsonSerializer.Deserialize<JsonElement>(payloadJson);
        });
    }

    public async Task<List<JsonElement>> AnularGuiaRemisionManualAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_AnularGuiaRemisionManual",
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
}
