

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Application.Proceso;

public sealed class ProcesosUseCase(IProcesosService procesosService) : IProcesosUseCase
{
    
    public async Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso)
    {
        return await procesosService.ListarPaletsForProcesoAsync(idempresa, ruc, idproceso);
    }
    
    public async Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    {
        return await procesosService.SincronizarPaletsAsync(idempresa, ruc, usuario, idRol, json);
    }
    
    public async Task<List<JsonElement>> SincronizarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string usuario, string idRol, string jsonProceso, string jsonDprocesoLogisticos, string jsonDprocesoSupervisores,string modo)
    {
        return await procesosService.SincronizarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, usuario, idRol, jsonProceso, jsonDprocesoLogisticos, jsonDprocesoSupervisores,modo);
    }
    
    public async Task<List<JsonElement>> ListarProcesosAsync(string idRol,string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    {   
        if(idRol == "ADPLA")
        {
            return await procesosService.ListarProcesosTodosAsync(idempresa, ruc, idProyecto, idCultivo);
        }
        return await procesosService.ListarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
    }
    
    public async Task<List<JsonElement>> GetSupervisoresDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await procesosService.GetSupervisoresDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    }
    
    public async Task<List<JsonElement>> GetPersonalLogisticaDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    {
        return await procesosService.GetPersonalLogisticaDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    }
    
    public async Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    {
        return await procesosService.BuscarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, turno, fecha);
    }
}