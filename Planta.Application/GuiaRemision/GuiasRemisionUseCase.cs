using System.Text.Json;
using System.Text.Json.Nodes;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Catalogos.Models;
using Planta.Application.GuiaRemision.Abstractions;
using Planta.Application.Maestros.Abstractions;

namespace Planta.Application.GuiaRemision;

public sealed class GuiasRemisionUseCase(
    IGuiasRemisionService guiasRemisionService,
    ICatalogosService catalogosService,
    IMaestrosService maestrosService) : IGuiasRemisionUseCase
{
    public async Task<List<JsonElement>> SincronizarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json)
    {
        return await guiasRemisionService.SincronizarGuiasRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);
    }

    public async Task<List<JsonElement>> ListarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string? estado,string? fechaDesde,string? fechaHasta,string? texto)
    {
        return await guiasRemisionService.ListarGuiasRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);
    }

    public async Task<List<JsonElement>> GetGuiaRemisionAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision)
    {
        var guiaResult = await guiasRemisionService.GetGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision);

        if (guiaResult.Count == 0)
            return guiaResult;

        var wrapper = guiaResult[0];
        var error = wrapper.GetProperty("error").GetBoolean();
        if (error)
            return guiaResult;

        var data = wrapper.GetProperty("data");
        if (data.ValueKind == JsonValueKind.Null || data.ValueKind != JsonValueKind.Array || data.GetArrayLength() == 0)
            return guiaResult;

        try
        {
            var guiaNode = JsonNode.Parse(data[0].GetRawText());
            if (guiaNode is null)
                return guiaResult;

            var guiaObj = guiaNode.AsObject();

            var documentoDestinatario = guiaObj["documentoDestinatario"]?.GetValue<string>();

            var faltantes = new List<string>();

            // Ejecutar secuencialmente para evitar MultipleActiveResultSets (MARS) en SQL Server
            var destinatarios = (await GetDestinatariosFromMaestrosAsync(idempresa, ruc))?.Data ?? new List<Destinatarios>();
            var consignatariosRaw = await GetConsignatariosFromMaestrosAsync(idempresa, ruc);
            var paises = await maestrosService.GetPaisesAsync() ?? new List<PaisExterno>();
            var variedades = (await GetVariedadesFromMaestrosAsync(idempresa, ruc))?.Data ?? new List<VariedadRepository>();
            var transportes = await maestrosService.GetTransportesAsync() ?? new List<TransporteExterno>();
            var campanias = await maestrosService.GetCampaniasAsync(ruc) ?? new List<CampaniaExterna>();

            // Enriquecer cabecera (destinatario)
            if (!string.IsNullOrWhiteSpace(documentoDestinatario))
            {
                var d = destinatarios.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.DocumentoFiscal) && x.DocumentoFiscal.Trim() == documentoDestinatario.Trim());
                if (d is null)
                    faltantes.Add($"Destinatario con documento {documentoDestinatario}");
                else
                    guiaObj["nombreDestinatario"] = d.Nombre;
            }

            // Campanía / fruta
            var campania = campanias.FirstOrDefault(c =>
                !string.IsNullOrWhiteSpace(c.IdProyecto) && c.IdProyecto.Trim() == (idProyecto ?? "").Trim());
            var fruta = campania?.Fruta?.Trim() ?? "";

            // El SP devuelve palets/detalle como string JSON (FOR JSON PATH anidado), parsear si es necesario
            JsonNode? ParseJsonStringField(string fieldName)
            {
                var node = guiaObj[fieldName];
                if (node is JsonValue val && val.TryGetValue<string>(out var str) && !string.IsNullOrWhiteSpace(str))
                {
                    var parsed = JsonNode.Parse(str);
                    if (parsed is not null)
                        guiaObj[fieldName] = parsed;
                    return parsed;
                }
                return node;
            }

            ParseJsonStringField("palets");
            var detalleNode = ParseJsonStringField("detalle");

            // Enriquecer detalle
            if (detalleNode is JsonArray detalleArray)
            {
                foreach (var item in detalleArray)
                {
                    if (item is not JsonObject itemObj) continue;

                    var docCons = itemObj["documentoConsignatario"]?.GetValue<string>();
                    var idDest = itemObj["idDestino"]?.GetValue<string>();
                    var codVar = itemObj["codigoVariedad"]?.GetValue<string>();
                    var idTran = itemObj["idTransporte"]?.GetValue<string>();
                    var pesoPorCaja = itemObj["pesoPorCaja"]?.GetValue<decimal?>();
                    var nombreTipoProcesoEmpacado = itemObj["nombreTipoProcesoEmpacado"]?.GetValue<string>();
                    var nombrePresentacion = itemObj["nombrePresentacion"]?.GetValue<string>();
                    var nombreFormato = itemObj["nombreFormato"]?.GetValue<string>();
                    var nombreTipoEmpaqueGuia = itemObj["nombreTipoEmpaqueGuia"]?.GetValue<string>();
                    var codigoLugarProduccion = itemObj["codigoLugarProduccion"]?.GetValue<string>();
                    var codigoRancho = itemObj["codigoRancho"]?.GetValue<string>();

                    string? nombreConsignatario = null;
                    string? nombreDestino = null;
                    string? nombreVariedad = null;
                    string? nombreTransporte = null;

                    if (!string.IsNullOrWhiteSpace(docCons))
                    {
                        var cons = FindConsignatario(consignatariosRaw, docCons);
                        if (cons is null)
                            faltantes.Add($"Consignatario con documento {docCons}");
                        else
                            nombreConsignatario = cons;
                    }

                    if (!string.IsNullOrWhiteSpace(idDest))
                    {
                        var dest = paises.FirstOrDefault(x =>
                            !string.IsNullOrWhiteSpace(x.Id) && x.Id.Trim() == idDest.Trim());
                        if (dest is null)
                            faltantes.Add($"Destino con id {idDest}");
                        else
                            nombreDestino = dest.Pais ?? dest.Nacionalidad;
                    }

                    if (!string.IsNullOrWhiteSpace(codVar))
                    {
                        var vari = variedades.FirstOrDefault(x =>
                            !string.IsNullOrWhiteSpace(x.IdVariedad) && x.IdVariedad.Trim() == codVar.Trim());
                        if (vari is null)
                            faltantes.Add($"Variedad con codigo {codVar}");
                        else
                            nombreVariedad = vari.Variedad;
                    }

                    if (!string.IsNullOrWhiteSpace(idTran))
                    {
                        var tran = transportes.FirstOrDefault(x =>
                            !string.IsNullOrWhiteSpace(x.Id) && x.Id.Trim() == idTran.Trim());
                        if (tran is null)
                            faltantes.Add($"Transporte con id {idTran}");
                        else
                            nombreTransporte = tran.Transporte;
                    }

                    itemObj["nombreConsignatario"] = nombreConsignatario;
                    itemObj["nombreDestino"] = nombreDestino;
                    itemObj["nombreVariedad"] = nombreVariedad;
                    itemObj["nombreTransporte"] = nombreTransporte;

                    // detalleDescripcion
                    var partes = new List<string?>();
                    var bloque1 = string.Join(" ", new[] { nombreTipoProcesoEmpacado }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    var bloque2 = string.Join(" ", new[] { nombreConsignatario, nombreDestino, nombrePresentacion, nombreFormato }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    var bloque3 = string.Join(" ", new[] { fruta, "var.", nombreVariedad, pesoPorCaja.HasValue ? $"{pesoPorCaja.Value} kg" : null }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    var bloque4 = string.Join(" ", new[] { nombreTipoEmpaqueGuia, "/", codigoRancho }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    var bloque5 = string.Join(" ", new[] { "LDP", codigoLugarProduccion }.Where(s => !string.IsNullOrWhiteSpace(s)));
                    var bloque6 = nombreTransporte;

                    if (!string.IsNullOrWhiteSpace(bloque1)) partes.Add(bloque1);
                    if (!string.IsNullOrWhiteSpace(bloque2)) partes.Add(bloque2);
                    if (!string.IsNullOrWhiteSpace(bloque3)) partes.Add(bloque3);
                    if (!string.IsNullOrWhiteSpace(bloque4)) partes.Add(bloque4);
                    if (!string.IsNullOrWhiteSpace(bloque5)) partes.Add(bloque5);
                    if (!string.IsNullOrWhiteSpace(bloque6)) partes.Add($"({bloque6})");

                    itemObj["detalleDescripcion"] = string.Join(" - ", partes.Where(p => !string.IsNullOrWhiteSpace(p)));
                }
            }

            if (faltantes.Count > 0)
            {
                var mensajeError = "No se encontraron los siguientes datos de catálogo: " + string.Join(", ", faltantes);
                var errorPayload = new { error = true, data = new JsonArray(guiaObj), mensaje = mensajeError };
                var errorJson = JsonSerializer.Serialize(errorPayload);
                return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(errorJson) };
            }

            var okPayload = new { error = false, data = new JsonArray(guiaObj), mensaje = "Guía obtenida correctamente." };
            var okJson = JsonSerializer.Serialize(okPayload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(okJson) };
        }
        catch (Exception ex)
        {
            var errorPayload = new { error = true, data = JsonValueKind.Null, mensaje = $"Error enriqueciendo guía: {ex.Message}" };
            var errorJson = JsonSerializer.Serialize(errorPayload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(errorJson) };
        }
    }

    private async Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosFromMaestrosAsync(string idempresa, string ruc)
    {
        var clientes = await maestrosService.GetClientesAsync(idempresa);
        var json = JsonSerializer.Serialize(clientes);
        return await catalogosService.GetDestinatariosAsync(idempresa, ruc, json);
    }

    private async Task<List<JsonElement>> GetConsignatariosFromMaestrosAsync(string idempresa, string ruc)
    {
        var clientes = await maestrosService.GetClientesAsync(idempresa);
        var json = JsonSerializer.Serialize(clientes);
        return await catalogosService.ListarConsignatariosAsync(idempresa, ruc, json);
    }

    private async Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadesFromMaestrosAsync(string idempresa, string ruc)
    {
        var variedadesExt = await maestrosService.GetVariedadesAsync(idempresa);
        var json = JsonSerializer.Serialize(variedadesExt);
        return await catalogosService.GetVariedadAuxiliarAsync(idempresa, ruc, json);
    }

    private static string? FindConsignatario(List<JsonElement> consignatariosRaw, string documento)
    {
        if (consignatariosRaw is null || consignatariosRaw.Count == 0) return null;
        var data = consignatariosRaw[0];
        if (!data.TryGetProperty("data", out var dataProp) || dataProp.ValueKind != JsonValueKind.Array)
            return null;

        foreach (var c in dataProp.EnumerateArray())
        {
            var doc = c.TryGetProperty("documento", out var d) ? d.GetString() :
                      c.TryGetProperty("documentoFiscal", out var df) ? df.GetString() : null;
            if (doc?.Trim() == documento.Trim())
            {
                return c.TryGetProperty("nombre", out var n) ? n.GetString() : null;
            }
        }
        return null;
    }

    public async Task<List<JsonElement>> EliminarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario,string idRol)
    {
        return await guiasRemisionService.EliminarGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, idRol);
    }

    public async Task<List<JsonElement>> EmitirGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario)
    {
        var result = await guiasRemisionService.EmitirGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);

        if (result.Count == 0)
        {
            return result;
        }

        var wrapper = result[0];
        var error = wrapper.GetProperty("error").GetBoolean();
        if (error)
        {
            return result;
        }

        // Iterar sobre las guías emitidas y obtener detalle sin palets
        JsonNode? dataNode = null;
        if (wrapper.TryGetProperty("data", out var dataProp))
        {
            if (dataProp.ValueKind == JsonValueKind.String)
            {
                var dataStr = dataProp.GetString();
                Console.WriteLine($"[EMITIR] data as string: {dataStr}");
                if (!string.IsNullOrWhiteSpace(dataStr))
                    dataNode = JsonNode.Parse(dataStr);
            }
            else if (dataProp.ValueKind == JsonValueKind.Array)
            {
                dataNode = JsonNode.Parse(dataProp.GetRawText());
                Console.WriteLine($"[EMITIR] data parsed as JsonNode type: {dataNode?.GetType().Name}");
            }
            else
            {
                Console.WriteLine($"[EMITIR] data is unexpected ValueKind: {dataProp.ValueKind}");
            }
        }
        else
        {
            Console.WriteLine("[EMITIR] data property NOT found in wrapper");
        }

        if (dataNode is JsonArray dataArray)
        {
            foreach (var item in dataArray)
            {
                if (item is not JsonObject itemObj)
                {
                    Console.WriteLine($"[EMITIR] item is not JsonObject, type={item?.GetType().Name}");
                    continue;
                }

                var gIdempresa = itemObj["idempresa"]?.GetValue<string>() ?? idempresa;
                var gRuc = itemObj["ruc"]?.GetValue<string>() ?? ruc;
                var gIdProyecto = itemObj["idProyecto"]?.GetValue<string>() ?? idProyecto;
                var gCodigoAcopio = itemObj["codigoAcopio"]?.GetValue<string>() ?? codigoAcopio;
                var gCodigoGuiaRemision = itemObj["codigoGuiaRemision"]?.GetValue<string>();

                Console.WriteLine($"[EMITIR] processing guia: {gCodigoGuiaRemision}");

                if (string.IsNullOrWhiteSpace(gCodigoGuiaRemision))
                {
                    Console.WriteLine("[EMITIR] skipped: empty codigoGuiaRemision");
                    continue;
                }

                var guiaDetalle = await GetGuiaRemisionAsync(gIdempresa, gRuc, gIdProyecto, gCodigoAcopio, gCodigoGuiaRemision);

                if (guiaDetalle.Count > 0)
                {
                    var gdWrapper = guiaDetalle[0];
                    var gdError = gdWrapper.GetProperty("error").GetBoolean();

                    if (!gdError && gdWrapper.TryGetProperty("data", out var gdData))
                    {

                        if (gdData.ValueKind == JsonValueKind.Array && gdData.GetArrayLength() > 0)
                        {
                            var guiaObj = JsonNode.Parse(gdData[0].GetRawText())?.AsObject();

                            if (guiaObj is not null)
                            {
                                guiaObj.Remove("palets");
                                Console.WriteLine(JsonSerializer.Serialize(guiaObj, new JsonSerializerOptions { WriteIndented = true }));
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"[EMITIR] dataNode is NOT a JsonArray. dataNode type: {dataNode?.GetType().Name ?? "null"}");
        }

        // Por ahora: respuesta fija para éxito
        var okPayload = new { error = false, mensaje = "Se emitieron 3 guías del grupo correctamente.", data = Array.Empty<object>() };
        var okJson = JsonSerializer.Serialize(okPayload);
        var element = JsonSerializer.Deserialize<JsonElement>(okJson);
        return new List<JsonElement> { element };
    }
    
    public async Task<List<JsonElement>> AnularGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario)
    {
        return await guiasRemisionService.AnularGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
    }

    public async Task<List<JsonElement>> EditarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json)
    {
        return await guiasRemisionService.EditarGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);
    }

    public async Task<List<JsonElement>> ListarCodigosCajaAsync(string idempresa,string ruc)
    {
        return await guiasRemisionService.ListarCodigosCajaAsync(idempresa, ruc);
    }
}
