

using System.Text.Json;

namespace Planta.Application.Administracion.Abstractions;

public interface IAdministracionService
{
    Task<List<JsonElement>> SincronizarReglasSobrepesoAsync(string json, string idempresa, string ruc, string usuario);
    Task<List<JsonElement>> ListarReglasSobrepesoAsync(string idempresa, string ruc, string? idProyecto = null, string? idCultivo = null);
    Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json, string idempresa, string ruc, string usuario);
    Task<List<JsonElement>> SincronizarUsuariosAsync(string json,string idempresa, string ruc, string usuario);
    Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string? idCultivo = null);
    Task<List<JsonElement>> ListarCorrelativosDocumentosAsync(string idempresa, string ruc);
    Task<List<JsonElement>> SincronizarCorrelativosDocumentosAsync(string json, string idempresa, string ruc, string usuario);
}