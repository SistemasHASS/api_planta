using System.Text.Json;
using System.Text.Json.Nodes;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Catalogos.Models;
using Planta.Application.GuiaRemision.Abstractions;
using Planta.Application.GuiaRemisionManual.Abstractions;
using Planta.Application.Maestros.Abstractions;

namespace Planta.Application.GuiaRemisionManual;

public sealed class GuiasRemisionManualUseCase(
    IGuiasRemisionManualService guiasRemisionManualService,
    ICatalogosService catalogosService,
    IMaestrosService maestrosService,
    IDocumentosElectronicosService documentosElectronicosService) : IGuiasRemisionManualUseCase
{
    public async Task<List<JsonElement>> SincronizarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json)
    {
        return await guiasRemisionManualService.SincronizarGuiasRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);
    }

    public async Task<List<JsonElement>> ListarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string? estado, string? fechaDesde, string? fechaHasta, string? texto)
    {
        return await guiasRemisionManualService.ListarGuiasRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);
    }

    public async Task<List<JsonElement>> ListarGuiasRemisionManualExcelAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string? estado, string? fechaDesde, string? fechaHasta, string? texto, string? codigoCultivo)
    {
        var result = await guiasRemisionManualService.ListarGuiasRemisionManualExcelAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);

        if (result.Count == 0)
            return result;

        var wrapper = result[0];
        if (wrapper.GetProperty("error").GetBoolean())
            return result;

        if (!wrapper.TryGetProperty("data", out var dataElement)
            || dataElement.ValueKind != JsonValueKind.Array
            || dataElement.GetArrayLength() == 0)
            return result;

        try
        {
            var campanias = await maestrosService.GetCampaniasAsync(ruc) ?? new List<CampaniaExterna>();
            var clientes = await maestrosService.GetClientesAsync(idempresa) ?? new List<ClienteExterno>();
            var acopios = await maestrosService.GetAcopiosAsync(idempresa) ?? new List<AcopiosExterno>();
            var enrichedArray = new JsonArray();

            foreach (var item in dataElement.EnumerateArray())
            {
                var node = JsonNode.Parse(item.GetRawText());
                if (node is not JsonObject row)
                    continue;

                var codigoAcopioFila = row["codigoAcopio_codigo"]?.GetValue<string>();
                var acopio = acopios.FirstOrDefault(a =>
                    !string.IsNullOrWhiteSpace(a.codigo_acopio)
                    && a.codigo_acopio.Trim().Equals(codigoAcopioFila ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(a.Ruc)
                    && a.Ruc.Trim().Equals(ruc, StringComparison.OrdinalIgnoreCase));
                row["PLANTA DE EMPAQUE"] = acopio?.Acopio ?? codigoAcopioFila ?? string.Empty;

                var idProyectoCodigo = row["idProyecto_codigo"]?.GetValue<string>();
                var campania = campanias.FirstOrDefault(c =>
                    !string.IsNullOrWhiteSpace(c.IdProyecto)
                    && c.IdProyecto.Trim().Equals(idProyectoCodigo ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                    && (string.IsNullOrWhiteSpace(codigoCultivo)
                        || (!string.IsNullOrWhiteSpace(c.CodCultivo)
                            && c.CodCultivo.Trim().Equals(codigoCultivo, StringComparison.OrdinalIgnoreCase))));
                row["CAMPAÑA"] = campania is not null
                    ? $"{campania.FechaInicio:yyyy-MM-dd} - {campania.FechaFin:yyyy-MM-dd}"
                    : idProyectoCodigo ?? string.Empty;

                var documentoDestinatario = row["documentoDestinatario_codigo"]?.GetValue<string>();
                var cliente = clientes.FirstOrDefault(c =>
                    (!string.IsNullOrWhiteSpace(c.DocumentoFiscal)
                        && c.DocumentoFiscal.Trim().Equals(documentoDestinatario ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrWhiteSpace(c.Documento)
                        && c.Documento.Trim().Equals(documentoDestinatario ?? string.Empty, StringComparison.OrdinalIgnoreCase)));
                row["DESTINATARIO"] = cliente?.Nombre ?? documentoDestinatario ?? string.Empty;

                row.Remove("idProyecto_codigo");
                row.Remove("codigoAcopio_codigo");
                row.Remove("documentoDestinatario_codigo");

                enrichedArray.Add(row);
            }

            var response = new { error = false, data = enrichedArray, mensaje = "" };
            return new List<JsonElement>
            {
                JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(response))
            };
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                error = true,
                data = JsonSerializer.Deserialize<JsonElement>("null"),
                mensaje = $"Error enriqueciendo Excel de guías manuales: {ex.Message}"
            };
            return new List<JsonElement>
            {
                JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(errorResponse))
            };
        }
    }

    public async Task<List<JsonElement>> GetGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string idRol, string codigoGuiaRemision)
    {
        return await guiasRemisionManualService.GetGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, idRol, codigoGuiaRemision);
    }

    public async Task<List<JsonElement>> EditarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json)
    {
        return await guiasRemisionManualService.EditarGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);
    }

    public async Task<List<JsonElement>> EliminarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario, string idRol)
    {
        return await guiasRemisionManualService.EliminarGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, idRol);
    }

    public async Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string? codigoEstadoSunat, string? estadoSunat, string? pdfFileUrl, string? xmlFileSignUrl, string? xmlFileSunatUrl, bool? enviadoBizlinks, string? respuestaBizlinks, string usuario)
    {
        return await guiasRemisionManualService.ActualizarEstadoSunatGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, codigoEstadoSunat, estadoSunat, pdfFileUrl, xmlFileSignUrl, xmlFileSunatUrl, enviadoBizlinks, respuestaBizlinks, usuario);
    }

    public async Task<List<JsonElement>> AnularGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
    {
        return await guiasRemisionManualService.AnularGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
    }

    public async Task<List<JsonElement>> EmitirGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
    {
        var emitirResult = await guiasRemisionManualService.EmitirGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
        Console.WriteLine($"[EmitirGuiaRemisionManual] emitirResult: {JsonSerializer.Serialize(emitirResult)}");

        if (emitirResult.Count == 0)
        {
            return emitirResult;
        }

        var wrapper = emitirResult[0];
        var error = wrapper.GetProperty("error").GetBoolean();
        if (error)
        {
            return emitirResult;
        }

        string? codigoEmitido = null;
        if (wrapper.TryGetProperty("data", out var dataProp))
        {
            if (dataProp.ValueKind == JsonValueKind.Array && dataProp.GetArrayLength() > 0)
            {
                var first = dataProp[0];
                if (first.TryGetProperty("codigoGuiaRemision", out var codigoProp))
                {
                    codigoEmitido = codigoProp.GetString();
                }
            }
        }

        if (string.IsNullOrWhiteSpace(codigoEmitido))
        {
            codigoEmitido = codigoGuiaRemision;
        }

        return await ProcesarEnvioAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoEmitido, usuario);
    }

    public async Task<List<JsonElement>> ReenviarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
    {

        var detalleResult = await guiasRemisionManualService.GetGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, string.Empty, codigoGuiaRemision);


        if (detalleResult.Count == 0)
        {
            return detalleResult;
        }

        var detalleWrapper = detalleResult[0];
        var detalleError = detalleWrapper.GetProperty("error").GetBoolean();
        if (detalleError)
        {
            return detalleResult;
        }

        if (!detalleWrapper.TryGetProperty("data", out var detalleData) || detalleData.ValueKind == JsonValueKind.Null)
        {
            return detalleResult;
        }

        JsonElement guiaElement;
        if (detalleData.ValueKind == JsonValueKind.Array && detalleData.GetArrayLength() > 0)
        {
            guiaElement = detalleData[0];
        }
        else if (detalleData.ValueKind == JsonValueKind.Object)
        {
            guiaElement = detalleData;
        }
        else
        {
            return detalleResult;
        }

        var guiaNode = JsonNode.Parse(guiaElement.GetRawText());
        if (guiaNode is not JsonObject guiaObj)
        {
            return detalleResult;
        }

        var estado = guiaObj["estado"]?.GetValue<string>();
        var enviadoBizlinksValue = guiaObj["enviadoBizlinks"];
        bool enviadoBizlinks = enviadoBizlinksValue?.GetValueKind() switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => enviadoBizlinksValue.GetValue<int>() == 1,
            _ => false
        };

        if (estado != "CERRADA")
        {
            var payload = new { error = true, data = (object?)null, mensaje = "La guía debe estar CERRADA para poder reenviarla." };
            var json = JsonSerializer.Serialize(payload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(json) };
        }

        if (enviadoBizlinks)
        {
            var payload = new { error = true, data = (object?)null, mensaje = "La guía ya fue enviada a SUNAT." };
            var json = JsonSerializer.Serialize(payload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(json) };
        }

        return await ProcesarEnvioAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, guiaObj);
    }

    public async Task<List<JsonElement>> ConsultarEstadoSunatGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
    {
        var detalleResult = await guiasRemisionManualService.GetGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, string.Empty, codigoGuiaRemision);
        Console.WriteLine($"[ConsultarEstadoSunat] detalleResult: {JsonSerializer.Serialize(detalleResult)}");

        if (detalleResult.Count == 0)
        {
            return detalleResult;
        }

        var detalleWrapper = detalleResult[0];
        var detalleError = detalleWrapper.GetProperty("error").GetBoolean();
        if (detalleError)
        {
            return detalleResult;
        }

        if (!detalleWrapper.TryGetProperty("data", out var detalleData) || detalleData.ValueKind == JsonValueKind.Null)
        {
            return detalleResult;
        }

        JsonElement guiaElement;
        if (detalleData.ValueKind == JsonValueKind.Array && detalleData.GetArrayLength() > 0)
        {
            guiaElement = detalleData[0];
        }
        else if (detalleData.ValueKind == JsonValueKind.Object)
        {
            guiaElement = detalleData;
        }
        else
        {
            return detalleResult;
        }

        var guiaObj = JsonNode.Parse(guiaElement.GetRawText())?.AsObject();
        if (guiaObj is null)
        {
            return detalleResult;
        }

        var serie = guiaObj["serie"]?.GetValue<string>() ?? string.Empty;
        var numero = guiaObj["numero"]?.GetValue<string>() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(serie) || string.IsNullOrWhiteSpace(numero))
        {
            var payload = new { error = true, data = (object?)null, mensaje = "La guía no tiene serie o número asignado." };
            var json = JsonSerializer.Serialize(payload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(json) };
        }

        var apiResponse = await documentosElectronicosService.ConsultarEstadoGuiaRemisionAsync(idempresa, serie, numero);
        var respuestaJson = JsonSerializer.Serialize(apiResponse);
        Console.WriteLine($"[ConsultarEstadoSunat] apiResponse: {respuestaJson}");

        string codigoEstadoSunat;
        if (apiResponse.Success && apiResponse.Data?.Exito == true)
        {
            codigoEstadoSunat = apiResponse.Data.EstadoSunat ?? "PE_02";
        }
        else if (apiResponse.Data != null)
        {
            codigoEstadoSunat = apiResponse.Data.EstadoSunat ?? "ERROR";
        }
        else
        {
            codigoEstadoSunat = "ERROR";
        }

        var esExito = apiResponse.Success && apiResponse.Data?.Exito == true;
        var esRc05 = codigoEstadoSunat == "RC_05";
        bool? enviadoBizlinks = esExito ? true : (esRc05 ? true : false);

        await guiasRemisionManualService.ActualizarEstadoSunatGuiaRemisionManualAsync(
            idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision,
            codigoEstadoSunat, string.Empty,
            apiResponse.Data?.PdfFileUrl,
            apiResponse.Data?.XmlFileSignUrl,
            apiResponse.Data?.XmlFileSunatUrl,
            enviadoBizlinks,
            respuestaJson,
            usuario);

        if (esRc05)
        {
            var anulacionResult = await guiasRemisionManualService.AnularGuiaRemisionManualAsync(
                idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
            if (anulacionResult.Count > 0 && anulacionResult[0].GetProperty("error").GetBoolean())
            {
                var mensajeAnulacion = anulacionResult[0].TryGetProperty("mensaje", out var m) ? m.GetString() : null;
                Console.WriteLine($"[ConsultarEstadoSunat] anulación automática no aplicada: {mensajeAnulacion}");
            }
        }

        var mensaje = apiResponse.Data?.MensajeSunat ?? apiResponse.Data?.Mensaje ?? apiResponse.Message;
        var tieneError = !apiResponse.Success || apiResponse.Data?.Exito != true;
        var data = JsonSerializer.SerializeToElement(apiResponse);

        var responsePayload = new { error = tieneError, mensaje, data };
        var responseJson = JsonSerializer.Serialize(responsePayload);
        return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(responseJson) };
    }

    private async Task<List<JsonElement>> ProcesarEnvioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario, JsonObject? guiaObjPrevia = null)
    {
        JsonObject? guiaObj;
        try
        {
            if (guiaObjPrevia is not null)
            {
                guiaObj = guiaObjPrevia;
            }
            else
            {
                var detalleResult = await guiasRemisionManualService.GetGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, string.Empty, codigoGuiaRemision);
                Console.WriteLine($"[ProcesarEnvio] detalleResult: {JsonSerializer.Serialize(detalleResult)}");

                if (detalleResult.Count == 0)
                {
                    return detalleResult;
                }

                var detalleWrapper = detalleResult[0];
                var detalleError = detalleWrapper.GetProperty("error").GetBoolean();
                if (detalleError)
                {
                    return detalleResult;
                }

                if (!detalleWrapper.TryGetProperty("data", out var detalleData) || detalleData.ValueKind == JsonValueKind.Null)
                {
                    return detalleResult;
                }

                JsonElement guiaElement;
                if (detalleData.ValueKind == JsonValueKind.Array && detalleData.GetArrayLength() > 0)
                {
                    guiaElement = detalleData[0];
                }
                else if (detalleData.ValueKind == JsonValueKind.Object)
                {
                    guiaElement = detalleData;
                }
                else
                {
                    return detalleResult;
                }

                var guiaNode = JsonNode.Parse(guiaElement.GetRawText());
                if (guiaNode is null)
                {
                    return detalleResult;
                }

                guiaObj = guiaNode.AsObject();
            }

            var documentoDestinatario = guiaObj["documentoDestinatario"]?.GetValue<string>();

            if (!string.IsNullOrWhiteSpace(documentoDestinatario))
            {
                var destinatarios = (await GetDestinatariosFromMaestrosAsync(idempresa, ruc))?.Data ?? new List<Destinatarios>();
                var d = destinatarios.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(x.DocumentoFiscal) && x.DocumentoFiscal.Trim() == documentoDestinatario.Trim());
                if (d is not null)
                {
                    guiaObj["nombreDestinatario"] = d.Nombre;
                }
            }

            Console.WriteLine($"[ProcesarEnvio] guia enriquecida: {guiaObj.ToJsonString()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ProcesarEnvio] error enriqueciendo cabecera: {ex.Message}");
            var errorPayload = new { error = true, data = (object?)null, mensaje = ex.Message };
            var errorJson = JsonSerializer.Serialize(errorPayload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(errorJson) };
        }

        var guiaPascalCase = ConvertirANodoPascalCase(guiaObj);
        var guiaPascalJson = JsonSerializer.Serialize(guiaPascalCase);
        Console.WriteLine($"[ProcesarEnvio] json pascalcase: {guiaPascalJson}");

        var guiaElementApi = JsonSerializer.Deserialize<JsonElement>(guiaPascalJson);
        var apiResponse = await documentosElectronicosService.EnviarGuiaRemisionAsync(guiaElementApi, CancellationToken.None);

        var respuestaJson = JsonSerializer.Serialize(apiResponse);
        Console.WriteLine($"[ProcesarEnvio] apiResponse: {respuestaJson}");

        string codigoEstadoSunat;
        if (apiResponse.Success && apiResponse.Data?.Exito == true)
        {
            codigoEstadoSunat = apiResponse.Data.EstadoSunat ?? "PE_02";
        }
        else if (apiResponse.Data != null)
        {
            codigoEstadoSunat = apiResponse.Data.EstadoSunat ?? "ERROR";
        }
        else
        {
            codigoEstadoSunat = "ERROR";
        }

        var esExito = apiResponse.Success && apiResponse.Data?.Exito == true;
        var esRc05 = codigoEstadoSunat == "RC_05";

        if (esExito)
        {
            await guiasRemisionManualService.ActualizarEstadoSunatGuiaRemisionManualAsync(
                idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision,
                codigoEstadoSunat, string.Empty,
                apiResponse.Data?.PdfFileUrl,
                apiResponse.Data?.XmlFileSignUrl,
                apiResponse.Data?.XmlFileSunatUrl,
                true,
                respuestaJson,
                usuario);
        }
        else if (esRc05)
        {
            await guiasRemisionManualService.ActualizarEstadoSunatGuiaRemisionManualAsync(
                idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision,
                codigoEstadoSunat, string.Empty,
                apiResponse.Data?.PdfFileUrl,
                apiResponse.Data?.XmlFileSignUrl,
                apiResponse.Data?.XmlFileSunatUrl,
                true,
                respuestaJson,
                usuario);

            var anulacionResult = await guiasRemisionManualService.AnularGuiaRemisionManualAsync(
                idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
            if (anulacionResult.Count > 0 && anulacionResult[0].GetProperty("error").GetBoolean())
            {
                var mensajeAnulacion = anulacionResult[0].TryGetProperty("mensaje", out var m) ? m.GetString() : null;
                Console.WriteLine($"[ProcesarEnvio] anulación automática no aplicada: {mensajeAnulacion}");
            }
        }
        else
        {
            await guiasRemisionManualService.ActualizarEstadoSunatGuiaRemisionManualAsync(
                idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision,
                codigoEstadoSunat, string.Empty,
                apiResponse.Data?.PdfFileUrl,
                apiResponse.Data?.XmlFileSignUrl,
                apiResponse.Data?.XmlFileSunatUrl,
                false,
                respuestaJson,
                usuario);
        }

        var mensaje = apiResponse.Data?.MensajeSunat ?? apiResponse.Data?.Mensaje ?? apiResponse.Message;
        var tieneError = !apiResponse.Success || apiResponse.Data?.Exito != true;
        var data = JsonSerializer.SerializeToElement(apiResponse);

        var responsePayload = new { error = tieneError, mensaje, data };
        var responseJson = JsonSerializer.Serialize(responsePayload);
        return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(responseJson) };
    }

    private async Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosFromMaestrosAsync(string idempresa, string ruc)
    {
        var clientes = await maestrosService.GetClientesAsync(idempresa);
        var json = JsonSerializer.Serialize(clientes);
        return await catalogosService.GetDestinatariosAsync(idempresa, ruc, json);
    }

    private static readonly Dictionary<string, string> GuiaPascalCaseMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["idempresa"] = "IdEmpresa",
        ["numeroDocumentoRemitente"] = "NumeroDocumentoRemitente",
        ["razonSocialRemitente"] = "RazonSocialRemitente",
        ["idProyecto"] = "IdProyecto",
        ["codigoAcopio"] = "CodigoAcopio",
        ["codigoGuiaRemision"] = "CodigoGuiaRemision",
        ["serie"] = "Serie",
        ["numero"] = "Numero",
        ["correoEmisor"] = "CorreoEmisor",
        ["correoAdquiriente"] = "CorreoAdquiriente",
        ["documentoDestinatario"] = "DocumentoDestinatario",
        ["nombreDestinatario"] = "NombreDestinatario",
        ["tipoDocumentoRemitente"] = "TipoDocumentoRemitente",
        ["tipoDocumentoDestinatario"] = "TipoDocumentoDestinatario",
        ["puntoPartida"] = "PuntoPartida",
        ["puntoLlegada"] = "PuntoLlegada",
        ["ubigeoPartida"] = "UbigeoPartida",
        ["ubigeoLlegada"] = "UbigeoLlegada",
        ["idEstablecimientoPartida"] = "IdEstablecimientoPartida",
        ["idEstablecimientoLlegada"] = "IdEstablecimientoLlegada",
        ["descripcionEstablecimientoPartida"] = "DescripcionEstablecimientoPartida",
        ["codigoEstablecimientoSunatPartida"] = "CodigoEstablecimientoSunatPartida",
        ["idTipoEstablecimientoPartida"] = "IdTipoEstablecimientoPartida",
        ["descripcionEstablecimientoLlegada"] = "DescripcionEstablecimientoLlegada",
        ["codigoEstablecimientoSunatLlegada"] = "CodigoEstablecimientoSunatLlegada",
        ["idTipoEstablecimientoLlegada"] = "IdTipoEstablecimientoLlegada",
        ["fechaEmision"] = "FechaEmision",
        ["idTransportista"] = "IdTransportista",
        ["razonSocialTransportista"] = "RazonSocialTransportista",
        ["RucTransportista"] = "RucTransportista",
        ["tipoDocumentoTransportista"] = "TipoDocumentoTransportista",
        ["idConductor"] = "IdConductor",
        ["NombreConductor"] = "NombreConductor",
        ["ApellidoConductor"] = "ApellidoConductor",
        ["nombreCompletoConductor"] = "NombreCompletoConductor",
        ["DniConductor"] = "DniConductor",
        ["numeroLicencia"] = "NumeroLicencia",
        ["tipoDocumentoConductor"] = "TipoDocumentoConductor",
        ["idVehiculo"] = "IdVehiculo",
        ["placaPrincipalVehiculo"] = "PlacaPrincipalVehiculo",
        ["placaRemolque"] = "PlacaRemolque",
        ["certificadoInscripcion"] = "CertificadoInscripcion",
        ["marcaVehiculo"] = "MarcaVehiculo",
        ["codigoSunatMotivoTraslado"] = "CodigoSunatMotivoTraslado",
        ["descripcionMotivoTrasladoCatalogo"] = "DescripcionMotivoTrasladoCatalogo",
        ["descripcionMotivoTraslado"] = "DescripcionMotivoTraslado",
        ["precinto"] = "Precinto",
        ["inicioTraslado"] = "InicioTraslado",
        ["observaciones"] = "Observaciones",
        ["observacionGuia"] = "ObservacionGuia",
        ["estado"] = "Estado",
        ["estadoSunat"] = "EstadoSunat",
        ["codigoEstadoSunat"] = "CodigoEstadoSunat",
        ["pesoTotal"] = "PesoTotal",
        ["unidadMedidaPesoBruto"] = "UnidadMedidaPesoBruto",
        ["cantidad"] = "Cantidad",
        ["usuarioEmision"] = "UsuarioEmision",
        ["fechaCreacionWeb"] = "FechaCreacionWeb",
        ["fechaCierre"] = "FechaCierre",
        ["parihuelas"] = "Parihuelas",
        ["observacionesUsuario"] = "ObservacionesUsuario",
        ["esReposicion"] = "EsReposicion",
        ["inspeccionTemperatura"] = "InspeccionTemperatura",
        ["inspeccionLibreOlores"] = "InspeccionLibreOlores",
        ["inspeccionLibreInsectos"] = "InspeccionLibreInsectos",
        ["inspeccionLibreMateriasExtranas"] = "InspeccionLibreMateriasExtranas",
        ["inspeccionUnidadLimpia"] = "InspeccionUnidadLimpia",
        ["inspeccionObservaciones"] = "InspeccionObservaciones",
        ["inspeccionMedidaCorrectiva"] = "InspeccionMedidaCorrectiva",
        ["numeroViaje"] = "NumeroViaje",
        ["fechaEntregaBienes"] = "FechaEntregaBienes",
        ["esEnsayo"] = "EsEnsayo",
        ["transactionId_uuid"] = "TransactionIdUuid",
        ["fechaCreacion"] = "FechaCreacion",
        ["usuarioCre"] = "UsuarioCre",
        ["usuarioMod"] = "UsuarioMod",
        ["fechaModificacion"] = "FechaModificacion",
        ["modalidadTraslado"] = "ModalidadTraslado",
        ["modalidadTrasladoDescripcion"] = "ModalidadTrasladoDescripcion",
        ["detalle"] = "Detalle",
        ["codigoItem"] = "CodigoItem",
        ["codigoUnidadMedida"] = "CodigoUnidadMedida",
        ["unidadMedida"] = "UnidadMedida",
        ["descripcion"] = "Descripcion",
        ["pesoEstimado"] = "PesoEstimado"
    };

    private static JsonNode? ConvertirANodoPascalCase(JsonNode? node)
    {
        if (node is JsonObject obj)
        {
            var result = new JsonObject();
            foreach (var prop in obj)
            {
                var key = GuiaPascalCaseMap.TryGetValue(prop.Key, out var mapped) ? mapped : prop.Key;
                result[key] = ConvertirANodoPascalCase(prop.Value);
            }
            return result;
        }

        if (node is JsonArray arr)
        {
            var result = new JsonArray();
            foreach (var item in arr)
            {
                result.Add(ConvertirANodoPascalCase(item));
            }
            return result;
        }

        return node?.DeepClone();
    }
}
