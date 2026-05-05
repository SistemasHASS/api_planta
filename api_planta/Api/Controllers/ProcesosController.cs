using Microsoft.AspNetCore.Mvc;
using api_planta.Domain.UseCase;
using System.Text.Json;

namespace api_planta.Controllers
{
    [Route("api/procesos")]
    [ApiController]
    public class ProcesosController : ControllerBase
    {
        private readonly IPaletUseCase _useCase;
        private readonly ILogger<ProcesosController> _logger;

        public ProcesosController(IPaletUseCase useCase, ILogger<ProcesosController> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        [HttpPost("listar")]
        public async Task<IActionResult> ListarProcesos([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null
                ? body.Value.ToString()
                : "{}";

            _logger.LogInformation("[Procesos/listar] JSON: {Json}", json);
            var resultado = await _useCase.ListarProcesosAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("obtener")]
        public async Task<IActionResult> ObtenerProceso([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/obtener] JSON: {Json}", json);
            var resultado = await _useCase.ObtenerProcesoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearProceso([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/crear] JSON: {Json}", json);
            var resultado = await _useCase.CrearProcesoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("cerrar")]
        public async Task<IActionResult> CerrarProceso([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/cerrar] JSON: {Json}", json);
            var resultado = await _useCase.CerrarProcesoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("reabrir")]
        public async Task<IActionResult> ReabrirProceso([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/reabrir] JSON: {Json}", json);
            var resultado = await _useCase.ReabrirProcesoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }
        [HttpPost("listar-por-acopio")]
        public async Task<IActionResult> ListarPorAcopio([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/listar-por-acopio] JSON: {Json}", json);
            var resultado = await _useCase.ListarProcesosPorAcopioAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("personal-disponible")]
        public async Task<IActionResult> ObtenerPersonalDisponible([FromBody] JsonElement body)
        {
            string json = body.ToString();
            _logger.LogInformation("[Procesos/personal-disponible] JSON: {Json}", json);
            var resultado = await _useCase.ObtenerPersonalDisponibleAsync(json);
            return Ok(resultado.FirstOrDefault());
        }
    }
}
