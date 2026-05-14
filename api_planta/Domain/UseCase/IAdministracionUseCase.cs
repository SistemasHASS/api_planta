using System.Text.Json;

namespace api_planta.Domain.UseCase
{
    public interface IAdministracionUseCase
    {
        Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string json);
        Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json);
        Task<List<JsonElement>> ListarUsuariosAsync(string ususario, string ruc);
        Task<List<JsonElement>> SincronizarUsuariosAsync(string userId,string json);
        Task<List<JsonElement>> ResetearPasswordUsuarioAsync(string json);
    }
}
