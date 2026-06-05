

using System.Text.Json;

namespace Planta.Application.Proceso.Abstractions;

public interface IProcesosUseCase
{
    Task<List<JsonElement>> ListarPaletsForProcesoAsync(string idempresa, string ruc, string idproceso);
    Task<List<JsonElement>> SincronizarPaletsAsync(string idempresa, string ruc, string usuario, string idRol, string json);
    Task<List<JsonElement>> SincronizarDPaletsAsync(string idempresa, string ruc, string codigoAcopio, string usuario, string json);
    Task<List<JsonElement>> SincronizarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string usuario, string idRol, string jsonProceso, string jsonDprocesoLogisticos, string jsonDprocesoSupervisores, string modo);
    Task<List<JsonElement>> ListarProcesosAsync(string idRol,string idempresa, string ruc, string idProyecto, string idCultivo, string acopioId);
    Task<List<JsonElement>> GetSupervisoresDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha);
    Task<List<JsonElement>> GetPersonalLogisticaDisponiblesAsync(string idempresa, string ruc, string idProyecto, string fecha);
    Task<List<JsonElement>> ListarTipoProcesoEmpacadoPorAcopioAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio);
    Task<List<JsonElement>> ListarDestinosPorMatrizCompatibilidadAsync(string idempresa, string ruc, string idProyecto, string documentoConsignatario);
    Task<List<JsonElement>> ListarFormatosPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId);
    Task<List<JsonElement>> ListarTiposEmpaqueGuiaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId);
    Task<List<JsonElement>> ListarPresentacionesPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId);
    Task<List<JsonElement>> ListarTiposCajaPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId);
    Task<List<JsonElement>> ListarTiposClamshellPorMatrizAsync(string idempresa, string ruc, string codigoCultivo, string documentoConsignatario, string destinoId, int formatoId, int tipoEmpaqueGuiaId);
    Task<List<JsonElement>> ListarCodigosRanchoPorLugarProduccionAsync(string idempresa, string ruc, string idProyecto, int idLugaresDeProduccion);
    Task<List<JsonElement>> ListarDPaletsPorAcopioAsync(string idempresa, string ruc, string codigoAcopio);
    Task<List<JsonElement>> ListarProcesosAbiertosConPaletsCerradosAsync(string idempresa, string ruc, string codigoAcopio);
    Task<List<JsonElement>> BuscarProcesoAsync(string idempresa, string ruc, string idProyecto, string codigoCultivo, string codigoAcopio, string turno, string fecha);
}