using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Procesos de empacado (un turno de un acopio en una fecha). Devuelve la respuesta del SP: { error, data, mensaje }.
/// Empresa, RUC, usuario, rol y acopio salen del token. Requiere token.
/// </summary>
[ApiController]
[Route("api/procesos")]
public sealed class ProcesosController(IProcesosUseCase procesos) : ControllerBase
{
    /// <summary>Procesos de la campania. El administrador ve los de todos los acopios; los demas, solo los de su acopio.</summary>
    [HttpPost("listar")]
    public async Task<IActionResult> Listar([FromBody] ListarProcesosRequest request, CancellationToken ct)
        => Ok(await procesos.ListarAsync(request, ct));

    /// <summary>Busca si ya hay un proceso para esa fecha y turno (antes de abrir uno nuevo).</summary>
    [HttpPost("buscar")]
    public async Task<IActionResult> Buscar([FromBody] BuscarProcesoRequest request, CancellationToken ct)
        => Ok(await procesos.BuscarAsync(request, ct));

    [HttpPost("supervisores-disponibles")]
    public async Task<IActionResult> SupervisoresDisponibles([FromBody] PersonalDisponibleRequest request, CancellationToken ct)
        => Ok(await procesos.ListarSupervisoresDisponiblesAsync(request, ct));

    [HttpPost("personal-logistico-disponible")]
    public async Task<IActionResult> PersonalLogisticoDisponible([FromBody] PersonalDisponibleRequest request, CancellationToken ct)
        => Ok(await procesos.ListarPersonalLogisticoDisponibleAsync(request, ct));

    [HttpPost("tipo-proceso-empacado-acopio")]
    public async Task<IActionResult> TipoProcesoEmpacadoAcopio([FromBody] ProyectoRequest request, CancellationToken ct)
        => Ok(await procesos.ListarTipoProcesoEmpacadoPorAcopioAsync(request, ct));

    /// <summary>Crea, cierra, reabre o edita un proceso con su personal, segun el campo "modo".</summary>
    [HttpPost("sincronizar")]
    public async Task<IActionResult> Sincronizar([FromBody] SincronizarProcesoRequest request, CancellationToken ct)
        => Ok(await procesos.SincronizarAsync(request, ct));
}
