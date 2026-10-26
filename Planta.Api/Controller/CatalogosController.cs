using Microsoft.AspNetCore.Mvc;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.UseCase;

namespace Planta.Api.Controller;

/// <summary>
/// Catalogos de la base de datos propia. Cada consulta devuelve la respuesta del SP: { error, data, mensaje }.
/// Empresa y RUC salen del token. Requiere token (politica por defecto).
/// </summary>
[ApiController]
[Route("api/catalogos")]
public sealed class CatalogosController(ICatalogosLocalUseCase catalogos) : ControllerBase
{
    [HttpPost("clientes")]
    public async Task<IActionResult> Clientes(CancellationToken ct)
        => Ok(await catalogos.ListarGrupoClienteAsync(ct));

    /// <summary>catalogo: tiposProcesoEmpacado, codigosRancho, lugaresProduccion, conductores, vehiculos, transportistas, supervisores o personalLogistico.</summary>
    [HttpPost("proyecto/{catalogo}")]
    public async Task<IActionResult> PorProyecto(CatalogoPorProyecto catalogo, [FromBody] ProyectoRequest request, CancellationToken ct)
        => Ok(await catalogos.ListarPorProyectoAsync(catalogo, request.IdProyecto, ct));

    /// <summary>catalogo: formatos, tiposClamshell, tiposCaja, presentaciones, tiposEmpaqueGuia, categorias o tiposEmpaque.</summary>
    [HttpPost("cultivo/{catalogo}")]
    public async Task<IActionResult> PorCultivo(CatalogoPorCultivo catalogo, [FromBody] CultivoRequest request, CancellationToken ct)
        => Ok(await catalogos.ListarPorCultivoAsync(catalogo, request.CodigoCultivo, ct));

    [HttpPost("destinatarios")]
    public async Task<IActionResult> Destinatarios(CancellationToken ct)
        => Ok(await catalogos.ListarDestinatariosAsync(ct));

    [HttpPost("consignatarios")]
    public async Task<IActionResult> Consignatarios(CancellationToken ct)
        => Ok(await catalogos.ListarConsignatariosAsync(ct));

    [HttpPost("variedades")]
    public async Task<IActionResult> Variedades(CancellationToken ct)
        => Ok(await catalogos.ListarVariedadesAsync(ct));

    [HttpPost("motivos-traslado")]
    public async Task<IActionResult> MotivosTraslado(CancellationToken ct)
        => Ok(await catalogos.ListarMotivosTrasladoAsync(ct));

    /// <summary>Unidades de medida.</summary>
    [HttpPost("unidades-medida")]
    public async Task<IActionResult> UnidadesMedida(CancellationToken ct)
        => Ok(await catalogos.ListarUnidadesMedidaAsync(ct));

    /// <summary>Tipos de locacion (puerto o aeropuerto).</summary>
    [HttpPost("tipos-locacion")]
    public async Task<IActionResult> TiposLocacion(CancellationToken ct)
        => Ok(await catalogos.ListarTiposLocacionAsync(ct));

    /// <summary>Puertos.</summary>
    [HttpPost("puertos")]
    public async Task<IActionResult> Puertos(CancellationToken ct)
        => Ok(await catalogos.ListarPuertosAsync(ct));

    /// <summary>Aeropuertos.</summary>
    [HttpPost("aeropuertos")]
    public async Task<IActionResult> AeroPuertos(CancellationToken ct)
        => Ok(await catalogos.ListarAeroPuertosAsync(ct));

    [HttpPost("establecimiento-emisor")]
    public async Task<IActionResult> EstablecimientoEmisor(CancellationToken ct)
        => Ok(await catalogos.ObtenerEstablecimientoEmisorAsync(ct));

    [HttpPost("establecimientos")]
    public async Task<IActionResult> Establecimientos(CancellationToken ct)
        => Ok(await catalogos.ListarEstablecimientosAsync(ct));

    [HttpPost("departamentos")]
    public async Task<IActionResult> Departamentos(CancellationToken ct)
        => Ok(await catalogos.ListarDepartamentosAsync(ct));

    [HttpPost("provincias")]
    public async Task<IActionResult> Provincias([FromBody] ProvinciasRequest request, CancellationToken ct)
        => Ok(await catalogos.ListarProvinciasAsync(request.CodigoDepartamento, ct));

    [HttpPost("distritos")]
    public async Task<IActionResult> Distritos([FromBody] DistritosRequest request, CancellationToken ct)
        => Ok(await catalogos.ListarDistritosAsync(request.CodigoDepartamento, request.CodigoProvincia, ct));

    [HttpPost("acopios")]
    public async Task<IActionResult> Acopios([FromBody] ProyectoRequest request, CancellationToken ct)
        => Ok(await catalogos.ListarAcopiosAsync(request.IdProyecto, ct));
}
