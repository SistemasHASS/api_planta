using api_planta.Domain.Services;
using api_planta.Domain.UseCase;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace api_planta.Application.Usecase
{
    public class AdministracionUseCaseImpl : IAdministracionUseCase
    {
        private readonly IAdministracionService _service;
        private readonly IMaestrosAuthService _maestrosAuthService;

        public AdministracionUseCaseImpl(IAdministracionService service, IMaestrosAuthService maestrosAuthService)
        {
            _service = service;
            _maestrosAuthService = maestrosAuthService;
        }

        public async Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string json)
        {
            return await _service.ListarMatricesCompatibilidadAsync(json);
        }

        public async Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json)
        {
            return await _service.SincronizarMatrizCompatibilidadAsync(json);
        }

        public async Task<List<JsonElement>> ListarUsuariosAsync(string usuario, string ruc)
        {   

            var listaUsuarios = await _maestrosAuthService.ObtenerListaUsuariosAsync(usuario, ruc, "PLANTA");

            if(listaUsuarios.Error)
            {   
                return new List<JsonElement> { JsonDocument.Parse(JsonSerializer.Serialize(listaUsuarios)).RootElement };
            }  
            return await _service.ListarUsuariosAsync(JsonSerializer.Serialize(listaUsuarios.Data));
        }

        public async Task<List<JsonElement>> SincronizarUsuariosAsync(string userId,string json)
        {
            var jsonConPasswordsHasheadas = HashearPasswordsUsuariosNuevos(json);
            return await _service.SincronizarUsuariosAsync(userId,jsonConPasswordsHasheadas);
        }

        public async Task<List<JsonElement>> ResetearPasswordUsuarioAsync(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return RespuestaError("Payload vacío.");

            JsonNode? root;
            try
            {
                root = JsonNode.Parse(json);
            }
            catch
            {
                return RespuestaError("Payload inválido.");
            }

            if (root is not JsonObject obj)
                return RespuestaError("Se esperaba un objeto JSON.");

            var id = obj["id"]?.GetValue<int?>();
            var usuario = obj["usuario"]?.GetValue<string?>();
            var password = obj["password"]?.GetValue<string?>();

            if (!id.HasValue || id.Value <= 0)
                return RespuestaError("El campo 'id' es requerido.");
            if (string.IsNullOrWhiteSpace(usuario))
                return RespuestaError("El campo 'usuario' es requerido.");
            if (string.IsNullOrWhiteSpace(password))
                return RespuestaError("El campo 'password' es requerido.");

            var passwordHasheado = BCrypt.Net.BCrypt.HashPassword(password);
            return await _service.ResetearPasswordUsuarioAsync(id.Value, usuario, passwordHasheado);
        }

        private static List<JsonElement> RespuestaError(string mensaje)
        {
            var payload = new { error = true, data = (object?)null, mensaje };
            var payloadJson = JsonSerializer.Serialize(payload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(payloadJson) };
        }

        private static string HashearPasswordsUsuariosNuevos(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return "{}";

            JsonNode? root;
            try
            {
                root = JsonNode.Parse(json);
            }
            catch
            {
                return json;
            }

            if (root is not JsonArray arr)
                return json;

            foreach (var node in arr)
            {
                if (node is not JsonObject obj)
                    continue;

                var modo = obj["modo"]?.GetValue<string?>();
                if (!string.Equals(modo, "nuevo", StringComparison.OrdinalIgnoreCase))
                    continue;

                var password = obj["password"]?.GetValue<string?>();
                if (string.IsNullOrWhiteSpace(password))
                    continue;

                obj["password"] = BCrypt.Net.BCrypt.HashPassword(password);
            }

            return root.ToJsonString();
        }
    }
}
