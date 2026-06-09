
using System.Text.Json;
using Planta.Application.GuiaRemision.Abstractions;

namespace Planta.Application.GuiaRemision;

public sealed class GuiasRemisionUseCase(IGuiasRemisionService guiasRemisionService) : IGuiasRemisionUseCase
{
    public async Task<List<JsonElement>> SincronizarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json)
    {
        return await guiasRemisionService.SincronizarGuiasRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json);
    }

    public async Task<List<JsonElement>> ListarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string? estado,
        string? fechaDesde,
        string? fechaHasta,
        string? texto)
    {
        return await guiasRemisionService.ListarGuiasRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);
    }

    public async Task<List<JsonElement>> GetGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision)
    {
        return await guiasRemisionService.GetGuiaRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision);
    }

    public async Task<List<JsonElement>> EliminarGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario,
        string idRol)
    {
        return await guiasRemisionService.EliminarGuiaRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario, idRol);
    }

    public async Task<List<JsonElement>> EmitirGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        return await guiasRemisionService.EmitirGuiaRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
    }

    public async Task<List<JsonElement>> AnularGuiaRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string codigoGuiaRemision,
        string usuario)
    {
        return await guiasRemisionService.AnularGuiaRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, codigoGuiaRemision, usuario);
    }

    public async Task<List<JsonElement>> ListarCodigosCajaAsync(
        string idempresa,
        string ruc)
    {
        return await guiasRemisionService.ListarCodigosCajaAsync(idempresa, ruc);
    }
}
