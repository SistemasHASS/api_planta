

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.Proceso.Abstractions;
using Planta.Application.Maestros.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/procesos")]
[ApiController]
public sealed class ProcesosController(ILogger<ProcesosController> logger, ICurrentUserContext _currentUser, IProcesosUseCase procesosUseCase) : ControllerBase
{

    public sealed class SincronizarProcesoRequest
    {
        public JsonElement? Proceso { get; set; }
        public JsonElement? DProcesoLogisticos { get; set; }
        public JsonElement? DProcesoSupervisores { get; set; }
        public string? CodigoCultivo { get; set; }
        public string? Idproyecto { get; set; }
        public string? Modo { get; set; }
    }

    [HttpPost("sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarProceso([FromBody] SincronizarProcesoRequest request)
    {
        try
        {   
             if (string.IsNullOrEmpty(_currentUser.Role))
            {
                return BadRequest("IdRol is required");
            } 
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            var jsonProceso = ControllerJsonHelper.ExtractJson(request?.Proceso);
            var jsonDprocesoLogisticos = ControllerJsonHelper.ExtractJson(request?.DProcesoLogisticos);
            var jsonDprocesoSupervisores = ControllerJsonHelper.ExtractJson(request?.DProcesoSupervisores);

            var codigoCultivo = request?.CodigoCultivo;
            var idProyecto = request?.Idproyecto;
            var modo = request?.Modo;

            if (string.IsNullOrWhiteSpace(idProyecto))
                return BadRequest("idProyecto is required");

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            var result = await procesosUseCase.SincronizarProcesoAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto!,
                codigoCultivo!,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonProceso,
                jsonDprocesoLogisticos,
                jsonDprocesoSupervisores,
                modo!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarProceso");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-procesos-acopio")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetProcesos([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            // if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            // {
            //     return BadRequest("CodigoAcopio is required");
            // }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;


            var result = await procesosUseCase.ListarProcesosAsync(_currentUser.Role!,_currentUser.IdEmpresa!, _currentUser.Ruc!, idproyecto!, codigoCultivo!, _currentUser.CodigoAcopio ?? string.Empty);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetProcesos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-supervisores-disponibles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetSupervisoresDisponibles([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;

            var fecha = root.TryGetProperty("fecha", out var fechaProp)
                ? fechaProp.GetString()
                : null;

            var result = await procesosUseCase.GetSupervisoresDisponiblesAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, idproyecto!, fecha!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetSupervisoresDisponibles");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-personal-logistica-disponibles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPersonalLogisticaDisponibles([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;
            
            var fecha = root.TryGetProperty("fecha", out var fechaProp)
                ? fechaProp.GetString()
                : null;

            var result = await procesosUseCase.GetPersonalLogisticaDisponiblesAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, idproyecto!, fecha!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetPersonalLogisticaDisponibles");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("buscar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetBuscarProcesos([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var codigoCultivo = root.TryGetProperty("codigoCultivo", out var cultivoProp)
                ? cultivoProp.GetString()
                : null;

            var idproyecto = root.TryGetProperty("idproyecto", out var proyectoProp)
                ? proyectoProp.GetString()
                : null;

            var turno = root.TryGetProperty("turno", out var turnoProp)
                ? turnoProp.GetString()
                : null;

            var fecha = root.TryGetProperty("fecha", out var fechaProp)
                ? fechaProp.GetString()
                : null;

            var result = await procesosUseCase.BuscarProcesoAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, idproyecto!, codigoCultivo!, _currentUser.CodigoAcopio!, turno!, fecha!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetBuscarProcesos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("reporte-semanal-filtros")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ObtenerReporteSemanalFiltros([FromQuery] string idProyecto)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(idProyecto))
            {
                return BadRequest("idProyecto is required");
            }

            var result = await procesosUseCase.ObtenerReporteSemanalFiltrosAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ObtenerReporteSemanalFiltros");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpPost("reporte-semanal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ObtenerReporteSemanalDatos([FromBody] ReporteSemanalDatosRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(request.IdProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var semanas = request.Semanas is { Count: > 0 } ? JsonSerializer.Serialize(request.Semanas) : null;
            var variedades = request.Variedades is { Count: > 0 } ? JsonSerializer.Serialize(request.Variedades) : null;
            var formatos = request.Formatos is { Count: > 0 } ? JsonSerializer.Serialize(request.Formatos) : null;
            var destinos = request.Destinos is { Count: > 0 } ? JsonSerializer.Serialize(request.Destinos) : null;
            var clientes = request.Clientes is { Count: > 0 } ? JsonSerializer.Serialize(request.Clientes) : null;
            var consignatarios = request.Consignatarios is { Count: > 0 } ? JsonSerializer.Serialize(request.Consignatarios) : null;

            var result = await procesosUseCase.ObtenerReporteSemanalDatosAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                request.IdProyecto,
                semanas,
                variedades,
                formatos,
                destinos,
                clientes,
                consignatarios
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ObtenerReporteSemanalDatos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpPost("reporte-campania")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ObtenerReporteCampaniaDatos([FromBody] ReporteCampaniaDatosRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(request.IdProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var semanas = request.Semanas is { Count: > 0 } ? JsonSerializer.Serialize(request.Semanas) : null;
            var variedades = request.Variedades is { Count: > 0 } ? JsonSerializer.Serialize(request.Variedades) : null;
            var formatos = request.Formatos is { Count: > 0 } ? JsonSerializer.Serialize(request.Formatos) : null;
            var destinos = request.Destinos is { Count: > 0 } ? JsonSerializer.Serialize(request.Destinos) : null;
            var clientes = request.Clientes is { Count: > 0 } ? JsonSerializer.Serialize(request.Clientes) : null;
            var consignatarios = request.Consignatarios is { Count: > 0 } ? JsonSerializer.Serialize(request.Consignatarios) : null;

            var result = await procesosUseCase.ObtenerReporteCampaniaDatosAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                request.IdProyecto,
                semanas,
                variedades,
                formatos,
                destinos,
                clientes,
                consignatarios
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ObtenerReporteCampaniaDatos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    public sealed class ReporteSemanalDatosRequest
    {
        public string IdProyecto { get; set; } = "";
        public List<string> Semanas { get; set; } = [];
        public List<string> Variedades { get; set; } = [];
        public List<string> Formatos { get; set; } = [];
        public List<string> Destinos { get; set; } = [];
        public List<string> Clientes { get; set; } = [];
        public List<string> Consignatarios { get; set; } = [];
    }

    public sealed class ReporteCampaniaDatosRequest
    {
        public string IdProyecto { get; set; } = "";
        public List<string> Semanas { get; set; } = [];
        public List<string> Variedades { get; set; } = [];
        public List<string> Formatos { get; set; } = [];
        public List<string> Destinos { get; set; } = [];
        public List<string> Clientes { get; set; } = [];
        public List<string> Consignatarios { get; set; } = [];
    }

}


[Route("api/palets")]
[ApiController]
public class PaletsController(ILogger<PaletsController> logger, ICurrentUserContext _currentUser, IProcesosUseCase procesosUseCase, IPdfService pdfService, IMaestrosService maestrosService, IWebHostEnvironment env) : ControllerBase
{

    [HttpGet("descargar-ficha-composicion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> DescargarFichaComposicion([FromQuery] string idPalet)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(idPalet))
            {
                return BadRequest("idPalet is required");
            }

            var result = await procesosUseCase.ObtenerDatosFichaComposicionPaletAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idPalet
            );

            if (result.Count == 0)
            {
                return NotFound("No se encontraron datos para el palet especificado.");
            }

            var wrapper = result[0];
            if (wrapper.TryGetProperty("error", out var errorProp) && errorProp.GetBoolean())
            {
                var msg = wrapper.TryGetProperty("mensaje", out var msgProp) ? msgProp.GetString() : "Error desconocido";
                return BadRequest(msg);
            }

            if (!wrapper.TryGetProperty("data", out var dataProp))
            {
                return BadRequest("No se recibieron datos válidos del SP.");
            }

            var rawJson = dataProp.GetRawText();
            Planta.Application.Proceso.Models.FichaComposicionPaletModel? model = null;
            try
            {
                model = JsonSerializer.Deserialize<Planta.Application.Proceso.Models.FichaComposicionPaletModel>(rawJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException jex)
            {
                logger.LogError(jex, "Error deserializando JSON de ficha de composición. JSON RAW: {RawJson}", rawJson);
                return BadRequest($"Error de formato en los datos recibidos: {jex.Message}");
            }

            if (model == null)
            {
                return BadRequest("Error al deserializar los datos para el PDF.");
            }

            model.Detalle ??= [];
            model.Cabecera.Clientes ??= [];
            model.Cabecera.Destinos ??= [];

            // Enriquecer Clientes
            var clientesExternos = await maestrosService.GetClientesAsync(_currentUser.IdEmpresa!);
            if (clientesExternos != null)
            {
                foreach (var c in model.Cabecera.Clientes)
                {
                    var documento = c.Documento?.Trim() ?? string.Empty;
                    var match = clientesExternos.FirstOrDefault(ce =>
                        (!string.IsNullOrWhiteSpace(ce.Documento) && ce.Documento.Trim().Equals(documento, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(ce.DocumentoFiscal) && ce.DocumentoFiscal.Trim().Equals(documento, StringComparison.OrdinalIgnoreCase)));
                    if (match != null)
                    {
                        c.Cliente = match.Nombre;
                    }
                }

                foreach (var detail in model.Detalle)
                {
                    detail.Clientes ??= [];

                    foreach (var c in detail.Clientes)
                    {
                        var documento = c.Documento?.Trim() ?? string.Empty;
                        var match = clientesExternos.FirstOrDefault(ce =>
                            (!string.IsNullOrWhiteSpace(ce.Documento) && ce.Documento.Trim().Equals(documento, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrWhiteSpace(ce.DocumentoFiscal) && ce.DocumentoFiscal.Trim().Equals(documento, StringComparison.OrdinalIgnoreCase)));
                        if (match != null)
                        {
                            c.Cliente = match.Nombre;
                        }
                    }
                }
            }

            // Enriquecer Destinos
            var paisesExternos = await maestrosService.GetPaisesAsync();
            if (paisesExternos != null)
            {
                foreach (var d in model.Cabecera.Destinos)
                {
                    var match = paisesExternos.FirstOrDefault(pe => pe.Id == d.DestinoId);
                    if (match != null)
                    {
                        d.Destino = match.Pais;
                    }
                }
            }

            // Enriquecer Variedades
            var variedadesExternas = await maestrosService.GetVariedadesAsync(_currentUser.IdEmpresa!);
            if (variedadesExternas != null)
            {
                foreach (var detail in model.Detalle)
                {
                    detail.Variedad ??= [];

                    foreach (var v in detail.Variedad)
                    {
                        var match = variedadesExternas.FirstOrDefault(ve => ve.IdVariedad == v.VariedadId && ve.IdCultivo == v.CodigoCultivo);
                        if (match != null)
                        {
                            v.Variedad = match.Variedad;
                        }
                    }
                }
            }

            // Enriquecer Razon Social
            var empresasExternas = await maestrosService.GetEmpresasAsync();
            if (empresasExternas != null)
            {
                var match = empresasExternas.FirstOrDefault(e => e.Ruc == model.Cabecera.Ruc);
                if (match != null)
                {
                    model.Cabecera.RazonSocial = match.RazonSocial;
                }
            }

            // Enriquecer Planta de Empaque
            var acopiosExternos = await maestrosService.GetAcopiosAsync(_currentUser.IdEmpresa!);
            if (acopiosExternos != null)
            {
                var match = acopiosExternos.FirstOrDefault(a => a.codigo_acopio == model.Cabecera.PlantaDeEmpaque);
                if (match != null)
                {
                    model.Cabecera.PlantaDeEmpaque = match.Acopio;
                }
            }

            var pdfBytes = await pdfService.GenerarFichaComposicionPaletAsync(model, env.WebRootPath);
            return File(pdfBytes, "application/pdf", $"FichaComposicion_{idPalet}.pdf");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al generar PDF de ficha de composición");
            return StatusCode(500, new { message = "Error interno al generar el PDF.", error = ex.Message });
        }
    }

    public sealed class SincronizarPaletsRequest
    {
        public JsonElement? Palets { get; set; }
    }

    public sealed class SincronizarDPaletsRequest
    {
        public JsonElement? DPalets { get; set; }
    }

    [HttpPost("sincronizar-dpalet")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarDPalets([FromBody] SincronizarDPaletsRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }

            var jsonDPalets = ControllerJsonHelper.ExtractJson(request?.DPalets);
            var result = await procesosUseCase.SincronizarDPaletsAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                jsonDPalets
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarDPalets");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-dpalets-por-acopio")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetDPaletsPorAcopio()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            // if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            // {
            //     return BadRequest("CodigoAcopio is required");
            // }

            var result = await procesosUseCase.ListarDPaletsPorAcopioAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                _currentUser.CodigoAcopio ?? string.Empty
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetDPaletsPorAcopio");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-dpalets-por-palet")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetDPaletsPorPalet([FromQuery] string idPalet)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(idPalet))
            {
                return BadRequest("idPalet is required");
            }

            var result = await procesosUseCase.ListarDPaletsPorPaletAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idPalet
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetDPaletsPorPalet");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpPost("sincronizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarPalets([FromBody] SincronizarPaletsRequest request)
    {
        try
        {   
            if (string.IsNullOrEmpty(_currentUser.Role))
            {
                return BadRequest("IdRol is required");
            } 
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            var jsonPalets = ControllerJsonHelper.ExtractJson(request?.Palets);
            var result = await procesosUseCase.SincronizarPaletsAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonPalets
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarPalets");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }


    [HttpGet("get-codigos-rancho-por-lugar-produccion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetCodigosRanchoPorLugarProduccion([FromQuery] string idproyecto, [FromQuery] int idLugaresDeProduccion)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(idproyecto))
                return BadRequest("idproyecto is required");

            if (idLugaresDeProduccion <= 0)
                return BadRequest("idLugaresDeProduccion must be greater than 0");

            var result = await procesosUseCase.ListarCodigosRanchoPorLugarProduccionAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idproyecto,
                idLugaresDeProduccion
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetCodigosRanchoPorLugarProduccion");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tipos-clamshell-por-matriz")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposClamshellPorMatriz([FromQuery] string codigoCultivo, [FromQuery] string documentoConsignatario, [FromQuery] string destinoId, [FromQuery] int formatoId, [FromQuery] int tipoEmpaqueGuiaId, [FromQuery] int? tipoCajaId = null, [FromQuery] int? presentacionId = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            if (string.IsNullOrWhiteSpace(destinoId))
                return BadRequest("destinoId is required");

            if (formatoId <= 0)
                return BadRequest("formatoId must be greater than 0");

            if (tipoEmpaqueGuiaId <= 0)
                return BadRequest("tipoEmpaqueGuiaId must be greater than 0");

            var result = await procesosUseCase.ListarTiposClamshellPorMatrizAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                codigoCultivo,
                documentoConsignatario,
                destinoId,
                formatoId,
                tipoEmpaqueGuiaId,
                tipoCajaId,
                presentacionId
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposClamshellPorMatriz");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tipos-caja-por-matriz")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposCajaPorMatriz([FromQuery] string codigoCultivo, [FromQuery] string documentoConsignatario, [FromQuery] string destinoId, [FromQuery] int formatoId, [FromQuery] int tipoEmpaqueGuiaId, [FromQuery] int? presentacionId = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            if (string.IsNullOrWhiteSpace(destinoId))
                return BadRequest("destinoId is required");

            if (formatoId <= 0)
                return BadRequest("formatoId must be greater than 0");

            if (tipoEmpaqueGuiaId <= 0)
                return BadRequest("tipoEmpaqueGuiaId must be greater than 0");

            var result = await procesosUseCase.ListarTiposCajaPorMatrizAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                codigoCultivo,
                documentoConsignatario,
                destinoId,
                formatoId,
                tipoEmpaqueGuiaId,
                presentacionId
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposCajaPorMatriz");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-presentaciones-por-matriz")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPresentacionesPorMatriz([FromQuery] string codigoCultivo, [FromQuery] string documentoConsignatario, [FromQuery] string destinoId, [FromQuery] int formatoId, [FromQuery] int tipoEmpaqueGuiaId)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            if (string.IsNullOrWhiteSpace(destinoId))
                return BadRequest("destinoId is required");

            if (formatoId <= 0)
                return BadRequest("formatoId must be greater than 0");

            if (tipoEmpaqueGuiaId <= 0)
                return BadRequest("tipoEmpaqueGuiaId must be greater than 0");

            var result = await procesosUseCase.ListarPresentacionesPorMatrizAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                codigoCultivo,
                documentoConsignatario,
                destinoId,
                formatoId,
                tipoEmpaqueGuiaId
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetPresentacionesPorMatriz");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-tipos-empaque-guia-por-matriz")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTiposEmpaqueGuiaPorMatriz([FromQuery] string codigoCultivo, [FromQuery] string documentoConsignatario, [FromQuery] string destinoId, [FromQuery] int formatoId)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            if (string.IsNullOrWhiteSpace(destinoId))
                return BadRequest("destinoId is required");

            if (formatoId <= 0)
                return BadRequest("formatoId must be greater than 0");

            var result = await procesosUseCase.ListarTiposEmpaqueGuiaPorMatrizAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                codigoCultivo,
                documentoConsignatario,
                destinoId,
                formatoId
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTiposEmpaqueGuiaPorMatriz");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-formatos-por-matriz")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetFormatosPorMatriz([FromQuery] string codigoCultivo, [FromQuery] string documentoConsignatario, [FromQuery] string destinoId)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(codigoCultivo))
                return BadRequest("codigoCultivo is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            if (string.IsNullOrWhiteSpace(destinoId))
                return BadRequest("destinoId is required");

            var result = await procesosUseCase.ListarFormatosPorMatrizAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                codigoCultivo,
                documentoConsignatario,
                destinoId
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetFormatosPorMatriz");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-destinos-por-matriz-compatibilidad")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetDestinosPorMatrizCompatibilidad([FromQuery] string idproyecto, [FromQuery] string documentoConsignatario)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            if (string.IsNullOrWhiteSpace(idproyecto))
                return BadRequest("idproyecto is required");

            if (string.IsNullOrWhiteSpace(documentoConsignatario))
                return BadRequest("documentoConsignatario is required");

            var result = await procesosUseCase.ListarDestinosPorMatrizCompatibilidadAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idproyecto,
                documentoConsignatario
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetDestinosPorMatrizCompatibilidad");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("getTipoProcesoEmpacado-Acopio")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetTipoProcesoEmpacadoPorAcopio([FromQuery] string idproyecto)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            Console.WriteLine("idproyecto============================================: " + idproyecto);
            if (string.IsNullOrWhiteSpace(idproyecto))
                return BadRequest("idproyecto is required");

            var result = await procesosUseCase.ListarTipoProcesoEmpacadoPorAcopioAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idproyecto,
                _currentUser.CodigoAcopio!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetTipoProcesoEmpacadoPorAcopio");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("get-palets-por-proceso")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetPaletsForProceso([FromQuery] string json)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }

            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var idproceso = root.TryGetProperty("idproceso", out var procesoProp)
                ? procesoProp.GetString()
                : null;


            var result = await procesosUseCase.ListarPaletsForProcesoAsync(_currentUser.IdEmpresa!, _currentUser.Ruc!, idproceso!);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetProcesos");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

    [HttpGet("reporte-diario")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ObtenerReporteDiario([FromQuery] string fecha, [FromQuery] string? acopios = null, [FromQuery] string? idCampana = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
            {
                return BadRequest("UserName is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Role))
            {
                return BadRequest("IdRol is required");
            }
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
            {
                return BadRequest("IdEmpresa is required");
            }
            if (string.IsNullOrEmpty(_currentUser.Ruc))
            {
                return BadRequest("Ruc is required");
            }
            if (string.IsNullOrWhiteSpace(fecha))
            {
                return BadRequest("Fecha is required");
            }
            if (string.IsNullOrWhiteSpace(idCampana))
            {
                return BadRequest("IdCampana is required");
            }

            var result = await procesosUseCase.ObtenerReporteDiarioAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                fecha,
                acopios ?? string.Empty,
                idCampana
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Login no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ObtenerReporteDiario");
            return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
        }
    }

}
