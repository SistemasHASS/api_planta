
using System.Text.Json;

namespace Planta.Application.GuiaRemision.Abstractions;

public interface IGuiasRemisionUseCase
{
    Task<List<JsonElement>> SincronizarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json);

    Task<List<JsonElement>> ListarGuiasRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string? estado,string? fechaDesde,string? fechaHasta,string? texto);

    Task<List<JsonElement>> GetGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision);

    Task<List<JsonElement>> EliminarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario,string idRol);

    Task<List<JsonElement>> EmitirGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario);

    Task<List<JsonElement>> AnularGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string codigoGuiaRemision,string usuario);

    Task<List<JsonElement>> EditarGuiaRemisionAsync(string idempresa,string ruc,string idProyecto,string codigoAcopio,string usuario,string idRol,string json);

    Task<List<JsonElement>> ListarCodigosCajaAsync(string idempresa,string ruc);
}
