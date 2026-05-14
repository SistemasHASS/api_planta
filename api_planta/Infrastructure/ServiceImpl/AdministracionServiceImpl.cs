using api_planta.Domain.Repository;
using api_planta.Domain.Services;
using System.Text.Json;

namespace api_planta.Infrastructure.ServiceImpl
{
    public class AdministracionServiceImpl : IAdministracionService
    {
        private readonly IAdministracionRepository _repository;

        public AdministracionServiceImpl(IAdministracionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string json)
        {
            return await _repository.ListarMatricesCompatibilidadAsync(json);
        }

        public async Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json)
        {
            return await _repository.SincronizarMatrizCompatibilidadAsync(json);
        }

        public async Task<List<JsonElement>> ListarUsuariosAsync(string json)
        {
            return await _repository.ListarUsuariosAsync(json);
        }

        public async Task<List<JsonElement>> SincronizarUsuariosAsync(string userId, string json)
        {
            return await _repository.SincronizarUsuariosAsync(userId, json);
        }

        public async Task<List<JsonElement>> ResetearPasswordUsuarioAsync(int id, string usuario, string passwordHasheado)
        {
            return await _repository.ResetearPasswordUsuarioAsync(id, usuario, passwordHasheado);
        }
    }
}
