using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class ReportesServiceImpl(IReportesRepository repository, IMaestrosService maestros, IExcelService excel) : IReportesService
{
    private const string ParametroRucExterno = "REPORTE_KG_INGRESADOS_EXTERNO";
    private const string ParametroReporteBh = "REPORTE_BH";

    public Task<JsonElement> ReporteDiarioAsync(ContextoUsuario contexto, ReporteDiarioRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteDiarioAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ReporteSemanalFiltrosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => repository.ReporteSemanalFiltrosAsync(contexto, idProyecto, cancellationToken);

    public Task<JsonElement> ReporteSemanalAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteSemanalAsync(contexto, new ReporteSemanalParametros(
            request.IdProyecto,
            ComoJson(request.Semanas),
            ComoJson(request.Variedades),
            ComoJson(request.Formatos),
            ComoJson(request.Destinos),
            ComoJson(request.Clientes),
            ComoJson(request.Consignatarios)), cancellationToken);

    public Task<JsonElement> ReporteCampaniaAsync(ContextoUsuario contexto, ReporteSemanalRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteCampaniaAsync(contexto, new ReporteSemanalParametros(
            request.IdProyecto,
            ComoJson(request.Semanas),
            ComoJson(request.Variedades),
            ComoJson(request.Formatos),
            ComoJson(request.Destinos),
            ComoJson(request.Clientes),
            ComoJson(request.Consignatarios)), cancellationToken);

    public async Task<string?> RucExternoKgAcopioAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarParametrosAsync(contexto, cancellationToken);
        if (!sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        foreach (var parametro in data.EnumerateArray())
        {
            var id = parametro.TryGetProperty("idparametro", out var idJson) ? idJson.GetString()?.Trim() : null;
            var valor = parametro.TryGetProperty("valor", out var valorJson) ? valorJson.GetString()?.Trim() : null;
            if (string.Equals(id, ParametroRucExterno, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(valor)
                && !string.Equals(valor, contexto.Ruc, StringComparison.OrdinalIgnoreCase))
            {
                return valor;
            }
        }

        return null;
    }

    public async Task<JsonElement> CampaniasKgAcopioAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken = default)
    {
        await ValidarRucAsync(contexto, ruc, cancellationToken);
        return await maestros.ConsultarAsync(MaestroExterno.Campanias, contexto.IdEmpresa, ruc.Trim(), null, false, cancellationToken);
    }

    public async Task<JsonElement> ReporteKgAcopioAsync(ContextoUsuario contexto, ReporteKgAcopioRequest request, CancellationToken cancellationToken = default)
    {
        await ValidarRucAsync(contexto, request.Ruc, cancellationToken);
        request.Ruc = request.Ruc.Trim();
        return await repository.ReporteKgAcopioAsync(contexto, request, cancellationToken);
    }

    public async Task<bool> ReporteBhHabilitadoAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
    {
        var sobre = await repository.ListarParametrosAsync(contexto, cancellationToken);
        if (!sobre.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        // PLANTA_ListarParametros ya devuelve solo los parametros activos.
        return data.EnumerateArray().Any(parametro =>
            parametro.TryGetProperty("idparametro", out var id)
            && string.Equals(id.GetString()?.Trim(), ParametroReporteBh, StringComparison.OrdinalIgnoreCase));
    }

    public Task<JsonElement> ReporteDiarioPropioExternoAsync(ContextoUsuario contexto, ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default)
        => repository.ReporteDiarioPropioExternoAsync(contexto, request, cancellationToken);

    public async Task<JsonElement> CampaniasAvanceCosechadorAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken = default)
    {
        await ValidarEmpresaAsync(contexto, ruc, cancellationToken);
        return await maestros.ConsultarAsync(MaestroExterno.Campanias, contexto.IdEmpresa, ruc.Trim(), null, false, cancellationToken);
    }

    public async Task<JsonElement> ReporteAvanceCosechadorAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default)
    {
        await ValidarEmpresaAsync(contexto, request.Ruc, cancellationToken);
        request.Ruc = request.Ruc.Trim();
        return await repository.ReporteAvanceCosechadorAsync(contexto, request, cancellationToken);
    }

    public async Task<JsonElement> ReporteAvanceGrupoAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default)
    {
        await ValidarEmpresaAsync(contexto, request.Ruc, cancellationToken);
        request.Ruc = request.Ruc.Trim();
        return await repository.ReporteAvanceGrupoAsync(contexto, request, cancellationToken);
    }

    public async Task<ArchivoGenerado> ExportarAvanceGrupoExcelAsync(ContextoUsuario contexto, ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default)
    {
        var sobre = await ReporteAvanceGrupoAsync(contexto, request, cancellationToken);
        if (sobre.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            var mensaje = sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
            throw new BadRequestException(mensaje ?? "No se pudo obtener el avance por grupo.");
        }

        var filas = new System.Text.Json.Nodes.JsonArray();
        if (sobre.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object
            && data.TryGetProperty("grupos", out var grupos) && grupos.ValueKind == JsonValueKind.Array)
        {
            foreach (var g in grupos.EnumerateArray())
            {
                filas.Add(new System.Text.Json.Nodes.JsonObject
                {
                    ["GRUPO"] = Texto(g, "grupo"),
                    ["LOTE"] = Texto(g, "lote"),
                    ["VARIEDAD"] = Texto(g, "variedad"),
                    ["DNI SUPERVISOR"] = Texto(g, "dniSupervisor"),
                    ["SUPERVISOR"] = Texto(g, "supervisor"),
                    ["COSECHADORES"] = Numero(g, "cosechadores"),
                    ["JARRA BLANCA"] = Numero(g, "jarraBlanca"),
                    ["JARRA VERDE"] = Numero(g, "jarraVerde"),
                    ["DESCARTE"] = Numero(g, "descarte"),
                    ["TOTAL"] = Numero(g, "total"),
                    ["% DESCARTE"] = Numero(g, "porcentajeDescarte"),
                    ["PROMEDIO"] = Numero(g, "promedioPorCosechador"),
                    ["VIAJES"] = Numero(g, "viajes"),
                });
            }
        }

        if (filas.Count == 0)
        {
            throw new BadRequestException("No se encontraron datos para exportar.");
        }

        var bytes = excel.GenerarTabla("Avance por grupo", JsonSerializer.SerializeToElement(filas));
        var desde = request.FechaDesde.Trim();
        var hasta = string.IsNullOrWhiteSpace(request.FechaHasta) ? desde : request.FechaHasta.Trim();
        return new ArchivoGenerado(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"avance-grupo-{desde}_{hasta}.xlsx");
    }

    public async Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ContextoUsuario contexto, ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default)
    {
        await ValidarEmpresaAsync(contexto, request.Ruc, cancellationToken);
        request.Ruc = request.Ruc.Trim();
        return await repository.ReporteAvanceCosechadorFiltrosAsync(contexto, request, cancellationToken);
    }

    public async Task<ArchivoGenerado> ExportarAvanceCosechadorExcelAsync(ContextoUsuario contexto, ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default)
    {
        var sobre = await ReporteAvanceCosechadorAsync(contexto, request, cancellationToken);
        if (sobre.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            var mensaje = sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
            throw new BadRequestException(mensaje ?? "No se pudo obtener el avance por cosechador.");
        }

        var filas = new System.Text.Json.Nodes.JsonArray();
        if (sobre.TryGetProperty("data", out var data) && data.ValueKind == JsonValueKind.Object
            && data.TryGetProperty("cosechadores", out var cosechadores) && cosechadores.ValueKind == JsonValueKind.Array)
        {
            foreach (var c in cosechadores.EnumerateArray())
            {
                filas.Add(new System.Text.Json.Nodes.JsonObject
                {
                    ["GRUPO"] = Texto(c, "grupo"),
                    ["DNI"] = Texto(c, "dni"),
                    ["COSECHADOR"] = Texto(c, "nombreCompleto"),
                    ["JARRA BLANCA"] = Numero(c, "jarraBlanca"),
                    ["JARRA VERDE"] = Numero(c, "jarraVerde"),
                    ["DESCARTE"] = Numero(c, "descarte"),
                    ["TOTAL"] = Numero(c, "total"),
                    ["% DESCARTE"] = Numero(c, "porcentajeDescarte"),
                    ["DÍAS"] = Numero(c, "diasTrabajados"),
                    ["VIAJES"] = Numero(c, "viajes"),
                });
            }
        }

        if (filas.Count == 0)
        {
            throw new BadRequestException("No se encontraron datos para exportar.");
        }

        var bytes = excel.GenerarTabla("Avance por cosechador", JsonSerializer.SerializeToElement(filas));
        var desde = request.FechaDesde.Trim();
        var hasta = string.IsNullOrWhiteSpace(request.FechaHasta) ? desde : request.FechaHasta.Trim();
        return new ArchivoGenerado(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"avance-cosechador-{desde}_{hasta}.xlsx");
    }

    public Task<JsonElement> ProcesosPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueProcesosRequest request, CancellationToken cancellationToken = default)
    {
        var fecha = (request.Fecha ?? string.Empty).Trim();
        if (fecha.Length == 0)
        {
            throw new BadRequestException("Selecciona una fecha.");
        }

        if (!DateOnly.TryParseExact(fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
        {
            throw new BadRequestException("La fecha debe venir como AAAA-MM-DD.");
        }

        // El acopio vacío es "todas las plantas".
        return repository.ProcesosPlanillaEmpaqueAsync(contexto, new PlanillaEmpaqueProcesosRequest { Fecha = fecha, CodigoAcopio = request.CodigoAcopio?.Trim() }, cancellationToken);
    }

    public Task<JsonElement> AnotadoresPlanillaEmpaqueAsync(ContextoUsuario contexto, PlanillaEmpaqueAnotadoresRequest request, CancellationToken cancellationToken = default)
        => repository.AnotadoresPlanillaEmpaqueAsync(contexto, new PlanillaEmpaqueAnotadoresRequest
        {
            CodigoAcopio = (request.CodigoAcopio ?? string.Empty).Trim(),
            IdProceso = (request.IdProceso ?? string.Empty).Trim(),
        }, cancellationToken);

    public async Task<ArchivoGenerado> ExportarPlanillaProduccionExcelAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default)
    {
        request = LimpiarPlanillaEmpaque(request);
        var sobre = await repository.PlanillaEmpaqueProduccionAsync(contexto, request, cancellationToken);
        var data = DataDePlanilla(sobre, "No se pudo obtener la planilla de producción.");
        var armado = PlanillaEmpaqueArmador.Produccion(data)
            ?? throw new BadRequestException(SinMovimientoPlanilla(request, "No hay cajas registradas en ese proceso."));

        armado.Planilla.Logo = await LogoDeEmpresaAsync(webRootPath, contexto.Ruc, cancellationToken);
        var bytes = excel.GenerarPlanillaEmpaque(armado.Planilla);
        return new ArchivoGenerado(bytes, TipoContenidoExcel, PlanillaEmpaqueArmador.NombreArchivo("produccion_empaque", armado, request.IdUsuarioAnotador));
    }

    public async Task<ArchivoGenerado> ExportarPlanillaTareoExcelAsync(ContextoUsuario contexto, PlanillaEmpaqueRequest request, string? webRootPath, CancellationToken cancellationToken = default)
    {
        request = LimpiarPlanillaEmpaque(request);
        var sobre = await repository.PlanillaEmpaqueTareoAsync(contexto, request, cancellationToken);
        var data = DataDePlanilla(sobre, "No se pudo obtener el tareo.");
        var armado = PlanillaEmpaqueArmador.Tareo(data)
            ?? throw new BadRequestException(SinMovimientoPlanilla(request, "Todavía no hay empacadores asignados en ese proceso."));

        armado.Planilla.Logo = await LogoDeEmpresaAsync(webRootPath, contexto.Ruc, cancellationToken);
        var bytes = excel.GenerarPlanillaEmpaque(armado.Planilla);
        return new ArchivoGenerado(bytes, TipoContenidoExcel, PlanillaEmpaqueArmador.NombreArchivo("tareo", armado, request.IdUsuarioAnotador));
    }

    /// <summary>El logo de la empresa del usuario (wwwroot/assets/logos/{RUC}.jpg), el mismo que usa la ficha de composición del palet. Sin archivo, null: la planilla sale sin logo.</summary>
    private static async Task<byte[]?> LogoDeEmpresaAsync(string? webRootPath, string? ruc, CancellationToken cancellationToken)
    {
        var rucLimpio = ruc?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(webRootPath) || rucLimpio.Length == 0 || rucLimpio != Path.GetFileName(rucLimpio))
        {
            return null;
        }

        var ruta = Path.Combine(webRootPath, "assets", "logos", $"{rucLimpio}.jpg");
        return File.Exists(ruta) ? await File.ReadAllBytesAsync(ruta, cancellationToken) : null;
    }

    private const string TipoContenidoExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private static PlanillaEmpaqueRequest LimpiarPlanillaEmpaque(PlanillaEmpaqueRequest request)
        => new()
        {
            CodigoAcopio = (request.CodigoAcopio ?? string.Empty).Trim(),
            IdProceso = (request.IdProceso ?? string.Empty).Trim(),
            IdUsuarioAnotador = request.IdUsuarioAnotador?.Trim(),
        };

    /// <summary>El data del SP; si el SP devolvió error se lanza con su mensaje.</summary>
    private static JsonElement DataDePlanilla(JsonElement sobre, string mensajePorDefecto)
    {
        if (sobre.TryGetProperty("error", out var error) && error.ValueKind == JsonValueKind.True)
        {
            var mensaje = sobre.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String ? m.GetString() : null;
            throw new BadRequestException(string.IsNullOrWhiteSpace(mensaje) ? mensajePorDefecto : mensaje);
        }

        return sobre.TryGetProperty("data", out var data) ? data : default;
    }

    /// <summary>El aviso cuando el proceso no tiene datos; si se pidió un anotador, dice que es de ese anotador.</summary>
    private static string SinMovimientoPlanilla(PlanillaEmpaqueRequest request, string general)
        => string.IsNullOrEmpty(request.IdUsuarioAnotador) ? general : "El anotador elegido no tiene datos en este proceso.";

    private static string Texto(JsonElement fila, string propiedad)
        => fila.TryGetProperty(propiedad, out var v) && v.ValueKind != JsonValueKind.Null ? v.ToString().Trim() : "";

    private static decimal Numero(JsonElement fila, string propiedad)
        => fila.TryGetProperty(propiedad, out var v) && v.ValueKind == JsonValueKind.Number && v.TryGetDecimal(out var n) ? n : 0;

    /// <summary>El RUC debe ser el de una empresa que devuelve apimaestra (get_empresas).</summary>
    private async Task ValidarEmpresaAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken)
    {
        var rucPedido = ruc?.Trim() ?? "";
        var empresas = await maestros.ConsultarAsync(MaestroExterno.Empresas, contexto.IdEmpresa, contexto.Ruc, null, false, cancellationToken);
        var existe = empresas.ValueKind == JsonValueKind.Array
            && empresas.EnumerateArray().Any(e =>
                e.TryGetProperty("ruc", out var r)
                && string.Equals(r.GetString()?.Trim(), rucPedido, StringComparison.OrdinalIgnoreCase));
        if (rucPedido.Length == 0 || !existe)
        {
            throw new BadRequestException("La empresa indicada no existe.");
        }
    }

    /// <summary>Solo se puede consultar el RUC propio o el externo autorizado por parametro.</summary>
    private async Task ValidarRucAsync(ContextoUsuario contexto, string ruc, CancellationToken cancellationToken)
    {
        var rucPedido = ruc?.Trim() ?? "";
        if (rucPedido.Length == 0 || string.Equals(rucPedido, contexto.Ruc, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var externo = await RucExternoKgAcopioAsync(contexto, cancellationToken);
        if (!string.Equals(externo, rucPedido, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("No tiene permiso para consultar la empresa indicada.");
        }
    }

    private static string? ComoJson(List<string>? valores)
        => valores is { Count: > 0 } ? JsonSerializer.Serialize(valores) : null;
}
