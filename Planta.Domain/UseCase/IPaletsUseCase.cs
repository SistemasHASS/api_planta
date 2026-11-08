using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Palets y sus detalles. Empresa, RUC, usuario, rol y acopio salen del token.</summary>
public interface IPaletsUseCase
{
    Task<JsonElement> ListarPorProcesoAsync(PaletsPorProcesoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Cajas disponibles del proceso (stock por formato y tipo de empaque). El SP devuelve activo = false si la empresa no maneja el flujo de empaque.</summary>
    Task<JsonElement> ListarStockCajasPorProcesoAsync(PaletsPorProcesoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Crea, edita o cierra palets (el SP lee el JSON y valida segun el rol).</summary>
    Task<JsonElement> SincronizarAsync(SincronizarPaletsRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> SincronizarDPaletsAsync(SincronizarDPaletsRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorAcopioAsync(CancellationToken cancellationToken = default);
    Task<JsonElement> ListarDPaletsPorPaletAsync(PaletRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCodigosRanchoPorLugarAsync(CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default);
    Task<ArchivoGenerado> GenerarFichaComposicionAsync(PaletRequest request, string? webRootPath, CancellationToken cancellationToken = default);

    Task<JsonElement> ListarDestinosPorMatrizAsync(DestinosPorMatrizRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarFormatosPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPresentacionesPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposCajaPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTiposClamshellPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default);
}
