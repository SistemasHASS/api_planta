using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Procesos de empacado. Empresa, RUC, usuario, rol y acopio salen del token.</summary>
public interface IProcesosUseCase
{
    /// <summary>Procesos de la campania: los del acopio del usuario, o todos si es administrador.</summary>
    Task<JsonElement> ListarAsync(ListarProcesosRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> BuscarAsync(BuscarProcesoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarSupervisoresDisponiblesAsync(PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ProyectoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Crea, cierra, reabre o edita un proceso con su personal (segun request.Modo).</summary>
    Task<JsonElement> SincronizarAsync(SincronizarProcesoRequest request, CancellationToken cancellationToken = default);
}
