using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>
/// Procesos de empacado (SP PLANTA_*Proceso*). Cada metodo llama a un SP y devuelve su respuesta { error, data, mensaje }.
/// Las validaciones de negocio viven en el SP, que ya existia y no se modifica.
/// </summary>
public interface IProcesosRepository
{
    /// <summary>Procesos del acopio del usuario (PLANTA_ListarProcesos).</summary>
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarProcesosRequest request, CancellationToken cancellationToken = default);

    /// <summary>Procesos de todos los acopios (PLANTA_ListarProcesosTodos).</summary>
    Task<JsonElement> ListarTodosAsync(ContextoUsuario contexto, ListarProcesosRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> BuscarAsync(ContextoUsuario contexto, BuscarProcesoRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarSupervisoresDisponiblesAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(ContextoUsuario contexto, PersonalDisponibleRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarProcesoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Jornadas de un proceso con lo trabajado (PLANTA_ListarJornadasProceso).</summary>
    Task<JsonElement> ListarJornadasAsync(ContextoUsuario contexto, JornadasProcesoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reabre una jornada cerrada (PLANTA_ReabrirJornadaAnotador); <paramref name="json"/> es [{"id":N}].</summary>
    Task<JsonElement> ReabrirJornadaAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
}
