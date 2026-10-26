using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Guias de remision manuales (se crean escribiendo todo el detalle, sin palets). Empresa, RUC, usuario, rol y acopio salen del token.</summary>
public interface IGuiasManualesUseCase
{
    Task<JsonElement> SincronizarAsync(SincronizarGuiasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAsync(ListarGuiasRequest request, CancellationToken cancellationToken = default);
    Task<ArchivoGenerado> ExportarExcelAsync(ExportarGuiasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerAsync(GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EditarAsync(EditarGuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EliminarAsync(GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EmitirAsync(GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ReenviarAsync(GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ConsultarEstadoSunatAsync(GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> AnularAsync(AnularGuiaRequest request, CancellationToken cancellationToken = default);
}
