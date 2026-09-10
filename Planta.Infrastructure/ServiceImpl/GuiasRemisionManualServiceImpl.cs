using System.Text.Json;
using Planta.Application.GuiaRemisionManual.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class GuiasRemisionManualServiceImpl(IGuiasRemisionManualRepository guiasRemisionManualRepository) : IGuiasRemisionManualService
{
    public Task<List<JsonElement>> SincronizarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json)
        => guiasRemisionManualRepository.SincronizarGuiasRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);

    public Task<List<JsonElement>> ListarGuiasRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string? estado, string? fechaDesde, string? fechaHasta, string? texto)
        => guiasRemisionManualRepository.ListarGuiasRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);

    public Task<List<JsonElement>> ListarGuiasRemisionManualExcelAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string? estado, string? fechaDesde, string? fechaHasta, string? texto)
        => guiasRemisionManualRepository.ListarGuiasRemisionManualExcelAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);

    public Task<List<JsonElement>> GetGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string idRol, string codigoGuiaRemision)
        => guiasRemisionManualRepository.GetGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, idRol, codigoGuiaRemision);

    public Task<List<JsonElement>> EditarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string usuario, string idRol, string json)
        => guiasRemisionManualRepository.EditarGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);

    public Task<List<JsonElement>> EliminarGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario, string idRol)
        => guiasRemisionManualRepository.EliminarGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, idRol);

    public Task<List<JsonElement>> EmitirGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
        => guiasRemisionManualRepository.EmitirGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);

    public Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string? codigoEstadoSunat, string? estadoSunat, string? pdfFileUrl, string? xmlFileSignUrl, string? xmlFileSunatUrl, bool? enviadoBizlinks, string? respuestaBizlinks, string usuario)
        => guiasRemisionManualRepository.ActualizarEstadoSunatGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, codigoEstadoSunat, estadoSunat, pdfFileUrl, xmlFileSignUrl, xmlFileSunatUrl, enviadoBizlinks, respuestaBizlinks, usuario);

    public Task<List<JsonElement>> AnularGuiaRemisionManualAsync(string idempresa, string ruc, string idProyecto, string codigoAcopio, string codigoGuiaRemision, string usuario)
        => guiasRemisionManualRepository.AnularGuiaRemisionManualAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
}
