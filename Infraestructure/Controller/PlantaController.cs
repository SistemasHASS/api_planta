using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api_planta.Infraestructure.Controller
{
    [Route("planta")]
    [ApiController]
    public class PlantaController : ControllerBase
    {
        private readonly IPlantaUseCase plantaUseCase;

        public PlantaController(IPlantaUseCase plantaUseCase)
        {
            this.plantaUseCase = plantaUseCase;
        }

        [HttpPost("clientes/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarClientes([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.plantaUseCase.ListarClientesAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("mercadodestino/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarMercadoDestino([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.plantaUseCase.ListarMercadoDestinoAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("formato/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarFormatos([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.plantaUseCase.ListarFormatosAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

    }
}
