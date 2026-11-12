using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Tareo y Cajas Empaque (Excel de producción y de tareo por proceso). Lo usan el administrador, el monitor (solo procesos cerrados) y el anotador
/// (procesos abiertos y cerrados de su acopio). Devuelve la respuesta del SP: { error, data, mensaje }; empresa, RUC, rol y acopio salen del token.
/// </summary>
[ApiController]
[Route("api/reportes/planilla-empaque")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Monitor},{Roles.Anotador}")]
public sealed class PlanillaEmpaqueController(IReportesUseCase reportes, IWebHostEnvironment entorno) : ControllerBase
{
    /// <summary>Los procesos de un rango de fechas para elegir de cuál sacar el Tareo o las Cajas de empaque (con Flujo_Empaque activo). Administrador y monitor: solo procesos cerrados; anotador: abiertos y cerrados de su acopio.</summary>
    [HttpPost("procesos")]
    public async Task<IActionResult> ProcesosPlanillaEmpaque([FromBody] PlanillaEmpaqueProcesosRequest request, CancellationToken ct)
        => Ok(await reportes.ProcesosPlanillaEmpaqueAsync(request, ct));

    /// <summary>Los anotadores de un proceso, para sacar la planilla de uno solo.</summary>
    [HttpPost("anotadores")]
    public async Task<IActionResult> AnotadoresPlanillaEmpaque([FromBody] PlanillaEmpaqueAnotadoresRequest request, CancellationToken ct)
        => Ok(await reportes.AnotadoresPlanillaEmpaqueAsync(request, ct));

    /// <summary>Excel de la planilla de producción del proceso (cajas por recolección) con el formato de la empresa.</summary>
    [HttpPost("produccion")]
    public async Task<IActionResult> ExportarPlanillaProduccionExcel([FromBody] PlanillaEmpaqueRequest request, CancellationToken ct)
    {
        var archivo = await reportes.ExportarPlanillaProduccionExcelAsync(request, entorno.WebRootPath, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }

    /// <summary>Excel del tareo / asistencia del proceso (los empacadores asignados, hayan anotado cajas o no) con el formato de la empresa.</summary>
    [HttpPost("tareo")]
    public async Task<IActionResult> ExportarPlanillaTareoExcel([FromBody] PlanillaEmpaqueRequest request, CancellationToken ct)
    {
        var archivo = await reportes.ExportarPlanillaTareoExcelAsync(request, entorno.WebRootPath, ct);
        return File(archivo.Contenido, archivo.TipoContenido, archivo.Nombre);
    }
}
