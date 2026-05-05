using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_planta.Infrastructure.Persistence;
using System.Data;
using System.Text.Json;

namespace api_planta.Api.Controllers
{
    [Route("api/guias")]
    [ApiController]
    public class GuiasController : ControllerBase
    {
        private readonly SistemaPaletsDbContext _context;
        private readonly ILogger<GuiasController> _logger;

        public GuiasController(SistemaPaletsDbContext context, ILogger<GuiasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private async Task<List<Dictionary<string, object?>>> EjecutarSPAsync(
            string spName, Dictionary<string, object?>? parametros = null)
        {
            var lista = new List<Dictionary<string, object?>>();
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parametros != null)
            {
                foreach (var kvp in parametros)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = kvp.Key;
                    param.Value = kvp.Value ?? DBNull.Value;
                    command.Parameters.Add(param);
                }
            }

            try
            {
                await _context.Database.OpenConnectionAsync();
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                    lista.Add(row);
                }
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }

            return lista;
        }

        [HttpGet("proceso/{procesoId}")]
        public async Task<IActionResult> ListarPorProceso(int procesoId)
        {
            try
            {
                var result = await EjecutarSPAsync("SP_Guias_ListarPorProceso",
                    new Dictionary<string, object?> { { "@ProcesoId", procesoId } });
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            try
            {
                var lista = new List<Dictionary<string, object?>>();
                var detalle = new List<Dictionary<string, object?>>();

                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SP_Guias_ObtenerPorId";
                command.CommandType = CommandType.StoredProcedure;
                var param = command.CreateParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                command.Parameters.Add(param);

                await _context.Database.OpenConnectionAsync();
                using var reader = await command.ExecuteReaderAsync();

                // First result set: guia
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object?>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    lista.Add(row);
                }

                // Second result set: detalle palets
                if (await reader.NextResultAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object?>();
                        for (int i = 0; i < reader.FieldCount; i++)
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        detalle.Add(row);
                    }
                }

                await _context.Database.CloseConnectionAsync();

                if (lista.Count == 0)
                    return NotFound(new { success = false, message = "Guia no encontrada." });

                var guia = lista[0];
                guia["detallePalets"] = detalle;

                return Ok(new { success = true, data = guia });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] JsonElement body)
        {
            try
            {
                var paletIds = body.GetProperty("paletIds").EnumerateArray()
                    .Select(x => x.GetInt32().ToString());
                var paletIdsCsv = string.Join(",", paletIds);

                var parametros = new Dictionary<string, object?>
                {
                    { "@ProcesoId", body.GetProperty("procesoId").GetInt32() },
                    { "@DestinatarioId", body.TryGetProperty("destinatarioId", out var d) && d.ValueKind != JsonValueKind.Null ? (object?)d.GetInt32() : null },
                    { "@TransportistaId", body.TryGetProperty("transportistaId", out var t) && t.ValueKind != JsonValueKind.Null ? (object?)t.GetInt32() : null },
                    { "@ConductorId", body.TryGetProperty("conductorId", out var c) && c.ValueKind != JsonValueKind.Null ? (object?)c.GetInt32() : null },
                    { "@VehiculoId", body.TryGetProperty("vehiculoId", out var v) && v.ValueKind != JsonValueKind.Null ? (object?)v.GetInt32() : null },
                    { "@Precinto", body.TryGetProperty("precinto", out var p) && p.ValueKind != JsonValueKind.Null ? (object?)p.GetString() : null },
                    { "@NumeroViaje", body.TryGetProperty("numeroViaje", out var nv) && nv.ValueKind != JsonValueKind.Null ? (object?)nv.GetInt32() : null },
                    { "@EsReposicion", body.TryGetProperty("esReposicion", out var rep) ? (object)(rep.GetBoolean() ? 1 : 0) : 0 },
                    { "@Observaciones", body.TryGetProperty("observaciones", out var obs) && obs.ValueKind != JsonValueKind.Null ? (object?)obs.GetString() : null },
                    { "@UsuarioId", body.GetProperty("usuarioId").GetInt32() },
                    { "@PaletIds", paletIdsCsv }
                };

                var result = await EjecutarSPAsync("SP_Guias_Crear", parametros);
                return Ok(new { success = true, data = result.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("cerrar")]
        public async Task<IActionResult> Cerrar([FromBody] JsonElement body)
        {
            try
            {
                var parametros = new Dictionary<string, object?>
                {
                    { "@Id", body.GetProperty("id").GetInt32() },
                    { "@Serie", body.TryGetProperty("serie", out var s) && s.ValueKind != JsonValueKind.Null ? (object?)s.GetString() : null },
                    { "@Numero", body.TryGetProperty("numero", out var n) && n.ValueKind != JsonValueKind.Null ? (object?)n.GetInt32() : null },
                    { "@FechaEmision", body.TryGetProperty("fechaEmision", out var fe) && fe.ValueKind != JsonValueKind.Null ? (object?)fe.GetString() : null }
                };

                var result = await EjecutarSPAsync("SP_Guias_Cerrar", parametros);
                return Ok(new { success = true, data = result.FirstOrDefault() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost("anular")]
        public async Task<IActionResult> Anular([FromBody] JsonElement body)
        {
            try
            {
                var parametros = new Dictionary<string, object?>
                {
                    { "@Id", body.GetProperty("id").GetInt32() }
                };

                await EjecutarSPAsync("SP_Guias_Anular", parametros);
                return Ok(new { success = true, message = "Guia anulada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await EjecutarSPAsync("SP_Guias_Eliminar",
                    new Dictionary<string, object?> { { "@Id", id } });
                return Ok(new { success = true, message = "Guia eliminada." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
