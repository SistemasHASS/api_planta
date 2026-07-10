using System.Text.Json;

namespace Planta.Application.GuiaRemisionManual.Abstractions;

public interface IGuiasRemisionManualService
{
    Task<List<JsonElement>> SincronizarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json);
    Task<List<JsonElement>> ListarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string? estado, string? fechaDesde, string? fechaHasta, string? texto);
    Task<List<JsonElement>> GetGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string idRol, string codigoGuiaRemision);
    Task<List<JsonElement>> EditarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json);
    Task<List<JsonElement>> EliminarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario, string idRol);
    Task<List<JsonElement>> EmitirGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario);
    Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string? codigoEstadoSunat, string? estadoSunat, string? pdfFileUrl, string? xmlFileSignUrl, string? xmlFileSunatUrl, bool? enviadoBizlinks, string? respuestaBizlinks, string usuario);
    Task<List<JsonElement>> AnularGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario);
}
