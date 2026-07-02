
using System.Text.Json;
using Planta.Application.GuiaRemision.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class GuiasRemisionServiceImpl(IGuiasRemisionRepository guiasRemisionRepository) : IGuiasRemisionService
{
    public Task<List<JsonElement>> SincronizarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json)
        => guiasRemisionRepository.SincronizarGuiasRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);

    public Task<List<JsonElement>> ListarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string? estado,string? fechaDesde,string? fechaHasta,string? texto)
        => guiasRemisionRepository.ListarGuiasRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);

    public Task<List<JsonElement>> GetGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision)
        => guiasRemisionRepository.GetGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision);

    public Task<List<JsonElement>> EliminarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario,string idRol)
        => guiasRemisionRepository.EliminarGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, idRol);

    public Task<List<JsonElement>> EmitirGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario)
        => guiasRemisionRepository.EmitirGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);

    public Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string estado,string codigoEstadoSunat,string estadoSunat,string? pdfFileUrl,string? xmlFileSignUrl,string? xmlFileSunatUrl,string usuario)
        => guiasRemisionRepository.ActualizarEstadoSunatGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, estado, codigoEstadoSunat, estadoSunat, pdfFileUrl, xmlFileSignUrl, xmlFileSunatUrl, usuario);

    public Task<List<JsonElement>> AnularGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario)
        => guiasRemisionRepository.AnularGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);

    public Task<List<JsonElement>> EditarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json)
        => guiasRemisionRepository.EditarGuiaRemisionAsync(idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);

    public Task<List<JsonElement>> ListarCodigosCajaAsync(string idempresa,string ruc)
        => guiasRemisionRepository.ListarCodigosCajaAsync(idempresa, ruc);
}
