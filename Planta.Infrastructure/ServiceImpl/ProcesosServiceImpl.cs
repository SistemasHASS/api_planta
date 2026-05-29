

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Infrastructure.ServiceImpl; 

public sealed class ProcesosServiceImpl (IProcesosRepository procesosRepository): IProcesosService
{
  
  public Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso)
    => procesosRepository.ListarPaletsForProcesoAsync(idempresa, ruc, idproceso);
  
  public Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    => procesosRepository.SincronizarPaletsAsync(idempresa, ruc, usuario, idRol, json);
  
  public Task<List<JsonElement>> SincronizarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string usuario, string idRol, string jsonProceso, string jsonDprocesoLogisticos, string jsonDprocesoSupervisores, string? modo)
    => procesosRepository.SincronizarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, usuario, idRol, jsonProceso, jsonDprocesoLogisticos, jsonDprocesoSupervisores, modo);
  
  public Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    => procesosRepository.ListarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
  
  public Task<List<JsonElement>> ListarProcesosTodosAsync(string idempresa, string ruc, string idProyecto, string idCultivo)
    => procesosRepository.ListarProcesosTodosAsync(idempresa, ruc, idProyecto, idCultivo);
    
  public Task<List<JsonElement>> GetSupervisoresDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    => procesosRepository.GetSupervisoresDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    
  public Task<List<JsonElement>> GetPersonalLogisticaDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha)
    => procesosRepository.GetPersonalLogisticaDisponiblesAsync(idempresa, ruc, idProyecto, fecha);
    
  public Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    => procesosRepository.BuscarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, turno, fecha);
}