using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Reportes del administrador. Devuelve la respuesta del SP: { error, data, mensaje }.
/// Empresa y RUC salen del token. Pantallas solo online: no se guarda nada en Dexie.
/// </summary>
[ApiController]
[Route("api/reportes")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Monitor}")]
public sealed class ReportesController(IReportesUseCase reportes) : ControllerBase
{
    /// <summary>Reporte diario: KPIs de pallets/cajas/kg, produccion por variedad, kg por acopio y avance por consignatario.</summary>
    [HttpPost("reporte-diario")]
    public async Task<IActionResult> ReporteDiario([FromBody] ReporteDiarioRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteDiarioAsync(request, ct));

    /// <summary>Opciones de los filtros del reporte semanal (semanas, variedades, formatos, destinos, clientes y consignatarios).</summary>
    [HttpPost("reporte-semanal/filtros")]
    public async Task<IActionResult> ReporteSemanalFiltros([FromBody] ProyectoRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteSemanalFiltrosAsync(request, ct));

    /// <summary>Reporte semanal. Cada lista de filtros vacia significa "todos".</summary>
    [HttpPost("reporte-semanal")]
    public async Task<IActionResult> ReporteSemanal([FromBody] ReporteSemanalRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteSemanalAsync(request, ct));

    /// <summary>Reporte de campania (acumulado por semana). Usa los mismos filtros que el semanal; lista vacia significa "todos".</summary>
    [HttpPost("reporte-campania")]
    public async Task<IActionResult> ReporteCampania([FromBody] ReporteSemanalRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteCampaniaAsync(request, ct));

    /// <summary>RUC de la empresa externa que el usuario puede consultar en KG ingresados: { rucExterno } (null si no tiene).</summary>
    [HttpPost("reporte-kg-acopio/empresa-externa")]
    public async Task<IActionResult> ReporteKgAcopioEmpresaExterna(CancellationToken ct)
        => Ok(await reportes.ReporteKgAcopioEmpresaExternaAsync(ct));

    /// <summary>Campañas de la empresa propia o de la externa autorizada.</summary>
    [HttpPost("reporte-kg-acopio/campanias")]
    public async Task<IActionResult> ReporteKgAcopioCampanias([FromBody] RucRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteKgAcopioCampaniasAsync(request, ct));

    /// <summary>KG de materia prima ingresada a los acopios (bruto, destaraje y neto) de un dia.</summary>
    [HttpPost("reporte-kg-acopio")]
    public async Task<IActionResult> ReporteKgAcopio([FromBody] ReporteKgAcopioRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteKgAcopioAsync(request, ct));

    /// <summary>Indica si el reporte BH / CAO esta habilitado para la empresa (parametro REPORTE_BH): { habilitado }.</summary>
    [HttpPost("reporte-bh/habilitado")]
    public async Task<IActionResult> ReporteBhHabilitado(CancellationToken ct)
        => Ok(await reportes.ReporteBhHabilitadoAsync(ct));

    /// <summary>Reporte diario de produccion separado por origen: BH (propio) y CAO (externo).</summary>
    [HttpPost("reporte-diario-propio-externo")]
    public async Task<IActionResult> ReporteDiarioPropioExterno([FromBody] ReporteDiarioPropioExternoRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteDiarioPropioExternoAsync(request, ct));

    /// <summary>Campañas de la empresa elegida en el select (RUC de apimaestra).</summary>
    [HttpPost("reporte-avance-cosechador/campanias")]
    public async Task<IActionResult> ReporteAvanceCosechadorCampanias([FromBody] RucRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteAvanceCosechadorCampaniasAsync(request, ct));

    /// <summary>Avance por grupo y supervisor: jarra blanca, jarra verde y descarte de la empresa elegida.</summary>
    [HttpPost("reporte-avance-grupo")]
    public async Task<IActionResult> ReporteAvanceGrupo([FromBody] ReporteAvanceGrupoRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteAvanceGrupoAsync(request, ct));

    /// <summary>Excel de la tabla "Detalle por grupo" con los mismos filtros del reporte.</summary>
    [HttpPost("reporte-avance-grupo/exportar-excel")]
    public async Task<IActionResult> ExportarAvanceGrupoExcel([FromBody] ReporteAvanceGrupoRequest request, CancellationToken ct)
    {
        var archivo = await reportes.ExportarAvanceGrupoExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Opciones de los filtros grupo y cosechador para el rango de fechas (los cosechadores se limitan a los grupos enviados).</summary>
    [HttpPost("reporte-avance-cosechador/filtros")]
    public async Task<IActionResult> ReporteAvanceCosechadorFiltros([FromBody] ReporteAvanceCosechadorFiltrosRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteAvanceCosechadorFiltrosAsync(request, ct));

    /// <summary>Excel de la tabla "Detalle por cosechador" con los mismos filtros del reporte.</summary>
    [HttpPost("reporte-avance-cosechador/exportar-excel")]
    public async Task<IActionResult> ExportarAvanceCosechadorExcel([FromBody] ReporteAvanceCosechadorRequest request, CancellationToken ct)
    {
        var archivo = await reportes.ExportarAvanceCosechadorExcelAsync(request, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Avance por cosechador: jarra blanca, jarra verde y descarte (por cosechador y por dia) de la empresa elegida.</summary>
    [HttpPost("reporte-avance-cosechador")]
    public async Task<IActionResult> ReporteAvanceCosechador([FromBody] ReporteAvanceCosechadorRequest request, CancellationToken ct)
        => Ok(await reportes.ReporteAvanceCosechadorAsync(request, ct));
}
