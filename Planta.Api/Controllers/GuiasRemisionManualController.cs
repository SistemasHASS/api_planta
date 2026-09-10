using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Planta.Api.Middlewares;
using Planta.Api.Security;
using Planta.Application.GuiaRemisionManual.Abstractions;

namespace Planta.Api.Controllers;

[Route("api/guias-remision-manual")]
[ApiController]
public sealed class GuiasRemisionManualController(
    ILogger<GuiasRemisionManualController> logger,
    ICurrentUserContext _currentUser,
    IGuiasRemisionManualUseCase guiasRemisionManualUseCase) : ControllerBase
{
    public class SincronizarGuiasRemisionManualRequest
    {
        public string? IdProyecto { get; set; }
        public JsonElement? Guias { get; set; }
    }

    [HttpPost("sincronizar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> SincronizarGuiasRemisionManual([FromBody] SincronizarGuiasRemisionManualRequest request)
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(request?.IdProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var jsonGuias = ControllerJsonHelper.ExtractJson(request?.Guias);

            var result = await guiasRemisionManualUseCase.SincronizarGuiasRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                request!.IdProyecto!,
                _currentUser.CodigoAcopio!,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonGuias
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en SincronizarGuiasRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("listar-guias-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ListarGuiasRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string? estado = null,
        [FromQuery] string? fechaDesde = null,
        [FromQuery] string? fechaHasta = null,
        [FromQuery] string? texto = null)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }

            var result = await guiasRemisionManualUseCase.ListarGuiasRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.UserName!,
                _currentUser.Role!,
                estado,
                fechaDesde,
                fechaHasta,
                texto
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ListarGuiasRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("exportar-guias-remision-manual-excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ExportarGuiasRemisionManualExcel(
        [FromQuery] string idProyecto,
        [FromQuery] string? estado = null,
        [FromQuery] string? fechaDesde = null,
        [FromQuery] string? fechaHasta = null,
        [FromQuery] string? texto = null,
        [FromQuery] string? codigoCultivo = null)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentUser.UserName))
                return BadRequest("UserName is required");
            if (string.IsNullOrEmpty(_currentUser.Role))
                return BadRequest("IdRol is required");
            if (string.IsNullOrEmpty(_currentUser.IdEmpresa))
                return BadRequest("IdEmpresa is required");
            if (string.IsNullOrEmpty(_currentUser.Ruc))
                return BadRequest("Ruc is required");
            if (string.IsNullOrEmpty(idProyecto))
                return BadRequest("IdProyecto is required");

            var esAdmin = _currentUser.Role.Equals("ADPLA", StringComparison.OrdinalIgnoreCase)
                       || _currentUser.Role.Equals("ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)
                       || _currentUser.Role.Equals("MOPLA", StringComparison.OrdinalIgnoreCase)
                       || _currentUser.Role.Equals("MONITOR", StringComparison.OrdinalIgnoreCase);

            if (!esAdmin)
                return StatusCode(403, new { error = true, mensaje = "Solo el rol administrador puede exportar a Excel." });

            var result = await guiasRemisionManualUseCase.ListarGuiasRemisionManualExcelAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.UserName!,
                _currentUser.Role!,
                estado,
                fechaDesde,
                fechaHasta,
                texto,
                codigoCultivo);

            if (result.Count == 0)
                return StatusCode(500, new { error = true, mensaje = "No se obtuvo respuesta del SP." });

            var wrapper = result[0];
            if (wrapper.GetProperty("error").GetBoolean())
            {
                var mensaje = wrapper.TryGetProperty("mensaje", out var message)
                    ? message.GetString()
                    : "Error al listar guías manuales para Excel.";
                return BadRequest(new { error = true, mensaje });
            }

            if (!wrapper.TryGetProperty("data", out var dataElement)
                || dataElement.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
                return BadRequest(new { error = true, mensaje = "No se encontraron datos para exportar." });

            var data = dataElement.Deserialize<List<Dictionary<string, object>>>();
            if (data == null || data.Count == 0)
                return BadRequest(new { error = true, mensaje = "No se encontraron guías manuales para exportar." });

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Guías Manuales");
            var columns = data[0].Keys.ToList();

            for (var i = 0; i < columns.Count; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = columns[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(31, 78, 121));
                cell.Style.Font.Color.SetColor(System.Drawing.Color.White);
                cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }

            for (var row = 0; row < data.Count; row++)
            {
                for (var col = 0; col < columns.Count; col++)
                {
                    var key = columns[col];
                    var value = data[row].TryGetValue(key, out var item) ? item : null;
                    worksheet.Cells[row + 2, col + 1].Value = value?.ToString() ?? string.Empty;
                }
            }

            worksheet.Cells[1, 1, data.Count + 1, columns.Count].AutoFilter = true;
            worksheet.Cells[1, 1, data.Count + 1, columns.Count].AutoFitColumns();

            var fileBytes = await package.GetAsByteArrayAsync();
            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "guias-remision-manuales.xlsx");
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ExportarGuiasRemisionManualExcel");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("get-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> GetGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.GetGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.Role!,
                codigoGuiaRemision
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en GetGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpPost("editar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EditarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromBody] object json)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (json == null)
            {
                return BadRequest("Json is required");
            }

            var jsonString = System.Text.Json.JsonSerializer.Serialize(json);

            var result = await guiasRemisionManualUseCase.EditarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                _currentUser.UserName!,
                _currentUser.Role!,
                jsonString
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en EditarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("eliminar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EliminarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.EliminarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio ?? string.Empty,
                codigoGuiaRemision,
                _currentUser.UserName!,
                _currentUser.Role!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en EliminarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("emitir-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> EmitirGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.EmitirGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en EmitirGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("reenviar-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ReenviarGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.ReenviarGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ReenviarGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("consultar-estado-sunat-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> ConsultarEstadoSunatGuiaRemisionManual(
        [FromQuery] string idProyecto,
        [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.ConsultarEstadoSunatGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en ConsultarEstadoSunatGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }

    [HttpGet("anular-guia-remision-manual")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> AnularGuiaRemisionManual([FromQuery] string idProyecto, [FromQuery] string codigoGuiaRemision)
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
            if (string.IsNullOrEmpty(_currentUser.CodigoAcopio))
            {
                return BadRequest("CodigoAcopio is required");
            }
            if (string.IsNullOrEmpty(idProyecto))
            {
                return BadRequest("IdProyecto is required");
            }
            if (string.IsNullOrEmpty(codigoGuiaRemision))
            {
                return BadRequest("CodigoGuiaRemision is required");
            }

            var result = await guiasRemisionManualUseCase.AnularGuiaRemisionManualAsync(
                _currentUser.IdEmpresa!,
                _currentUser.Ruc!,
                idProyecto,
                _currentUser.CodigoAcopio!,
                codigoGuiaRemision,
                _currentUser.UserName!
            );

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Acceso no autorizado para usuario {Usuario}", _currentUser.UserName);
            return Unauthorized(new { error = true, mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error interno en AnularGuiaRemisionManual");
            return StatusCode(500, new { error = true, mensaje = ex.Message });
        }
    }
}
