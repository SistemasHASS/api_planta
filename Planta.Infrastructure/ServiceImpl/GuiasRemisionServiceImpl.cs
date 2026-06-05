
using System.Text.Json;
using Planta.Application.GuiaRemision.Abstractions;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class GuiasRemisionServiceImpl(IGuiasRemisionRepository guiasRemisionRepository) : IGuiasRemisionService
{
    public Task<List<JsonElement>> SincronizarGuiasRemisionAsync(
        string idempresa,
        string ruc,
        string idProyecto,
        string codigoAcopio,
        string usuario,
        string idRol,
        string json,
        string jsonDetalle)
        => guiasRemisionRepository.SincronizarGuiasRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, json, jsonDetalle);

    public Task<List<JsonElement>> ListarGuiasRemisionAsync(
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
        => guiasRemisionRepository.ListarGuiasRemisionAsync(
            idempresa, ruc, idProyecto, codigoAcopio, usuario, idRol, estado, fechaDesde, fechaHasta, texto);
}
