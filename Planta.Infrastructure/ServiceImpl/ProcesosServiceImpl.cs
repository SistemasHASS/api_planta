using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class ProcesosServiceImpl(IProcesosRepository repository) : IProcesosService
{
    public Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarProcesosRequest request, bool verTodosLosAcopios, CancellationToken cancellationToken = default)
        => verTodosLosAcopios
            ? repository.ListarTodosAsync(contexto, request, cancellationToken)
            : repository.ListarAsync(contexto, request, cancellationToken);

    public Task<JsonElement> BuscarAsync(ContextoUsuario contexto, BuscarProcesoRequest request, CancellationToken cancellationToken = default)
        => repository.BuscarAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarSupervisoresDisponiblesAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
        => repository.ListarSupervisoresDisponiblesAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
        => repository.ListarPersonalLogisticoDisponibleAsync(contexto, request, cancellationToken);

    public Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => repository.ListarTipoProcesoEmpacadoPorAcopioAsync(contexto, idProyecto, cancellationToken);

    public Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarProcesoRequest request, CancellationToken cancellationToken = default)
        => repository.SincronizarAsync(contexto, request, cancellationToken);
}
