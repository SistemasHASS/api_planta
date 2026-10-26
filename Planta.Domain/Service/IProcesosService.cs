using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IProcesosService
{
    /// <summary>Procesos de la campania. Si <paramref name="verTodosLosAcopios"/> es true trae los de todos los acopios (administrador).</summary>
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarProcesosRequest request, bool verTodosLosAcopios, CancellationToken cancellationToken = default);

    Task<JsonElement> BuscarAsync(ContextoUsuario contexto, BuscarProcesoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarSupervisoresDisponiblesAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarProcesoRequest request, CancellationToken cancellationToken = default);
}
