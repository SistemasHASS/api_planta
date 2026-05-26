

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Application.Proceso;

public sealed class ProcesosUseCase(IProcesosService procesosService) : IProcesosUseCase
{

    public async Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    {
        return await procesosService.ListarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
    }

}