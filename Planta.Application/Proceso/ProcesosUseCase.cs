

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
    
    public async Task<List<JsonElement>> SincronizarDPaletsAsync(string idempresa, string ruc, string codigoAcopio, string usuario, string json)
    {
        return await procesosService.SincronizarDPaletsAsync(idempresa, ruc, codigoAcopio, usuario, json);
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
    
    public async Task<List<JsonElement>> ListarTipoProcesoEmpacadoPorAcopioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio)
    {
        return await procesosService.ListarTipoProcesoEmpacadoPorAcopioAsync(idempresa, ruc, idProyecto, codigoAcopio);
    }
    
    public async Task<List<JsonElement>> ListarDestinosPorMatrizCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string documentoConsignatario)
    {
        return await procesosService.ListarDestinosPorMatrizCompatibilidadAsync(idempresa, ruc, idProyecto, documentoConsignatario);
    }
    
    public async Task<List<JsonElement>> ListarFormatosPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId)
    {
        return await procesosService.ListarFormatosPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId);
    }
    
    public async Task<List<JsonElement>> ListarTiposEmpaqueGuiaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId)
    {
        return await procesosService.ListarTiposEmpaqueGuiaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId);
    }
    
    public async Task<List<JsonElement>> ListarPresentacionesPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await procesosService.ListarPresentacionesPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
    }
    
    public async Task<List<JsonElement>> ListarTiposCajaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await procesosService.ListarTiposCajaPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
    }
    
    public async Task<List<JsonElement>> ListarTiposClamshellPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId)
    {
        return await procesosService.ListarTiposClamshellPorMatrizAsync(idempresa, ruc, codigoCultivo, documentoConsignatario, destinoId, formatoId, tipoEmpaqueGuiaId);
    }
    
    public async Task<List<JsonElement>> ListarCodigosRanchoPorLugarProduccionAsync(string idempresa, string ruc, string idProyecto, int idLugaresDeProduccion)
    {
        return await procesosService.ListarCodigosRanchoPorLugarProduccionAsync(idempresa, ruc, idProyecto, idLugaresDeProduccion);
    }
    
    public async Task<List<JsonElement>> ListarDPaletsPorAcopioAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await procesosService.ListarDPaletsPorAcopioAsync(idempresa, ruc, codigoAcopio);
    }
    
    public async Task<List<JsonElement>> ListarProcesosAbiertosConPaletsCerradosAsync(string idempresa, string ruc, string codigoAcopio)
    {
        return await procesosService.ListarProcesosAbiertosConPaletsCerradosAsync(idempresa, ruc, codigoAcopio);
    }
    
    public async Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha)
    {
        return await procesosService.BuscarProcesoAsync(idempresa, ruc, idProyecto, codigoCultivo, codigoAcopio, turno, fecha);
    }
}