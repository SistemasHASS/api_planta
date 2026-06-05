

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Infrastructure.ServiceImpl; 

public sealed class ProcesosServiceImpl (IProcesosRepository procesosRepository): IProcesosService
{
  
  public Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso)
    => procesosRepository.ListarPaletsForProcesoAsync(idempresa, ruc, idproceso);
  
  public Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    => procesosRepository.SincronizarPaletsAsync(idempresa, ruc, usuario, idRol, json);
  
  public Task<List<JsonElement>> SincronizarDPaletsAsync(string idempresa, string ruc, string codigoAcopio, string usuario, string json)
    => procesosRepository.SincronizarDPaletsAsync(idempresa, ruc, codigoAcopio, usuario, json);
  
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
    
  public Task<List<JsonElement>> ListarTipoProcesoEmpacadoPorAcopioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio)
    => procesosRepository.ListarTipoProcesoEmpacadoPorAcopioAsync(idempresa, ruc, idProyecto, codigoAcopio);
  
  public Task<List<JsonElement>> ListarDestinosPorMatrizCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string documentoConsignatario)
    => procesosRepository.ListarDestinosPorMatrizCompatibilidadAsync(idempresa, ruc, idProyecto, documentoConsignatario);
  
  public Task<List<JsonElement>> ListarFormatosPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId)
    => procesosRepository.ListarFormatosPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId);
  
  public Task<List<JsonElement>> ListarTiposEmpaqueGuiaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId)
    => procesosRepository.ListarTiposEmpaqueGuiaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId);
  
  public Task<List<JsonElement>> ListarPresentacionesPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    => procesosRepository.ListarPresentacionesPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
  
  public Task<List<JsonElement>> ListarTiposCajaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    => procesosRepository.ListarTiposCajaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
  
  public Task<List<JsonElement>> ListarTiposClamshellPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    => procesosRepository.ListarTiposClamshellPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
  
  public Task<List<JsonElement>> ListarCodigosRanchoPorLugarProduccionAsync(string idempresa, string ruc, string idProyecto, int idLugaresDeProduccion)
    => procesosRepository.ListarCodigosRanchoPorLugarProduccionAsync(idempresa, ruc, idProyecto, idLugaresDeProduccion);
  
  public Task<List<JsonElement>> ListarDPaletsPorAcopioAsync(string idempresa, string ruc, string codigoAcopio)
    => procesosRepository.ListarDPaletsPorAcopioAsync(idempresa, ruc, codigoAcopio);
  
  public Task<List<JsonElement>> ListarProcesosAbiertosConPaletsCerradosAsync(string idempresa, string ruc, string codigoAcopio)
    => procesosRepository.ListarProcesosAbiertosConPaletsCerradosAsync(idempresa, ruc, codigoAcopio);
  
  public Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    => procesosRepository.BuscarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, turno, fecha);
}