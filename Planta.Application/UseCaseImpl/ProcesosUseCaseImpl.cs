using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token, valida lo que llega y delega al servicio. Las reglas de negocio las aplica el SP.</summary>
public sealed class ProcesosUseCaseImpl(IProcesosService procesos, ICurrentUser currentUser) : IProcesosUseCase
{
    public Task<JsonElement> ListarAsync(ListarProcesosRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoCultivo, "codigoCultivo");

        var contexto = ContextoDeUsuario.Crear(currentUser);
        var esAdmin = string.Equals(contexto.Rol, Roles.Admin, StringComparison.OrdinalIgnoreCase);
        return procesos.ListarAsync(contexto, request, esAdmin, cancellationToken);
    }

    public Task<JsonElement> BuscarAsync(BuscarProcesoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoCultivo, "codigoCultivo");
        Requerir(request.Turno, "turno");
        Requerir(request.Fecha, "fecha");
        return procesos.BuscarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarSupervisoresDisponiblesAsync(PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Fecha, "fecha");
        return procesos.ListarSupervisoresDisponiblesAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarPersonalLogisticoDisponibleAsync(PersonalDisponibleRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Fecha, "fecha");
        return procesos.ListarPersonalLogisticoDisponibleAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarTipoProcesoEmpacadoPorAcopioAsync(ProyectoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        // Es una consulta del acopio del usuario: sin acopio en el token no hay nada que listar.
        return procesos.ListarTipoProcesoEmpacadoPorAcopioAsync(ContextoDeUsuario.Crear(currentUser, exigirAcopio: true), request.IdProyecto, cancellationToken);
    }

    public Task<JsonElement> SincronizarAsync(SincronizarProcesoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoCultivo, "codigoCultivo");
        return procesos.SincronizarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    private static void Requerir(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new BadRequestException($"El campo '{campo}' es requerido.");
        }
    }
}
