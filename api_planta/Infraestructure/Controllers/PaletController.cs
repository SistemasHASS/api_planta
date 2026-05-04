using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api_planta.Infraestructure.Controllers
{
    [Route("api/palets")]
    [ApiController]
    public class PaletController : ControllerBase
    {
        private readonly IPaletUseCase _useCase;
        private readonly ILogger<PaletController> _logger;

        public PaletController(IPaletUseCase useCase, ILogger<PaletController> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        [HttpPost("crear")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> Crear([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[CrearPalet] Request body: {Json}", json);

                var resultado = await _useCase.CrearPaletAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CrearPalet] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al crear el palet" });
            }
        }

        [HttpPost("obtener-por-proceso")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ObtenerPorProceso([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[ObtenerPaletsPorProceso] Request body: {Json}", json);

                var resultado = await _useCase.ObtenerPaletsPorProcesoAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ObtenerPaletsPorProceso] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al obtener los palets" });
            }
        }

        [HttpPost("obtener-por-id")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ObtenerPorId([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[ObtenerPaletPorId] Request body: {Json}", json);

                var resultado = await _useCase.ObtenerPaletPorIdAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ObtenerPaletPorId] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al obtener el detalle del palet" });
            }
        }

        [HttpPost("agregar-cajas")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> AgregarCajas([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[AgregarCajas] Request body: {Json}", json);

                var resultado = await _useCase.AgregarCajasAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[AgregarCajas] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al agregar las cajas" });
            }
        }

        [HttpPost("cerrar-saldo")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> CerrarSaldo([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[CerrarPaletSaldo] Request body: {Json}", json);

                var resultado = await _useCase.CerrarPaletSaldoAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[CerrarPaletSaldo] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al cerrar el palet" });
            }
        }

        [HttpPost("reabrir")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> Reabrir([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[ReabrirPalet] Request body: {Json}", json);

                var resultado = await _useCase.ReabrirPaletAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReabrirPalet] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al reabrir el palet" });
            }
        }

        [HttpPost("eliminar")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> Eliminar([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[EliminarPalet] Request body: {Json}", json);

                var resultado = await _useCase.EliminarPaletAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EliminarPalet] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al eliminar el palet" });
            }
        }

        [HttpPost("editar-cajas")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> EditarCajas([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[EditarCajas] Request body: {Json}", json);

                var resultado = await _useCase.EditarCajasAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EditarCajas] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al editar las cajas" });
            }
        }

        [HttpPost("eliminar-composicion")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> EliminarComposicion([FromBody] JsonElement body)
        {
            try
            {
                string json = body.ToString();
                _logger.LogInformation("[EliminarComposicion] Request body: {Json}", json);

                var resultado = await _useCase.EliminarComposicionAsync(json);
                var firstElement = resultado?.FirstOrDefault();
                
                return Ok(firstElement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EliminarComposicion] Error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "Error al eliminar la composición" });
            }
        }
    }
}
