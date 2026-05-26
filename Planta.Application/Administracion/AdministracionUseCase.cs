
using System.Text.Json;
using Planta.Application.Administracion.Abstractions;

namespace Planta.Application.Administracion;

public sealed class AdministracionUseCase(IAdministracionService administracionService) : IAdministracionUseCase
{
    public Task<List<JsonElement>> SincronizarReglasSobrepesoAsync(string json, string idempresa, string ruc, string usuario)
    {
        return administracionService.SincronizarReglasSobrepesoAsync(json, idempresa, ruc, usuario);
    }
    
    public Task<List<JsonElement>> ListarReglasSobrepesoAsync(string idempresa, string ruc, string? idProyecto = null, string? idCultivo = null)
    {
        return administracionService.ListarReglasSobrepesoAsync(idempresa, ruc, idProyecto, idCultivo);
    }
    
    public Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json, string idempresa, string ruc, string usuario)
    {
        return administracionService.SincronizarMatrizCompatibilidadAsync(json, idempresa, ruc, usuario);
    }
    
    public Task<List<JsonElement>> SincronizarUsuariosAsync(string json, string idempresa, string ruc, string usuario)
    {
        return administracionService.SincronizarUsuariosAsync(json, idempresa, ruc, usuario);
    }
    
    public Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string? idCultivo = null)
    {
        return administracionService.ListarMatricesCompatibilidadAsync(idempresa,ruc, idProyecto, idCultivo);
    }
}
