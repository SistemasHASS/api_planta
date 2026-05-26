

using System.Text.Json;

namespace Planta.Application.Proceso.Abstractions;

public interface IProcesosUseCase
{
    
    Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId);

}