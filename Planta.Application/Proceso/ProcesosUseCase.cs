

using System.Text.Json;
using System.Text.Json.Nodes;
using Planta.Application.Catalogos.Models;
using Planta.Application.Maestros.Abstractions;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Application.Proceso;

public sealed class ProcesosUseCase(IProcesosService procesosService, IMaestrosService maestrosService) : IProcesosUseCase
{
    
    public async Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso)
    {
        return await procesosService.ListarPaletsForProcesoAsync(idempresa, ruc, idproceso);
    }
    
    public async Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    {
        return await procesosService.SincronizarPaletsAsync(idempresa, ruc, usuario, idRol, json);
    }
    
    public async Task<List<JsonElement>> SincronizarDPaletsAsync(string idempresa, string ruc, string codigoAcopio, string usuario, string json)
    {
        return await procesosService.SincronizarDPaletsAsync(idempresa, ruc, codigoAcopio, usuario, json);
    }
    
    public async Task<List<JsonElement>> SincronizarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string usuario, string idRol, string jsonProceso, string jsonDprocesoLogisticos, string jsonDprocesoSupervisores,string modo)
    {
        return await procesosService.SincronizarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, usuario, idRol, jsonProceso, jsonDprocesoLogisticos, jsonDprocesoSupervisores,modo);
    }
    
    public async Task<List<JsonElement>> ListarProcesosAsync(string idRol,string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    {   
        if(idRol == "ADPLA")
        {
            return await procesosService.ListarProcesosTodosAsync(idempresa, ruc, idProyecto, idCultivo);
        }
        return await procesosService.ListarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
    }
    
    public async Task<List<JsonElement>> GetSupervisoresDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await procesosService.GetSupervisoresDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    }
    
    public async Task<List<JsonElement>> GetPersonalLogisticaDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await procesosService.GetPersonalLogisticaDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    }
    
    public async Task<List<JsonElement>> ListarTipoProcesoEmpacadoPorAcopioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio)
    {
        return await procesosService.ListarTipoProcesoEmpacadoPorAcopioAsync(idempresa, ruc, idProyecto, codigoAcopio);
    }
    
    public async Task<List<JsonElement>> ListarDestinosPorMatrizCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string documentoConsignatario)
    {
        return await procesosService.ListarDestinosPorMatrizCompatibilidadAsync(idempresa, ruc, idProyecto, documentoConsignatario);
    }
    
    public async Task<List<JsonElement>> ListarFormatosPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId)
    {
        return await procesosService.ListarFormatosPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId);
    }
    
    public async Task<List<JsonElement>> ListarTiposEmpaqueGuiaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId)
    {
        return await procesosService.ListarTiposEmpaqueGuiaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId);
    }
    
    public async Task<List<JsonElement>> ListarPresentacionesPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await procesosService.ListarPresentacionesPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
    }
    
    public async Task<List<JsonElement>> ListarTiposCajaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId, int? presentacionId = null)
    {
        return await procesosService.ListarTiposCajaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId, presentacionId);
    }
    
    public async Task<List<JsonElement>> ListarTiposClamshellPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId, int? tipoCajaId = null, int? presentacionId = null)
    {
        return await procesosService.ListarTiposClamshellPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId, tipoCajaId, presentacionId);
    }
    
    public async Task<List<JsonElement>> ListarCodigosRanchoPorLugarProduccionAsync(string idempresa, string ruc, string idProyecto, int idLugaresDeProduccion)
    {
        return await procesosService.ListarCodigosRanchoPorLugarProduccionAsync(idempresa, ruc, idProyecto, idLugaresDeProduccion);
    }
    
    public async Task<List<JsonElement>> ListarDPaletsPorAcopioAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await procesosService.ListarDPaletsPorAcopioAsync(idempresa, ruc, codigoAcopio);
    }

    public async Task<List<JsonElement>> ListarDPaletsPorPaletAsync(string idempresa, string ruc, string idPalet)
    {
        return await procesosService.ListarDPaletsPorPaletAsync(idempresa, ruc, idPalet);
    }

    public async Task<List<JsonElement>> ObtenerDatosFichaComposicionPaletAsync(string idempresa, string ruc, string idPalet)
    {
        return await procesosService.ObtenerDatosFichaComposicionPaletAsync(idempresa, ruc, idPalet);
    }
    
    public async Task<List<JsonElement>> ListarProcesosAbiertosConPaletsCerradosAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await procesosService.ListarProcesosAbiertosConPaletsCerradosAsync(idempresa, ruc, codigoAcopio);
    }
    
    public async Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    {
        return await procesosService.BuscarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, turno, fecha);
    }

    public async Task<List<JsonElement>> ObtenerReporteDiarioAsync(string idempresa, string ruc, string fecha, string acopios, string idCampana)
    {
        var result = await procesosService.ObtenerReporteDiarioAsync(idempresa, ruc, fecha, acopios, idCampana);

        if (result.Count == 0)
        {
            return result;
        }

        var wrapper = result[0];
        if (!wrapper.TryGetProperty("error", out var errorElement))
        {
            return result;
        }

        var hasError = errorElement.ValueKind == JsonValueKind.True
                       || (errorElement.ValueKind != JsonValueKind.False && errorElement.GetBoolean());
        if (hasError)
        {
            return result;
        }

        if (!wrapper.TryGetProperty("data", out var dataElement) || dataElement.ValueKind != JsonValueKind.Object)
        {
            return result;
        }

        if (JsonNode.Parse(dataElement.GetRawText()) is not JsonObject dataNode)
        {
            return result;
        }

        var modified = false;

        if (dataNode["produccionPorVariedad"] is JsonArray produccionArray && produccionArray.Count > 0)
        {
            var variedadesLookup = BuildVariedadLookup(await maestrosService.GetVariedadesAsync(idempresa));
            if (variedadesLookup.Count > 0)
            {
                foreach (var entry in produccionArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var variedadId = row["variedadId"]?.GetValue<string>();
                    var codigoCultivo = row["codigoCultivo"]?.GetValue<string>();
                    var nombreActual = row["variedad"]?.GetValue<string>();

                    var key = BuildVariedadKey(codigoCultivo, variedadId);
                    string? nuevoNombre = null;

                    if (key is not null && variedadesLookup.TryGetValue(key, out var nombreVariedad) && !string.IsNullOrWhiteSpace(nombreVariedad))
                    {
                        nuevoNombre = nombreVariedad;
                    }
                    else if (!string.IsNullOrWhiteSpace(variedadId) && string.IsNullOrWhiteSpace(nombreActual))
                    {
                        nuevoNombre = variedadId;
                    }

                    if (nuevoNombre is not null && !string.Equals(nombreActual, nuevoNombre, StringComparison.OrdinalIgnoreCase))
                    {
                        row["variedad"] = nuevoNombre;
                        modified = true;
                    }
                }
            }
        }

        if (dataNode["avancePorConsignatario"] is JsonArray consignatarioArray && consignatarioArray.Count > 0)
        {
            var clientesLookup = BuildClienteLookup(await maestrosService.GetClientesAsync(idempresa));
            if (clientesLookup.Count > 0)
            {
                foreach (var entry in consignatarioArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var consignatarioId = row["consignatarioId"]?.GetValue<string>();
                    var nombreActual = row["consignatario"]?.GetValue<string>();

                    if (string.IsNullOrWhiteSpace(consignatarioId))
                    {
                        continue;
                    }

                    string? nuevoNombre = null;

                    if (clientesLookup.TryGetValue(consignatarioId.Trim(), out var nombreConsignatario) && !string.IsNullOrWhiteSpace(nombreConsignatario))
                    {
                        nuevoNombre = nombreConsignatario;
                    }
                    else if (string.IsNullOrWhiteSpace(nombreActual))
                    {
                        nuevoNombre = consignatarioId;
                    }

                    if (nuevoNombre is not null && !string.Equals(nombreActual, nuevoNombre, StringComparison.OrdinalIgnoreCase))
                    {
                        row["consignatario"] = nuevoNombre;
                        modified = true;
                    }
                }
            }
        }

        if (!modified)
        {
            return result;
        }

        if (JsonNode.Parse(wrapper.GetRawText()) is not JsonObject wrapperNode)
        {
            return result;
        }

        wrapperNode["data"] = dataNode;
        result[0] = JsonSerializer.Deserialize<JsonElement>(wrapperNode.ToJsonString());
        return result;
    }

    public async Task<List<JsonElement>> ObtenerReporteSemanalFiltrosAsync(string idempresa, string ruc, string idProyecto)
    {
        var result = await procesosService.ObtenerReporteSemanalFiltrosAsync(idempresa, ruc, idProyecto);

        if (result.Count == 0)
        {
            return result;
        }

        var wrapper = result[0];
        if (!wrapper.TryGetProperty("error", out var errorElement))
        {
            return result;
        }

        var hasError = errorElement.ValueKind == JsonValueKind.True
                       || (errorElement.ValueKind != JsonValueKind.False && errorElement.GetBoolean());
        if (hasError)
        {
            return result;
        }

        if (!wrapper.TryGetProperty("data", out var dataElement) || dataElement.ValueKind != JsonValueKind.Object)
        {
            return result;
        }

        if (JsonNode.Parse(dataElement.GetRawText()) is not JsonObject dataNode)
        {
            return result;
        }

        var enriched = await EnriquecerFiltros(dataNode, idempresa);
        if (!enriched)
        {
            return result;
        }

        if (JsonNode.Parse(wrapper.GetRawText()) is not JsonObject wrapperNode)
        {
            return result;
        }

        wrapperNode["data"] = dataNode;
        result[0] = JsonSerializer.Deserialize<JsonElement>(wrapperNode.ToJsonString());
        return result;
    }

    public async Task<List<JsonElement>> ObtenerReporteSemanalDatosAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string? semanas,
        string? variedades,
        string? formatos,
        string? destinos,
        string? clientes,
        string? consignatarios)
    {
        var result = await procesosService.ObtenerReporteSemanalDatosAsync(
            idempresa, ruc, idProyecto, semanas, variedades, formatos, destinos, clientes, consignatarios);

        if (result.Count == 0)
        {
            return result;
        }

        var wrapper = result[0];
        if (!wrapper.TryGetProperty("error", out var errorElement))
        {
            return result;
        }

        var hasError = errorElement.ValueKind == JsonValueKind.True
                       || (errorElement.ValueKind != JsonValueKind.False && errorElement.GetBoolean());
        if (hasError)
        {
            return result;
        }

        if (!wrapper.TryGetProperty("data", out var dataElement) || dataElement.ValueKind != JsonValueKind.Object)
        {
            return result;
        }

        if (JsonNode.Parse(dataElement.GetRawText()) is not JsonObject dataNode)
        {
            return result;
        }

        var enriched = await EnriquecerDetalleVariedades(dataNode, idempresa);
        if (!enriched)
        {
            return result;
        }

        if (JsonNode.Parse(wrapper.GetRawText()) is not JsonObject wrapperNode)
        {
            return result;
        }

        wrapperNode["data"] = dataNode;
        result[0] = JsonSerializer.Deserialize<JsonElement>(wrapperNode.ToJsonString());
        return result;
    }

    public async Task<List<JsonElement>> ObtenerReporteCampaniaDatosAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string? semanas,
        string? variedades,
        string? formatos,
        string? destinos,
        string? clientes,
        string? consignatarios)
    {
        var result = await procesosService.ObtenerReporteCampaniaDatosAsync(
            idempresa, ruc, idProyecto, semanas, variedades, formatos, destinos, clientes, consignatarios);

        if (result.Count == 0)
        {
            return result;
        }

        var wrapper = result[0];
        if (!wrapper.TryGetProperty("error", out var errorElement))
        {
            return result;
        }

        var hasError = errorElement.ValueKind == JsonValueKind.True
                       || (errorElement.ValueKind != JsonValueKind.False && errorElement.GetBoolean());
        if (hasError)
        {
            return result;
        }

        if (!wrapper.TryGetProperty("data", out var dataElement) || dataElement.ValueKind != JsonValueKind.Object)
        {
            return result;
        }

        if (JsonNode.Parse(dataElement.GetRawText()) is not JsonObject dataNode)
        {
            return result;
        }

        var enriched = await EnriquecerDetalleVariedades(dataNode, idempresa);
        if (!enriched)
        {
            return result;
        }

        if (JsonNode.Parse(wrapper.GetRawText()) is not JsonObject wrapperNode)
        {
            return result;
        }

        wrapperNode["data"] = dataNode;
        result[0] = JsonSerializer.Deserialize<JsonElement>(wrapperNode.ToJsonString());
        return result;
    }

    private async Task<bool> EnriquecerDetalleVariedades(JsonObject dataNode, string idempresa)
    {
        var modified = false;

        if (dataNode["detalleVariedad"] is JsonArray detalleArray && detalleArray.Count > 0)
        {
            var variedadesLookup = BuildVariedadLookup(await maestrosService.GetVariedadesAsync(idempresa));
            if (variedadesLookup.Count > 0)
            {
                foreach (var entry in detalleArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var variedadId = row["variedadId"]?.GetValue<string>();
                    var codigoCultivo = row["codigoCultivo"]?.GetValue<string>();
                    var nombreActual = row["variedad"]?.GetValue<string>();

                    var key = BuildVariedadKey(codigoCultivo, variedadId);
                    string? nuevoNombre = null;

                    if (key is not null && variedadesLookup.TryGetValue(key, out var nombreVariedad) && !string.IsNullOrWhiteSpace(nombreVariedad))
                    {
                        nuevoNombre = nombreVariedad;
                    }
                    else if (!string.IsNullOrWhiteSpace(variedadId) && string.IsNullOrWhiteSpace(nombreActual))
                    {
                        nuevoNombre = variedadId;
                    }

                    if (nuevoNombre is not null && !string.Equals(nombreActual, nuevoNombre, StringComparison.OrdinalIgnoreCase))
                    {
                        row["variedad"] = nuevoNombre;
                        modified = true;
                    }
                }
            }
        }

        return modified;
    }

    private async Task<bool> EnriquecerFiltros(JsonObject dataNode, string idempresa)
    {
        var modified = false;

        if (dataNode["variedades"] is JsonArray variedadesArray && variedadesArray.Count > 0)
        {
            var variedadesLookup = BuildVariedadLookup(await maestrosService.GetVariedadesAsync(idempresa));
            if (variedadesLookup.Count > 0)
            {
                foreach (var entry in variedadesArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var variedadId = row["id"]?.GetValue<string>();
                    var codigoCultivo = row["codigoCultivo"]?.GetValue<string>();
                    var nombreActual = row["nombre"]?.GetValue<string>();

                    var key = BuildVariedadKey(codigoCultivo, variedadId);
                    string? nuevoNombre = null;

                    if (key is not null && variedadesLookup.TryGetValue(key, out var nombreVariedad) && !string.IsNullOrWhiteSpace(nombreVariedad))
                    {
                        nuevoNombre = nombreVariedad;
                    }
                    else if (!string.IsNullOrWhiteSpace(variedadId) && string.IsNullOrWhiteSpace(nombreActual))
                    {
                        nuevoNombre = variedadId;
                    }

                    if (nuevoNombre is not null && !string.Equals(nombreActual, nuevoNombre, StringComparison.Ordinal))
                    {
                        row["nombre"] = nuevoNombre;
                        modified = true;
                    }
                }
            }
        }

        if (dataNode["destinos"] is JsonArray destinosArray && destinosArray.Count > 0)
        {
            var destinosLookup = BuildPaisLookup(await maestrosService.GetPaisesAsync());
            if (destinosLookup.Count > 0)
            {
                foreach (var entry in destinosArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var destinoId = row["id"]?.GetValue<string>()?.Trim();
                    if (string.IsNullOrWhiteSpace(destinoId))
                    {
                        continue;
                    }

                    if (destinosLookup.TryGetValue(destinoId, out var nombreDestino) && !string.IsNullOrWhiteSpace(nombreDestino))
                    {
                        var nombreActual = row["nombre"]?.GetValue<string>();
                        if (!string.Equals(nombreActual, nombreDestino, StringComparison.Ordinal))
                        {
                            row["nombre"] = nombreDestino;
                            modified = true;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(row["nombre"]?.GetValue<string>()))
                    {
                        row["nombre"] = destinoId;
                        modified = true;
                    }
                }
            }
        }

        if (dataNode["consignatarios"] is JsonArray consignatariosArray && consignatariosArray.Count > 0)
        {
            var clientesLookup = BuildClienteLookup(await maestrosService.GetClientesAsync(idempresa));
            if (clientesLookup.Count > 0)
            {
                foreach (var entry in consignatariosArray)
                {
                    if (entry is not JsonObject row)
                    {
                        continue;
                    }

                    var documento = row["id"]?.GetValue<string>()?.Trim();
                    if (string.IsNullOrWhiteSpace(documento))
                    {
                        continue;
                    }

                    if (clientesLookup.TryGetValue(documento, out var nombreConsignatario) && !string.IsNullOrWhiteSpace(nombreConsignatario))
                    {
                        var nombreActual = row["nombre"]?.GetValue<string>();
                        if (!string.Equals(nombreActual, nombreConsignatario, StringComparison.Ordinal))
                        {
                            row["nombre"] = nombreConsignatario;
                            modified = true;
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(row["nombre"]?.GetValue<string>()))
                    {
                        row["nombre"] = documento;
                        modified = true;
                    }
                }
            }
        }

        return modified;
    }

    private static Dictionary<string, string> BuildVariedadLookup(IReadOnlyList<VariedadExterna>? variedades)
    {
        var lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (variedades is null)
        {
            return lookup;
        }

        foreach (var variedad in variedades)
        {
            var key = BuildVariedadKey(variedad?.IdCultivo, variedad?.IdVariedad);
            if (key is null)
            {
                continue;
            }

            var nombre = (variedad?.Variedad ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                nombre = (variedad?.IdVariedad ?? string.Empty).Trim();
            }

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                lookup[key] = nombre;
            }
        }

        return lookup;
    }

    private static Dictionary<string, string> BuildPaisLookup(IReadOnlyList<PaisExterno>? paises)
    {
        var lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (paises is null)
        {
            return lookup;
        }

        foreach (var pais in paises)
        {
            var id = pais?.Id?.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                continue;
            }

            var nombre = (pais?.Pais ?? pais?.Nacionalidad ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                nombre = id;
            }

            lookup[id] = nombre;
        }

        return lookup;
    }

    private static Dictionary<string, string> BuildClienteLookup(IReadOnlyList<ClienteExterno>? clientes)
    {
        var lookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (clientes is null)
        {
            return lookup;
        }

        foreach (var cliente in clientes)
        {
            var nombre = (cliente?.Nombre ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                nombre = (cliente?.Documento ?? cliente?.DocumentoFiscal ?? string.Empty).Trim();
            }

            if (string.IsNullOrWhiteSpace(nombre))
            {
                continue;
            }

            AddKey(cliente?.DocumentoFiscal, nombre);
            AddKey(cliente?.Documento, nombre);
        }

        return lookup;

        void AddKey(string? key, string value)
        {
            var normalized = key?.Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                return;
            }

            lookup[normalized] = value;
        }
    }

    private static string? BuildVariedadKey(string? idCultivo, string? idVariedad)
    {
        var cultivo = idCultivo?.Trim();
        var variedad = idVariedad?.Trim();

        if (string.IsNullOrWhiteSpace(cultivo) || string.IsNullOrWhiteSpace(variedad))
        {
            return null;
        }

        return $"{cultivo}|{variedad}";
    }
}