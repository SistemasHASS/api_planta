

using System.Text.Json;
using Planta.Application.Administracion.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class AdministracionServiceImpl(IAdministracionRepository administracionRepository) : IAdministracionService
{
    public Task<List<JsonElement>> SincronizarReglasSobrepesoAsync(string json, string idempresa, string ruc, string usuario)
        => administracionRepository.SincronizarReglasSobrepesoAsync(json, idempresa, ruc, usuario);
        
    public Task<List<JsonElement>> ListarReglasSobrepesoAsync(string idempresa, string ruc, string? idProyecto = null, string? idCultivo = null)
        => administracionRepository.ListarReglasSobrepesoAsync(idempresa, ruc, idProyecto, idCultivo);
        
    public Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json, string idempresa, string ruc, string usuario)
        => administracionRepository.SincronizarMatrizCompatibilidadAsync(json, idempresa, ruc, usuario);
        
    public Task<List<JsonElement>> SincronizarUsuariosAsync(string json, string idempresa, string ruc, string usuario)
        => administracionRepository.SincronizarUsuariosAsync(json, idempresa, ruc, usuario);


    public Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string? idCultivo = null)
        => administracionRepository.ListarMatricesCompatibilidadAsync(idempresa,ruc, idProyecto, idCultivo);

    public Task<List<JsonElement>> ListarCorrelativosDocumentosAsync(string idempresa, string ruc)
        => administracionRepository.ListarCorrelativosDocumentosAsync(idempresa, ruc);

    public Task<List<JsonElement>> SincronizarCorrelativosDocumentosAsync(string json, string idempresa, string ruc, string usuario)
        => administracionRepository.SincronizarCorrelativosDocumentosAsync(json, idempresa, ruc, usuario);
}