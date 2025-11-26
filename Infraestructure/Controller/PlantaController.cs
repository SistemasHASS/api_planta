using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api_planta.Infraestructure.Controller
{
    [Route("transporte")]
    [ApiController]
    public class PlantaController : ControllerBase
    {
        private readonly IPlantaUseCase transporteUseCase;

        public PlantaController(IPlantaUseCase transporteUseCase)
        {
            this.transporteUseCase = transporteUseCase;
        }

        [HttpPost("vehiculos/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarVehiculos([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.ListarVehiculosAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("localidad/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarLocalidad([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.ListarLocalidadAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("conductor/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarConductores([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.ListarConductoresAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("viajes/guardar-viajes")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> GuardarViajes([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.GuardarViajesAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("viajes/reporte-viajes")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ReporteViajes([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.ReporteViajesAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("viajes/reporte-viajes-detallado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ReporteViajesDetallado([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.ReporteViajesDetalladoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("viajes/recuperar-viaje")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> RecuperarViaje([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.RecuperarViajeAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("viajes/recuperar-viaje-controlador")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> RecuperarViajeControlador([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.transporteUseCase.RecuperarViajeControladorAsync(json);
            return Ok(resultado.FirstOrDefault());
        }
    }
}
