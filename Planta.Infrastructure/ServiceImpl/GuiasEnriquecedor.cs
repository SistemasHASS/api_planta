using System.Text.Json;
using System.Text.Json.Nodes;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Completa con nombres (que viven en apimaestra) lo que devuelven los SP de guias: el detalle de una guia
/// y las filas del Excel. Los catalogos se piden uno por uno: la base usa una sola conexion a la vez.
/// </summary>
internal sealed class GuiasEnriquecedor(IMaestrosService maestros, ICatalogosLocalService catalogos)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    // ───────── Detalle de una guia ─────────

    /// <summary>
    /// Agrega nombre del destinatario y, en cada linea del detalle, consignatario, destino, variedad, transporte y la descripcion
    /// para el documento electronico. Devuelve la lista de datos de catalogo que no se encontraron.
    /// </summary>
    public async Task<List<string>> EnriquecerGuiaAsync(ContextoUsuario contexto, string idProyecto, JsonObject guia, CancellationToken ct)
    {
        var documentoDestinatario = Texto(guia, "documentoDestinatario");
        var motivoTraslado = Texto(guia, "codigoSunatMotivoTraslado") ?? Texto(guia, "motivoTraslado");
        var esTrasladoEntreEstablecimientos = string.Equals(motivoTraslado?.Trim(), "04", StringComparison.OrdinalIgnoreCase);
        var faltantes = new List<string>();

        var destinatarios = await FilasAsync(await catalogos.ListarDestinatariosAsync(contexto, ct));
        var consignatarios = await FilasAsync(await catalogos.ListarConsignatariosAsync(contexto, ct));
        var paises = await LeerAsync<PaisExterno>(MaestroExterno.Paises, contexto, ct);
        var variedades = await FilasAsync(await catalogos.ListarVariedadesAsync(contexto, ct));
        var transportes = await LeerAsync<TransporteExterno>(MaestroExterno.Transporte, contexto, ct);
        var campanias = await LeerAsync<CampaniaExterna>(MaestroExterno.Campanias, contexto, ct);

        // Destinatario de la cabecera
        if (!string.IsNullOrWhiteSpace(documentoDestinatario))
        {
            if (esTrasladoEntreEstablecimientos)
            {
                guia["nombreDestinatario"] = Texto(guia, "razonSocialRemitente") ?? documentoDestinatario;
            }
            else
            {
                var destinatario = destinatarios.FirstOrDefault(d => Igual(Texto(d, "documentoFiscal"), documentoDestinatario));
                if (destinatario is null)
                {
                    faltantes.Add($"Destinatario con documento {documentoDestinatario}");
                }
                else
                {
                    guia["nombreDestinatario"] = Texto(destinatario, "nombre");
                }
            }
        }

        var campania = campanias.FirstOrDefault(c => Igual(c.IdProyecto, idProyecto));
        var fruta = campania?.Fruta?.Trim() ?? "";

        // El SP devuelve palets y detalle como texto JSON (FOR JSON anidado): se convierten en arreglos.
        ConvertirTextoEnJson(guia, "palets");
        var detalle = ConvertirTextoEnJson(guia, "detalle");

        if (detalle is JsonArray lineas)
        {
            foreach (var linea in lineas.OfType<JsonObject>())
            {
                var documentoConsignatario = Texto(linea, "documentoConsignatario");
                var idDestino = Texto(linea, "idDestino");
                var codigoVariedad = Texto(linea, "codigoVariedad");
                var idTransporte = Texto(linea, "idTransporte");

                string? nombreConsignatario = null;
                string? nombreDestino = null;
                string? nombreVariedad = null;
                string? nombreTransporte = null;

                if (!string.IsNullOrWhiteSpace(documentoConsignatario))
                {
                    var consignatario = consignatarios.FirstOrDefault(c =>
                        Igual(Texto(c, "documento") ?? Texto(c, "documentoFiscal"), documentoConsignatario));
                    if (consignatario is null) faltantes.Add($"Consignatario con documento {documentoConsignatario}");
                    else nombreConsignatario = Texto(consignatario, "nombre");
                }

                if (!string.IsNullOrWhiteSpace(idDestino))
                {
                    var pais = paises.FirstOrDefault(p => Igual(p.Id, idDestino));
                    if (pais is null) faltantes.Add($"Destino con id {idDestino}");
                    else nombreDestino = pais.Pais ?? pais.Nacionalidad;
                }

                if (!string.IsNullOrWhiteSpace(codigoVariedad))
                {
                    var variedad = variedades.FirstOrDefault(v => Igual(Texto(v, "idvariedad"), codigoVariedad));
                    if (variedad is null) faltantes.Add($"Variedad con codigo {codigoVariedad}");
                    else nombreVariedad = Texto(variedad, "variedad");
                }

                if (!string.IsNullOrWhiteSpace(idTransporte))
                {
                    var transporte = transportes.FirstOrDefault(t => Igual(t.Id, idTransporte));
                    if (transporte is null) faltantes.Add($"Transporte con id {idTransporte}");
                    else nombreTransporte = transporte.Transporte;
                }

                linea["nombreConsignatario"] = nombreConsignatario;
                linea["nombreDestino"] = nombreDestino;
                linea["nombreVariedad"] = nombreVariedad;
                linea["nombreTransporte"] = nombreTransporte;
                linea["detalleDescripcion"] = DescripcionDeLinea(linea, fruta, nombreConsignatario, nombreDestino, nombreVariedad, nombreTransporte);
            }
        }

        return faltantes;
    }

    // ───────── Reporte de códigos caja ─────────

    /// <summary>
    /// Completa cada combinación de PLANTA_CodigosCaja (SP PLANTA_ListarCodigosCajaDetalle) con lo que vive en apimaestra y en
    /// los catálogos: consignatario, destino, variedad, transporte y la fruta del cultivo. Arma la descripción completa igual que
    /// una línea del detalle de la guía. Si un nombre no se encuentra, la descripción usa el código.
    /// </summary>
    public async Task<JsonArray> EnriquecerCodigosCajaAsync(ContextoUsuario contexto, JsonElement filas, CancellationToken ct)
    {
        var resultado = new JsonArray();
        if (filas.ValueKind != JsonValueKind.Array || filas.GetArrayLength() == 0)
        {
            return resultado;
        }

        var consignatarios = await FilasAsync(await catalogos.ListarConsignatariosAsync(contexto, ct));
        var paises = await LeerAsync<PaisExterno>(MaestroExterno.Paises, contexto, ct);
        var variedades = await FilasAsync(await catalogos.ListarVariedadesAsync(contexto, ct));
        var transportes = await LeerAsync<TransporteExterno>(MaestroExterno.Transporte, contexto, ct);
        var campanias = await LeerAsync<CampaniaExterna>(MaestroExterno.Campanias, contexto, ct);

        foreach (var elemento in filas.EnumerateArray())
        {
            if (JsonNode.Parse(elemento.GetRawText()) is not JsonObject fila) continue;

            var codigoCultivo = Texto(fila, "codigoCultivo");
            var documentoConsignatario = Texto(fila, "documentoConsignatario");
            var idDestino = Texto(fila, "idDestino");
            var codigoVariedad = Texto(fila, "codigoVariedad");
            var idTransporte = Texto(fila, "idTransporte");

            var fruta = Nulo(campanias.FirstOrDefault(c => Igual(c.CodCultivo, codigoCultivo))?.Fruta);

            var consignatario = consignatarios.FirstOrDefault(c => Igual(Texto(c, "documento") ?? Texto(c, "documentoFiscal"), documentoConsignatario));
            var nombreConsignatario = consignatario is null ? null : Nulo(Texto(consignatario, "nombre"));

            var pais = paises.FirstOrDefault(p => Igual(p.Id, idDestino));
            var nombreDestino = pais is null ? null : Nulo(pais.Pais) ?? Nulo(pais.Nacionalidad);

            // Primero la variedad del mismo cultivo; si no hay, la que tenga ese código.
            var variedad = variedades.FirstOrDefault(v => Igual(Texto(v, "idvariedad"), codigoVariedad) && Igual(Texto(v, "idcultivo"), codigoCultivo))
                ?? variedades.FirstOrDefault(v => Igual(Texto(v, "idvariedad"), codigoVariedad));
            var nombreVariedad = variedad is null ? null : Nulo(Texto(variedad, "variedad"));

            var nombreTransporte = Nulo(transportes.FirstOrDefault(t => Igual(t.Id, idTransporte))?.Transporte);

            fila["fruta"] = fruta;
            fila["nombreConsignatario"] = nombreConsignatario;
            fila["nombreDestino"] = nombreDestino;
            fila["nombreVariedad"] = nombreVariedad;
            fila["nombreTransporte"] = nombreTransporte;
            fila["descripcion"] = DescripcionDeLinea(fila, fruta ?? "",
                nombreConsignatario ?? documentoConsignatario,
                nombreDestino ?? idDestino,
                nombreVariedad ?? codigoVariedad,
                nombreTransporte ?? idTransporte);
            resultado.Add(fila);
        }

        return resultado;
    }

    private static string? Nulo(string? texto) => string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();

    /// <summary>Texto que se imprime en la guia por cada linea: bloques separados por " - ".</summary>
    private static string DescripcionDeLinea(JsonObject linea, string fruta, string? consignatario, string? destino, string? variedad, string? transporte)
    {
        var pesoPorCaja = Texto(linea, "pesoPorCaja");
        var bloques = new[]
        {
            Unir(Texto(linea, "nombreTipoProcesoEmpacado")),
            Unir(consignatario, destino, Texto(linea, "nombrePresentacion"), Texto(linea, "nombreFormato")),
            Unir(fruta, "var.", variedad, string.IsNullOrWhiteSpace(pesoPorCaja) ? null : $"{pesoPorCaja} kg"),
            Unir(Texto(linea, "nombreTipoEmpaqueGuia"), "/", Texto(linea, "codigoRancho")),
            Unir("LDP", Texto(linea, "codigoLugarProduccion")),
            string.IsNullOrWhiteSpace(transporte) ? "" : $"({transporte})",
        };
        return string.Join(" - ", bloques.Where(b => !string.IsNullOrWhiteSpace(b)));
    }

    /// <summary>Nombre del destinatario de una guia manual (se busca por su documento fiscal). Null si no se encuentra.</summary>
    public async Task<string?> NombreDestinatarioAsync(ContextoUsuario contexto, string documentoDestinatario, CancellationToken ct)
    {
        var destinatarios = await FilasAsync(await catalogos.ListarDestinatariosAsync(contexto, ct));
        var destinatario = destinatarios.FirstOrDefault(d => Igual(Texto(d, "documentoFiscal"), documentoDestinatario));
        return destinatario is null ? null : Texto(destinatario, "nombre");
    }

    // ───────── Filas del Excel ─────────

    /// <summary>
    /// Cambia las columnas "_codigo" que trae el SP por el nombre legible (planta, campaña, variedad, destino, destinatario,
    /// consignatario, transporte, calibre) y las quita. Variedad y calibre solo se traducen si se indica el cultivo.
    /// </summary>
    public async Task<JsonArray> EnriquecerFilasExcelAsync(ContextoUsuario contexto, JsonElement filas, string? codigoCultivo, CancellationToken ct)
    {
        var hayCultivo = !string.IsNullOrWhiteSpace(codigoCultivo);

        var campanias = await LeerAsync<CampaniaExterna>(MaestroExterno.Campanias, contexto, ct);
        var paises = await LeerAsync<PaisExterno>(MaestroExterno.Paises, contexto, ct);
        var transportes = await LeerAsync<TransporteExterno>(MaestroExterno.Transporte, contexto, ct);
        var clientes = await LeerAsync<ClienteExterno>(MaestroExterno.Clientes, contexto, ct);
        var acopios = await LeerAsync<AcopiosExterno>(MaestroExterno.Acopios, contexto, ct);
        var variedades = hayCultivo ? await FilasAsync(await catalogos.ListarVariedadesAsync(contexto, ct)) : [];
        var calibres = hayCultivo ? await LeerAsync<CalibreExterno>(MaestroExterno.Calibres, contexto, ct) : [];

        var resultado = new JsonArray();
        foreach (var elemento in filas.EnumerateArray())
        {
            if (JsonNode.Parse(elemento.GetRawText()) is not JsonObject fila) continue;

            if (fila.ContainsKey("codigoAcopio_codigo"))
            {
                var codigo = Texto(fila, "codigoAcopio_codigo");
                var acopio = acopios.FirstOrDefault(a => Igual(a.codigo_acopio, codigo) && Igual(a.Ruc, contexto.Ruc));
                fila["PLANTA DE EMPAQUE"] = acopio?.Acopio ?? codigo;
            }

            if (fila.ContainsKey("idProyecto_codigo"))
            {
                var idProyecto = Texto(fila, "idProyecto_codigo");
                var campania = campanias.FirstOrDefault(c => Igual(c.IdProyecto, idProyecto) && (!hayCultivo || Igual(c.CodCultivo, codigoCultivo)));
                fila["CAMPAÑA"] = campania is null ? idProyecto : $"{campania.FechaInicio:yyyy-MM-dd} - {campania.FechaFin:yyyy-MM-dd}";
            }

            if (hayCultivo && fila.ContainsKey("variedad_codigo"))
            {
                var codigo = Texto(fila, "variedad_codigo");
                var variedad = variedades.FirstOrDefault(v => Igual(Texto(v, "idvariedad"), codigo) && Igual(Texto(v, "idcultivo"), codigoCultivo));
                fila["VARIEDAD"] = (variedad is null ? null : Texto(variedad, "variedad")) ?? codigo;
            }

            if (fila.ContainsKey("destinoId_codigo"))
            {
                var codigo = Texto(fila, "destinoId_codigo");
                var pais = paises.FirstOrDefault(p => Igual(p.Id, codigo));
                fila["DESTINO"] = pais?.Pais ?? pais?.Nacionalidad ?? codigo;
            }

            if (fila.ContainsKey("documentoDestinatario_codigo"))
            {
                var documento = Texto(fila, "documentoDestinatario_codigo");
                fila["DESTINATARIO"] = NombreDeCliente(clientes, documento) ?? documento;
            }

            if (fila.ContainsKey("documentoConsignatario_codigo"))
            {
                var documento = Texto(fila, "documentoConsignatario_codigo");
                fila["CONSIGNATARIO"] = NombreDeCliente(clientes, documento) ?? documento;
            }

            if (fila.ContainsKey("transporte_codigo"))
            {
                var codigo = Texto(fila, "transporte_codigo");
                fila["TRANSPORTE"] = transportes.FirstOrDefault(t => Igual(t.Id, codigo))?.Transporte ?? codigo;
            }

            if (hayCultivo && fila.ContainsKey("calibre_codigo"))
            {
                var codigo = Texto(fila, "calibre_codigo");
                fila["CALIBRE"] = calibres.FirstOrDefault(c => Igual(c.Id, codigo) && Igual(c.IdCultivo, codigoCultivo))?.Calibre ?? codigo;
            }

            foreach (var columna in new[] { "idProyecto_codigo", "codigoAcopio_codigo", "variedad_codigo", "destinoId_codigo", "documentoDestinatario_codigo", "documentoConsignatario_codigo", "transporte_codigo", "calibre_codigo" })
            {
                fila.Remove(columna);
            }

            resultado.Add(fila);
        }

        return resultado;
    }

    private static string? NombreDeCliente(List<ClienteExterno> clientes, string? documento)
        => clientes.FirstOrDefault(c => Igual(c.DocumentoFiscal, documento) || Igual(c.Documento, documento))?.Nombre;

    // ───────── Utilidades ─────────

    private async Task<List<T>> LeerAsync<T>(MaestroExterno maestro, ContextoUsuario contexto, CancellationToken ct)
    {
        var json = await maestros.ConsultarAsync(maestro, contexto.IdEmpresa, contexto.Ruc, contexto.Usuario, formatoSp: false, cancellationToken: ct);
        return json.Deserialize<List<T>>(JsonOptions) ?? [];
    }

    /// <summary>Filas de la propiedad "data" de un sobre de SP (vacio si no es un arreglo).</summary>
    private static Task<List<JsonObject>> FilasAsync(JsonElement sobre)
    {
        var filas = new List<JsonObject>();
        if (sobre.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in data.EnumerateArray())
            {
                if (JsonNode.Parse(item.GetRawText()) is JsonObject fila) filas.Add(fila);
            }
        }

        return Task.FromResult(filas);
    }

    /// <summary>Si la propiedad es un texto con JSON, la reemplaza por el JSON ya interpretado y lo devuelve.</summary>
    private static JsonNode? ConvertirTextoEnJson(JsonObject objeto, string propiedad)
    {
        var valor = objeto[propiedad];
        if (valor is JsonValue json && json.TryGetValue<string>(out var texto) && !string.IsNullOrWhiteSpace(texto))
        {
            var interpretado = JsonNode.Parse(texto);
            if (interpretado is not null)
            {
                objeto[propiedad] = interpretado;
            }

            return interpretado;
        }

        return valor;
    }

    /// <summary>Lee una propiedad como texto sin importar mayusculas; acepta textos y numeros. Devuelve null si no existe o es null.</summary>
    internal static string? Texto(JsonObject objeto, string propiedad)
    {
        foreach (var par in objeto)
        {
            if (!string.Equals(par.Key, propiedad, StringComparison.OrdinalIgnoreCase)) continue;
            return par.Value switch
            {
                null => null,
                JsonValue valor when valor.TryGetValue<string>(out var texto) => texto,
                var otro => otro.ToJsonString().Trim('"'),
            };
        }

        return null;
    }

    private static string Unir(params string?[] partes) => string.Join(" ", partes.Where(p => !string.IsNullOrWhiteSpace(p)));

    private static bool Igual(string? a, string? b)
        => !string.IsNullOrWhiteSpace(a) && !string.IsNullOrWhiteSpace(b) && a.Trim().Equals(b.Trim(), StringComparison.OrdinalIgnoreCase);
}
