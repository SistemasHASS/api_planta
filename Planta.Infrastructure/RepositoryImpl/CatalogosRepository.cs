
using System.Text.Json;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Catalogos.Models;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class CatalogosRepository : BaseRepository, ICatalogosRepository
{

    public CatalogosRepository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosAsync(string idempresa, string ruc, string json)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_GETDestinatarios",
            new Dictionary<string, object?>
            {   
                {"@idempresa", idempresa},
                {"@ruc", ruc},
                { "@json", json }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Destinatarios>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Destinatarios>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Destinatarios>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Destinatarios>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<List<JsonElement>> SincronizarAcopiosAsync(string idempresa, string ruc, string usuario, string json, string json_detalle)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarAcopio_Config",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
                { "@usuario", usuario },
                { "@json", json },
                { "@json_detalle", json_detalle}
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

    public async Task<CatalogosResponse<List<TipoProcesoEmpacado>>> GetTipoProcesoEmpacadoAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTipoProcesoEmpacado",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@idproyecto", idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<TipoProcesoEmpacado>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<TipoProcesoEmpacado>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<TipoProcesoEmpacado>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<TipoProcesoEmpacado>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Formato>>> GetFormatosAsync(string idempresa, string ruc, string codigoCultivo)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarFormatos",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@codigoCultivo", codigoCultivo }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Formato>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Formato>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Formato>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Formato>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }


    public async Task<CatalogosResponse<List<TipoClamshell>>> GetTiposClamshellAsync(string idempresa, string ruc, string codigoCultivo)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTiposClamshell",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@codigoCultivo", codigoCultivo }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<TipoClamshell>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<TipoClamshell>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<TipoClamshell>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<TipoClamshell>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<List<JsonElement>> SincronizarCatalogosAsync(string tabla, string json, string idempresa, string ruc, string usuario)
    {
        var parametros = new Dictionary<string, object?>
            {
                { "@Tabla", tabla },
                { "@Json", json },
                { "@Idempresa", idempresa },
                { "@Ruc", ruc },
                { "@Usuario", usuario }
            };
        Console.WriteLine($"Parametros: {JsonSerializer.Serialize(parametros)}");
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarCatalogo", parametros, result =>
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

    public async Task<CatalogosResponse<List<UsuariosAcopio>>> GetUsuarioAcopioAsync(string json)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListaUsuariosAcopio",
            new Dictionary<string, object?>
            {
                { "@json", json }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<UsuariosAcopio>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<UsuariosAcopio>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<UsuariosAcopio>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<UsuariosAcopio>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<PersonalLogistico>>> GetPersonalLogisticoAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarPersonalLogistica",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@idproyecto", idproyecto}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<PersonalLogistico>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<PersonalLogistico>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<PersonalLogistico>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<PersonalLogistico>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Supervisor>>> GetSupervisoresAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarSupervisores",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@idproyecto", idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Supervisor>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Supervisor>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Supervisor>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Supervisor>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Transportista>>> GetTransportistasAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTransportistas",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@idproyecto", idproyecto}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Transportista>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Transportista>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Transportista>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Transportista>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Vehiculo>>> GetVehiculosAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarVehiculos",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@idproyecto", @idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Vehiculo>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Vehiculo>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Vehiculo>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Vehiculo>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Conductores>>> GetConductoresAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarConductores",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@idproyecto", idproyecto}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Conductores>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Conductores>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Conductores>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Conductores>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<LugaresProduccion>>> GetLugaresProduccionAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarLugaresProduccion",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@idproyecto", idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<LugaresProduccion>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<LugaresProduccion>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<LugaresProduccion>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<LugaresProduccion>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<TipoCaja>>> GetTipoCajaAsync(string idempresa, string ruc, string codigoCultivo)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTiposCaja",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@codigoCultivo", codigoCultivo}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<TipoCaja>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<TipoCaja>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<TipoCaja>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<TipoCaja>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<Presentacion>>> GetPresentacionesAsync(string idempresa, string ruc, string codigoCultivo)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarPresentacion",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@codigoCultivo", codigoCultivo }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Presentacion>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Presentacion>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Presentacion>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Presentacion>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<TiposEmpaqueGuia>>> GetTiposEmpaqueGuiaAsync(string idempresa, string ruc, string codigoCultivo)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTiposEmpaqueGuia",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@codigoCultivo", codigoCultivo }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<TiposEmpaqueGuia>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<TiposEmpaqueGuia>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<TiposEmpaqueGuia>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<TiposEmpaqueGuia>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };

    }

    public async Task<CatalogosResponse<List<Categoria>>> GetCategoriaAsync(string idempresa, string ruc, string codigoCultivo)
    {

        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarCategorias",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@codigoCultivo", codigoCultivo}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Categoria>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Categoria>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Categoria>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Categoria>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };

    }

    public async Task<CatalogosResponse<List<TiposEmpaque>>> GetTiposEmpaquesAsync(string idempresa, string ruc, string codigoCultivo)
    {

        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarTiposEmpaque",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc},
                { "@codigoCultivo", codigoCultivo}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<TiposEmpaque>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<TiposEmpaque>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<TiposEmpaque>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<TiposEmpaque>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };

    }

    public async Task<CatalogosResponse<List<Acopios>>> GetAcopiosSeriesAsync(string idempresa, string json)
    {

        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_GETAcopio_Config",
            new Dictionary<string, object?>
            {
                { "@json", json },
                { "@idempresa", idempresa }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<Acopios>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<Acopios>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<Acopios>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<Acopios>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };

    }

    public async Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadAuxiliarAsync(string idempresa, string ruc, string json)
    {

        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_GetVariedadAuxiliar",
            new Dictionary<string, object?>
            {
                { "@json", json },
                { "@idempresa", idempresa },
                { "@ruc", ruc}
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<VariedadRepository>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<VariedadRepository>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<VariedadRepository>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<VariedadRepository>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };

    }

    public async Task<CatalogosResponse<List<CodigoRancho>>> GetCodigosRanchoAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarCodigosRancho",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@idproyecto", idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<CodigoRancho>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<CodigoRancho>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<CodigoRancho>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<CodigoRancho>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<CatalogosResponse<List<LugarProduccionConfig>>> GetLugaresProduccionConfigAsync(string idempresa, string ruc, string idproyecto)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_ListarLugaresProduccion_Confg",
            new Dictionary<string, object?>
            {
                { "@idempresa", idempresa },
                { "@ruc",  ruc },
                { "@idproyecto", idproyecto }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<LugarProduccionConfig>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<LugarProduccionConfig>>(dataStr);
                    }
                }

                return new CatalogosResponse<List<LugarProduccionConfig>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new CatalogosResponse<List<LugarProduccionConfig>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }

    public async Task<List<JsonElement>> SincronizarDestinatariosAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    {
        return await EjecutarStoredProcedureAsync("PLANTA_SincronizarDestinatarios",
        new Dictionary<string, object?>
        {
                { "@idempresa", idempresa },
                { "@ruc", ruc },
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

}






