

using System.Text.Json;

namespace Planta.Application.Proceso.Abstractions;

public interface IProcesosService
{
    // Task<List<JsonElement>> SincronizarProcesospesoAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId);
    Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId);
}