using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api_planta.Infraestructure.Controller
{
    [Route("mantenedoresplanta")]
    [ApiController]
    public class MantenedoresController : ControllerBase
    {
        private readonly IMantenedoresUseCase mantenedoresUseCase;

        public MantenedoresController(IMantenedoresUseCase mantenedoresUseCase)
        {
            this.mantenedoresUseCase = mantenedoresUseCase;
        }

        [HttpPost("lineas/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarLineasProduccion([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.mantenedoresUseCase.ListarLineasProduccionAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("lineas/crud-linea-produccion")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> CrudLineaProduccion([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.mantenedoresUseCase.CrudLineaProduccionAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("configuracion-lineas/sincronizar")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> SincronizarConfiguracionLineas([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.mantenedoresUseCase.SincronizarConfiguracionLineasAsync(json);
            return Ok(resultado.FirstOrDefault());
        }

        [HttpPost("configuracion-lineas/listado")]
        [ProducesResponseType(typeof(JsonElement), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<dynamic>> ListarConfiguracionLineasProduccion([FromBody] JsonElement? body = null)
        {
            string json = body.HasValue && body.Value.ValueKind != JsonValueKind.Null ? body.Value.ToString() : "[]";
            var resultado = await this.mantenedoresUseCase.ListarConfiguracionLineasProduccionAsync(json);
            return Ok(resultado.FirstOrDefault());
        }
    }
}
