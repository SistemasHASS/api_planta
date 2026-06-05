
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
        string json,
        string jsonDetalle)
    {
        return await guiasRemisionService.SincronizarGuiasRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json, jsonDetalle);
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
}
