using Microsoft.AspNetCore.Mvc;
using api_planta.Domain.UseCase;
using System.Text.Json;

namespace api_planta.Controllers
{
    [Route("api/catalogos")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly IPaletUseCase _useCase;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogosController(IPaletUseCase useCase, ILogger<CatalogosController> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        /// <summary>
        /// Helper: los SPs retornan 1 fila con columna JsonData (string JSON).
        /// BaseRepository la parsea a JsonElement. Esta función desempaqueta:
        ///   - Si es un array JSON → lo retorna tal cual
        ///   - Si es un string JSON (ej. "[{...}]") → lo parsea
        ///   - Si tiene propiedad JsonData → extrae y parsea
        ///   - Si es un objeto → lo retorna como objeto único
        /// </summary>
        private IActionResult UnwrapSpResult(List<JsonElement> resultado, string endpoint)
        {
            if (resultado == null || !resultado.Any())
                return Ok(new object[0]);

            var first = resultado.First();

            // Caso 1: El SP retornó un array JSON directamente
            if (first.ValueKind == JsonValueKind.Array)
                return Ok(first);

            // Caso 2: El SP retornó un string (ej. el JSON como texto plano)
            if (first.ValueKind == JsonValueKind.String)
            {
                var jsonStr = first.GetString();
                if (string.IsNullOrEmpty(jsonStr) || jsonStr == "null")
                    return Ok(new object[0]);
                try
                {
                    var parsed = JsonSerializer.Deserialize<JsonElement>(jsonStr);
                    return Ok(parsed);
                }
                catch
                {
                    _logger.LogWarning("[{Endpoint}] Could not parse string result as JSON", endpoint);
                    return Ok(new object[0]);
                }
            }

            // Caso 3: Objeto con propiedad JsonData
            if (first.ValueKind == JsonValueKind.Object && first.TryGetProperty("JsonData", out var jsonData))
            {
                if (jsonData.ValueKind == JsonValueKind.Null)
                    return Ok(new object[0]);
                if (jsonData.ValueKind == JsonValueKind.String)
                {
                    var str = jsonData.GetString();
                    if (string.IsNullOrEmpty(str) || str == "null")
                        return Ok(new object[0]);
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<JsonElement>(str);
                        return Ok(parsed);
                    }
                    catch
                    {
                        _logger.LogWarning("[{Endpoint}] Could not parse JsonData string", endpoint);
                        return Ok(new object[0]);
                    }
                }
                return Ok(jsonData);
            }

            // Caso 4: Objeto sin JsonData → retornar tal cual (ej. resultado de verificarDriscoll)
            return Ok(first);
        }

        /// <summary>
        /// Helper: Extrae el JSON string del body del request
        /// </summary>
        private static string ExtractJson(JsonElement? body)
        {
            return body.HasValue && body.Value.ValueKind != JsonValueKind.Null
                ? body.Value.ToString()
                : "{}";
        }

        /// <summary>
        /// Obtener todos los catálogos de una vez
        /// </summary>
        [HttpPost("listar")]
        public async Task<IActionResult> ListarCatalogos([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/listar] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCatalogosAsync(json);
                return UnwrapSpResult(resultado, "listar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/listar] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener catálogos", details = ex.Message });
            }
        }

        /// <summary>
        /// Destinos filtrados por consignatarioId (MatrizCompatibilidad)
        /// </summary>
        [HttpPost("destinos")]
        public async Task<IActionResult> ListarDestinos([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/destinos] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerDestinosPorConsignatarioAsync(json);
                return UnwrapSpResult(resultado, "destinos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/destinos] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener destinos", details = ex.Message });
            }
        }

        /// <summary>
        /// Formatos filtrados por consignatarioId + destinoId (MatrizCompatibilidad)
        /// </summary>
        [HttpPost("formatos")]
        public async Task<IActionResult> ListarFormatos([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/formatos] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerFormatosAsync(json);
                return UnwrapSpResult(resultado, "formatos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/formatos] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener formatos", details = ex.Message });
            }
        }

        /// <summary>
        /// Tipos empaque guía filtrados por consignatarioId + destinoId + formatoId
        /// </summary>
        [HttpPost("tipos-empaque-guia")]
        public async Task<IActionResult> ListarTiposEmpaqueGuia([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/tipos-empaque-guia] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerTiposEmpaqueGuiaAsync(json);
                return UnwrapSpResult(resultado, "tipos-empaque-guia");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/tipos-empaque-guia] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener tipos empaque guía", details = ex.Message });
            }
        }

        /// <summary>
        /// Calibres filtrados por consignatarioId + destinoId + formatoId + tipoEmpaqueGuiaId
        /// </summary>
        [HttpPost("calibres")]
        public async Task<IActionResult> ListarCalibres([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/calibres] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCalibresAsync(json);
                return UnwrapSpResult(resultado, "calibres");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/calibres] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener calibres", details = ex.Message });
            }
        }

        /// <summary>
        /// Presentaciones filtradas (MatrizCompatibilidad)
        /// </summary>
        [HttpPost("presentaciones")]
        public async Task<IActionResult> ListarPresentaciones([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/presentaciones] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerPresentacionesAsync(json);
                return UnwrapSpResult(resultado, "presentaciones");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/presentaciones] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener presentaciones", details = ex.Message });
            }
        }

        /// <summary>
        /// Tipos desde matriz (tipoCaja, tipoClamshell, calibre, tipoEmpaque)
        /// </summary>
        [HttpPost("tipos-desde-matriz")]
        public async Task<IActionResult> ListarTiposDesdeMatriz([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/tipos-desde-matriz] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerTiposDesdeMatrizAsync(json);
                return UnwrapSpResult(resultado, "tipos-desde-matriz");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/tipos-desde-matriz] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener tipos desde matriz", details = ex.Message });
            }
        }

        /// <summary>
        /// Códigos rancho filtrados por lugarProduccionId + consignatarioId (GET - como el sistema original)
        /// </summary>
        [HttpGet("codigos-rancho")]
        public async Task<IActionResult> ListarCodigosRanchoGet([FromQuery] int lugarProduccionId, [FromQuery] int? consignatarioId = null)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new { lugarProduccionId, consignatarioId });
            _logger.LogInformation("[Catalogos/codigos-rancho GET] lugarProduccionId: {LugarProduccionId}, consignatarioId: {ConsignatarioId}", lugarProduccionId, consignatarioId);
            try
            {
                var resultado = await _useCase.ObtenerCodigosRanchoAsync(json);
                return UnwrapSpResult(resultado, "codigos-rancho");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/codigos-rancho GET] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener códigos rancho", details = ex.Message });
            }
        }

        /// <summary>
        /// Códigos rancho filtrados por lugarProduccionId + consignatarioId
        /// </summary>
        [HttpPost("codigos-rancho")]
        public async Task<IActionResult> ListarCodigosRancho([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/codigos-rancho] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCodigosRanchoAsync(json);
                return UnwrapSpResult(resultado, "codigos-rancho");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/codigos-rancho] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener códigos rancho", details = ex.Message });
            }
        }

        /// <summary>
        /// Verificar si un consignatario es Driscoll
        /// </summary>
        [HttpPost("verificar-driscoll")]
        public async Task<IActionResult> VerificarDriscoll([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/verificar-driscoll] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.VerificarDriscollAsync(json);
                return UnwrapSpResult(resultado, "verificar-driscoll");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/verificar-driscoll] Error");
                return StatusCode(500, new { success = false, message = "Error al verificar driscoll", details = ex.Message });
            }
        }

        /// <summary>
        /// Variedades filtradas por consignatarioId
        /// </summary>
        [HttpPost("variedades")]
        public async Task<IActionResult> ListarVariedades([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/variedades] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerVariedadesPorConsignatarioAsync(json);
                return UnwrapSpResult(resultado, "variedades");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/variedades] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener variedades", details = ex.Message });
            }
        }

        /// <summary>
        /// Configuración de tipo proceso empacado por acopio
        /// </summary>
        [HttpPost("config-tipo-proceso")]
        public async Task<IActionResult> ObtenerConfigTipoProceso([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/config-tipo-proceso] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerConfigTipoProcesoAsync(json);
                return UnwrapSpResult(resultado, "config-tipo-proceso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/config-tipo-proceso] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener config tipo proceso", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtener la campaña activa actual
        /// </summary>
        [HttpPost("campania-activa")]
        public async Task<IActionResult> ObtenerCampaniaActiva([FromBody] JsonElement? body = null)
        {
            var json = ExtractJson(body);
            _logger.LogInformation("[Catalogos/campania-activa] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCampaniaActivaAsync(json);
                return UnwrapSpResult(resultado, "campania-activa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/campania-activa] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener campaña activa", details = ex.Message });
            }
        }
    }
}
