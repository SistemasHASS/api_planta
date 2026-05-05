using Microsoft.AspNetCore.Mvc;
using api_planta.Domain.UseCase;
using api_planta.Api.Utils;
using System.Text.Json;

namespace api_planta.Api.Controllers
{
    [Route("api/catalogos")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly IPaletUseCase _useCase;
        private readonly ICatalogosUsaCase _catalogosUseCase;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogosController(IPaletUseCase useCase, ICatalogosUsaCase catalogosUseCase, ILogger<CatalogosController> logger)
        {
            _useCase = useCase;
            _catalogosUseCase = catalogosUseCase;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los catálogos de una vez
        /// </summary>
        [HttpPost("listar")]
        public async Task<IActionResult> ListarCatalogos([FromBody] JsonElement? body = null)
        {
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/listar] JSON: {Json}", json);
            try
            {
                var resultado = await _catalogosUseCase.ObtenerCatalogosAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "listar");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/destinos] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerDestinosPorConsignatarioAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "destinos");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/formatos] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerFormatosAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "formatos");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/tipos-empaque-guia] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerTiposEmpaqueGuiaAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "tipos-empaque-guia");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/calibres] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCalibresAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "calibres");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/presentaciones] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerPresentacionesAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "presentaciones");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/tipos-desde-matriz] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerTiposDesdeMatrizAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "tipos-desde-matriz");
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
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "codigos-rancho");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/codigos-rancho] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCodigosRanchoAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "codigos-rancho");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/verificar-driscoll] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.VerificarDriscollAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "verificar-driscoll");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/variedades] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerVariedadesPorConsignatarioAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "variedades");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/config-tipo-proceso] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerConfigTipoProcesoAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "config-tipo-proceso");
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
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/campania-activa] JSON: {Json}", json);
            try
            {
                var resultado = await _useCase.ObtenerCampaniaActivaAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "campania-activa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/campania-activa] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener campaña activa", details = ex.Message });
            }
        }
    }

    [Route("api/catalogos/operarios")]
    [ApiController]
    public class CatalogoOperariosController : ControllerBase
    {
        private readonly IPaletUseCase _useCase;
        private readonly ICatalogosUsaCase _catalogosUseCase;
        private readonly ILogger<CatalogosController> _logger;

        public CatalogoOperariosController(IPaletUseCase useCase, ICatalogosUsaCase catalogosUseCase, ILogger<CatalogosController> logger)
        {
            _useCase = useCase;
            _catalogosUseCase = catalogosUseCase;
            _logger = logger;
        }

        [HttpPost("listar")]
        public async Task<IActionResult> ListarCatalogos([FromBody] JsonElement? body = null)
        {
            var json = ControllerJsonHelper.ExtractJson(body);
            _logger.LogInformation("[Catalogos/listar] JSON: {Json}", json);
            try
            {
                var resultado = await _catalogosUseCase.ObtenerCatalogosOperariosAsync(json);
                return ControllerJsonHelper.UnwrapSpResult(this, resultado, _logger, "listar");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Catalogos/listar] Error");
                return StatusCode(500, new { success = false, message = "Error al obtener catálogos", details = ex.Message });
            }
        }

    }
}
