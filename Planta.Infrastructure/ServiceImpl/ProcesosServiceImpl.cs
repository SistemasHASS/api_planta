

using System.Text.Json;
using Planta.Application.Proceso.Abstractions;

namespace Planta.Infrastructure.ServiceImpl; 

public sealed class ProcesosServiceImpl (IProcesosRepository procesosRepository): IProcesosService
{
  // public Task<List<JsonElement>> SincronizarProcesospesoAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
  //   => procesosRepository.SincronizarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
    
  public Task<List<JsonElement>> ListarProcesosAsync(string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId)
    => procesosRepository.ListarProcesosAsync(idempresa, ruc, idProyecto, idCultivo, acopioId);
}