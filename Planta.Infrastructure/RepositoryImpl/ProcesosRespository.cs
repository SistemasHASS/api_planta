

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class ProcesosRespository : BaseRepository, IProcesosRepository
{

    public ProcesosRespository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarPaletsForProceso",
        new Dictionary<string, object?>
        {   
            {"@idempresa", idempresa},
            {"@ruc", ruc},
            { "@idproceso", idproceso }
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

    public async Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarPalets",
        new Dictionary<string, object?>
        {
            {"@idempresa", idempresa},
            {"@ruc", ruc},
            {"@usuario", usuario},
            {"@idRol", idRol},
            {"@Json", json}
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

    public async Task<List<JsonElement>> SincronizarDPaletsAsync(string idempresa, string ruc, string codigoAcopio, string usuario, string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarDPalets",
        new Dictionary<string, object?>
        {
            {"@idempresa", idempresa},
            {"@ruc", ruc},
            {"@codigoAcopio", codigoAcopio},
            {"@usuario", usuario},
            {"@json", json}
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

    public async Task<List<JsonElement>> SincronizarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string usuario, string idRol, string jsonProceso, string jsonDprocesoLogisticos, string jsonDprocesoSupervisores, string? modo = null)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarProceso",
        new Dictionary<string, object?>
        {
            { "@modo", modo},
            { "@idempresa", idempresa },
            { "@ruc", ruc },
            { "@idProyecto", idProyecto },
            { "@codigoCultivo", codigoCultivo },
            { "@codigoAcopio", codigoAcopio },
            { "@usuario", usuario },
            { "@idRol", idRol },
            { "@jsonProceso", jsonProceso },
            { "@jsonDprocesoLogisticos", jsonDprocesoLogisticos },
            { "@jsonDprocesoSupervisores", jsonDprocesoSupervisores }
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

    public async Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_BuscarProceso",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoCultivo", codigoCultivo },
                { "@codigoAcopio", codigoAcopio },
                { "@turno", turno },
                { "@fecha", fecha}
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

    public async Task<List<JsonElement>> ListarProcesosTodosAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarProcesosTodos",
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
    
    public async Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarProcesos",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@codigoCultivo", codigoCultivo },
                { "@codigoAcopio", codigoAcopio }
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

    public async Task<List<JsonElement>> GetSupervisoresDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarSupervisoresDisponibles",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idproyecto", idProyecto },
                { "@fecha", fecha }
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

    public async Task<List<JsonElement>> GetPersonalLogisticaDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarPersonalLogisticaDisponibles",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idproyecto", idProyecto },
                { "@fecha", fecha }
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

    public async Task<List<JsonElement>> ListarTiposClamshellPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarTiposClamshellPorMatriz",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoCultivo", codigoCultivo },
                { "@documentoConsignatario", documentoConsignatario },
                { "@destinoId", destinoId },
                { "@formatoId", formatoId },
                { "@tipoEmpaqueGuiaId", tipoEmpaqueGuiaId }
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

    public async Task<List<JsonElement>> ListarTiposCajaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarTiposCajaPorMatriz",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoCultivo", codigoCultivo },
                { "@documentoConsignatario", documentoConsignatario },
                { "@destinoId", destinoId },
                { "@formatoId", formatoId },
                { "@tipoEmpaqueGuiaId", tipoEmpaqueGuiaId }
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

    public async Task<List<JsonElement>> ListarPresentacionesPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarPresentacionesPorMatriz",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoCultivo", codigoCultivo },
                { "@documentoConsignatario", documentoConsignatario },
                { "@destinoId", destinoId },
                { "@formatoId", formatoId },
                { "@tipoEmpaqueGuiaId", tipoEmpaqueGuiaId }
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

    public async Task<List<JsonElement>> ListarTiposEmpaqueGuiaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarTiposEmpaqueGuiaPorMatriz",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoCultivo", codigoCultivo },
                { "@documentoConsignatario", documentoConsignatario },
                { "@destinoId", destinoId },
                { "@formatoId", formatoId }
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

    public async Task<List<JsonElement>> ListarFormatosPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarFormatosPorMatriz",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoCultivo", codigoCultivo },
                { "@documentoConsignatario", documentoConsignatario },
                { "@destinoId", destinoId }
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

    public async Task<List<JsonElement>> ListarDestinosPorMatrizCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string documentoConsignatario)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarDestinos_Por_MatrizCompatibilidad",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idProyecto", idProyecto },
                { "@documentoConsignatario", documentoConsignatario }
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

    public async Task<List<JsonElement>> ListarCodigosRanchoPorLugarProduccionAsync(string idempresa, string ruc, string idProyecto, int idLugaresDeProduccion)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarCodigosRanchoPorLugarProduccion",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@idproyecto", idProyecto },
                { "@idLugaresDeProduccion", idLugaresDeProduccion }
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

    public async Task<List<JsonElement>> ListarDPaletsPorAcopioAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarDPalets_Por_Acopio",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoAcopio", codigoAcopio }
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

    public async Task<List<JsonElement>> ListarTipoProcesoEmpacadoPorAcopioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarTipoProcesoEmpacadoPorAcopio",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@idProyecto", idProyecto },
                { "@codigoAcopio", codigoAcopio },
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

    public async Task<List<JsonElement>> ListarProcesosAbiertosConPaletsCerradosAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_ListarProcesosAbiertosConPaletsCerrados",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@codigoAcopio", codigoAcopio }
        }
        , result =>
        {
            var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

            var mensaje = result.IsDBNull(1) ? null : Convert.ToString(result.GetValue(1));

            JsonElement data;
            if (result.IsDBNull(2))
            {
                data = JsonSerializer.Deserialize<JsonElement>("null");
            }
            else
            {
                var dataStr = Convert.ToString(result.GetValue(2));
                data = string.IsNullOrWhiteSpace(dataStr)
                    ? JsonSerializer.Deserialize<JsonElement>("null")
                    : JsonSerializer.Deserialize<JsonElement>(dataStr);
            }

            var payload = new { error, data, mensaje };
            var payloadJson = JsonSerializer.Serialize(payload);
            return JsonSerializer.Deserialize<JsonElement>(payloadJson);
        });
    }

}






