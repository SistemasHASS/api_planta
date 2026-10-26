using System.Text.Json;
using System.Text.Json.Nodes;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class AdministracionServiceImpl(IAdministracionRepository repository, IMaestrosService maestros, ICatalogosLocalService catalogos, IExcelService excel) : IAdministracionService
{
    public Task<JsonElement> ListarMatricesCompatibilidadAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => repository.ListarMatricesCompatibilidadAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarReglasSobrepesoAsync(ContextoUsuario contexto, ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => repository.ListarReglasSobrepesoAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarCorrelativosDocumentosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarCorrelativosDocumentosAsync(contexto, cancellationToken);

    /// <summary>
    /// Reporte "Códigos caja": el SP trae cada combinación con los nombres de la base propia; aquí se completan consignatario,
    /// destino, variedad, transporte y fruta (apimaestra y catálogos) y la descripción completa, igual que el detalle de la guía.
    /// </summary>
    public async Task<JsonElement> ListarCodigosCajaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarCodigosCajaDetalleAsync(contexto, cancellationToken);
        if (sobre.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            return sobre;
        }

        var data = sobre.TryGetProperty("data", out var filas) ? filas : default;
        var enriquecidas = await new GuiasEnriquecedor(maestros, catalogos).EnriquecerCodigosCajaAsync(contexto, data, cancellationToken);
        var mensaje = sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
        return JsonSerializer.SerializeToElement(new { error = false, data = enriquecidas, mensaje });
    }

    /// <summary>
    /// Excel de códigos caja con solo las filas que la persona ve en la tabla: los id van al SP PLANTA_ExportarCodigosCajaDetalle,
    /// que las busca dentro de la empresa del token (otros id no devuelven nada). Aquí solo se completan nombres y se arma el Excel.
    /// </summary>
    public async Task<ArchivoGenerado> ExportarCodigosCajaExcelAsync(ContextoUsuario contexto, ExportarCodigosCajaRequest request, CancellationToken cancellationToken = default)
    {
        var ids = (request.Ids ?? []).Distinct().ToList();
        if (ids.Count == 0)
        {
            throw new BadRequestException("No hay códigos caja para exportar.");
        }

        var sobre = await repository.ExportarCodigosCajaDetalleAsync(contexto, JsonSerializer.Serialize(ids), cancellationToken);
        if (sobre.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            var mensaje = sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
            throw new BadRequestException(mensaje ?? "No se pudo obtener los códigos caja.");
        }

        var data = sobre.TryGetProperty("data", out var filasSp) ? filasSp : default;
        var enriquecidas = await new GuiasEnriquecedor(maestros, catalogos).EnriquecerCodigosCajaAsync(contexto, data, cancellationToken);

        var filas = new JsonArray();
        foreach (var fila in enriquecidas.OfType<JsonObject>())
        {
            var ldp = GuiasEnriquecedor.Texto(fila, "codigoLugarProduccion")?.Trim();
            var peso = GuiasEnriquecedor.Texto(fila, "pesoPorCaja")?.Trim();
            filas.Add(new JsonObject
            {
                ["CÓDIGO"] = GuiasEnriquecedor.Texto(fila, "codigoItem")?.Trim(),
                ["CULTIVO"] = ValorExcel(fila, "fruta", "codigoCultivo"),
                ["CONSIGNATARIO"] = ValorExcel(fila, "nombreConsignatario", "documentoConsignatario"),
                ["DESTINO"] = ValorExcel(fila, "nombreDestino", "idDestino"),
                ["PRESENTACIÓN"] = ValorExcel(fila, "nombrePresentacion", "idPresentacion"),
                ["FORMATO"] = ValorExcel(fila, "nombreFormato", "codigoFormato"),
                ["VARIEDAD"] = ValorExcel(fila, "nombreVariedad", "codigoVariedad"),
                ["PESO POR CAJA (KG)"] = string.IsNullOrEmpty(peso) ? "-" : peso,
                ["TIPO EMPAQUE GUÍA"] = ValorExcel(fila, "nombreTipoEmpaqueGuia", "idTipoEmpaqueGuia"),
                ["RANCHO"] = ValorExcel(fila, "codigoRancho", "idCodigoRancho"),
                ["LDP"] = string.IsNullOrEmpty(ldp) ? "-" : $"LDP {ldp}",
                ["LUGAR DE PRODUCCIÓN"] = ValorExcel(fila, "nombreLugarProduccion", "idLugarProduccion"),
                ["TRANSPORTE"] = ValorExcel(fila, "nombreTransporte", "idTransporte"),
                ["DESCRIPCIÓN"] = GuiasEnriquecedor.Texto(fila, "descripcion")?.Trim(),
            });
        }

        if (filas.Count == 0)
        {
            throw new BadRequestException("No se encontraron datos para exportar.");
        }

        var bytes = excel.GenerarTabla("Códigos Caja", JsonSerializer.SerializeToElement(filas));
        return new ArchivoGenerado(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "codigos-caja.xlsx");
    }

    /// <summary>Nombre del dato; si no hay, su código; si el código está vacío o es 0 (sin dato), "-".</summary>
    private static string ValorExcel(JsonObject fila, string propiedadNombre, string propiedadCodigo)
    {
        var nombre = GuiasEnriquecedor.Texto(fila, propiedadNombre)?.Trim();
        if (!string.IsNullOrEmpty(nombre)) return nombre;
        var codigo = GuiasEnriquecedor.Texto(fila, propiedadCodigo)?.Trim();
        return string.IsNullOrEmpty(codigo) || codigo == "0" ? "-" : codigo;
    }

    public Task<JsonElement> SincronizarAcopioConfigAsync(ContextoUsuario contexto, SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarAcopioConfigAsync(contexto, request, cancellationToken);

    /// <summary>Usuarios de la apimaestra (aplicacion PLANTA) con el acopio y la serie de guia que tienen asignados en la base propia.</summary>
    public async Task<JsonElement> ListarUsuariosAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var usuarios = await maestros.ConsultarAsync(MaestroExterno.Usuarios, contexto.IdEmpresa, contexto.Ruc, contexto.Usuario, formatoSp: true, cancellationToken: cancellationToken);
        return await repository.ListarUsuariosAcopioAsync(usuarios.GetRawText(), cancellationToken);
    }

    public Task<JsonElement> SincronizarUsuariosAsync(ContextoUsuario contexto, SincronizarUsuariosRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarUsuariosAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarDestinatariosAsync(ContextoUsuario contexto, SincronizarDestinatariosRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarDestinatariosAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarConsignatariosAsync(ContextoUsuario contexto, SincronizarConsignatariosRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarConsignatariosAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarVariedadesAsync(ContextoUsuario contexto, SincronizarVariedadesRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarVariedadesAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarCorrelativosDocumentosAsync(ContextoUsuario contexto, SincronizarCorrelativosRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarCorrelativosDocumentosAsync(contexto, request, cancellationToken);

    /// <summary>Tablas de catálogos (operativos y del menú Catálogos) que se pueden crear o editar desde Administración (el SP genérico acepta más; aquí se limita a estas).</summary>
    private static readonly HashSet<string> TablasOperativas = new(StringComparer.OrdinalIgnoreCase)
    {
        "PLANTA_Conductores", "PLANTA_Vehiculos", "PLANTA_Transportistas", "PLANTA_Supervisores", "PLANTA_PersonalLogistica",
        "PLANTA_TipoProcesoEmpacado", "PLANTA_GrupoCliente", "PLANTA_Formatos", "PLANTA_Presentacion", "PLANTA_TiposCaja", "PLANTA_TiposClamshell",
        "PLANTA_LugaresProduccion", "PLANTA_CodigosRancho", "PLANTA_Categorias", "PLANTA_TiposEmpaque", "PLANTA_TiposEmpaqueGuia",
    };

    public Task<JsonElement> SincronizarCatalogoOperativoAsync(ContextoUsuario contexto, SincronizarCatalogoOperativoRequest request, CancellationToken cancellationToken = default)
    {
        if (!TablasOperativas.Contains(request.Tabla?.Trim() ?? ""))
            return Task.FromResult(JsonSerializer.SerializeToElement(new { error = true, data = Array.Empty<object>(), mensaje = "Tabla no permitida." }));
        return repository.SincronizarCatalogoOperativoAsync(contexto, request, cancellationToken);
    }

    public Task<JsonElement> SincronizarReglasSobrepesoAsync(ContextoUsuario contexto, SincronizarReglasSobrepesoRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarReglasSobrepesoAsync(contexto, request, cancellationToken);

    public Task<JsonElement> SincronizarMatricesCompatibilidadAsync(ContextoUsuario contexto, SincronizarMatricesCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarMatricesCompatibilidadAsync(contexto, request, cancellationToken);
}
