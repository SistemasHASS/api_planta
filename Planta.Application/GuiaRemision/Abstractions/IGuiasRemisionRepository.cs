
using System.Text.Json;

namespace Planta.Application.GuiaRemision.Abstractions;

public interface IGuiasRemisionRepository
{
    Task<List<JsonElement>> SincronizarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json);

    Task<List<JsonElement>> ListarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string? estado,
        string? fechaDesde,
        string? fechaHasta,
        string? texto);

    Task<List<JsonElement>> GetGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision);

    Task<List<JsonElement>> EliminarGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario,
        string idRol);

    Task<List<JsonElement>> EmitirGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario);

    Task<List<JsonElement>> ActualizarEstadoSunatGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string estado,
        string codigoEstadoSunat,
        string estadoSunat,
        string? pdfFileUrl,
        string? xmlFileSignUrl,
        string? xmlFileSunatUrl,
        string usuario);

    Task<List<JsonElement>> AnularGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario);

    Task<List<JsonElement>> EditarGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json);

    Task<List<JsonElement>> ListarCodigosCajaAsync(
        string idempresa,
        string ruc);
}
