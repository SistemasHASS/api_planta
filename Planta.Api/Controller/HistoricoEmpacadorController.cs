using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Histórico de empacadores: quiénes fueron asignados a un puesto y, de cada uno, jornada, proceso, línea o curo, puesto y cajas.
/// Lo usan el administrador y el monitor (todas las plantas) y el anotador (solo la de su token). Devuelve la respuesta del SP: { error, data, mensaje }.
/// </summary>
[ApiController]
[Route("api/reportes/historico-empacador")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Monitor},{Roles.Anotador}")]
public sealed class HistoricoEmpacadorController(IReportesUseCase reportes) : ControllerBase
{
    /// <summary>Los empacadores asignados a un puesto en un rango de fechas de proceso, con sus totales.</summary>
    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] HistoricoEmpacadoresRequest request, CancellationToken ct)
        => Ok(await reportes.HistoricoEmpacadoresAsync(request, ct));

    /// <summary>Las asignaciones de un empacador (jornada, proceso, línea o curo, puesto y cajas), de la más reciente a la más antigua.</summary>
    [HttpPost("detalle")]
    public async Task<IActionResult> Detalle([FromBody] HistoricoEmpacadorDetalleRequest request, CancellationToken ct)
        => Ok(await reportes.HistoricoEmpacadorDetalleAsync(request, ct));
}
