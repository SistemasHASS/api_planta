using System.Text.Json;

namespace api_planta.Domain.Repository
{
    public interface IAdministracionRepository
    {
        Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string json);
        Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json);
        Task<List<JsonElement>> ListarUsuariosAsync(string json);
        Task<List<JsonElement>> SincronizarUsuariosAsync(string userId,string json);
        Task<List<JsonElement>> ResetearPasswordUsuarioAsync(int id, string usuario, string passwordHasheado);
    }
}
