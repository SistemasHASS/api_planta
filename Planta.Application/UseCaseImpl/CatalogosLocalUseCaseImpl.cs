using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Constants;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma empresa y RUC del token, valida el filtro que llega y delega al servicio.</summary>
public sealed class CatalogosLocalUseCaseImpl(ICatalogosLocalService catalogos, ICurrentUser currentUser) : ICatalogosLocalUseCase
{
    public Task<JsonElement> ListarGrupoClienteAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarGrupoClienteAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, string idProyecto, CancellationToken cancellationToken = default)
        => catalogos.ListarPorProyectoAsync(catalogo, ContextoDeUsuario.Crear(currentUser), Requerir(idProyecto, "idProyecto"), cancellationToken);

    public Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, string codigoCultivo, CancellationToken cancellationToken = default)
        => catalogos.ListarPorCultivoAsync(catalogo, ContextoDeUsuario.Crear(currentUser), Requerir(codigoCultivo, "codigoCultivo"), cancellationToken);

    public Task<JsonElement> ListarDestinatariosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarDestinatariosAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarConsignatariosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarConsignatariosAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarVariedadesAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarVariedadesAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarAcopiosAsync(string idProyecto, CancellationToken cancellationToken = default)
        => catalogos.ListarAcopiosAsync(ContextoDeUsuario.Crear(currentUser), Requerir(idProyecto, "idProyecto"), cancellationToken);

    public Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarMotivosTrasladoAsync(cancellationToken);

    public Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarUnidadesMedidaAsync(cancellationToken);

    public Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarTiposLocacionAsync(cancellationToken);

    public Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarPuertosAsync(cancellationToken);

    public Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarAeroPuertosAsync(cancellationToken);

    public Task<JsonElement> ObtenerEstablecimientoEmisorAsync(CancellationToken cancellationToken = default)
        => catalogos.ObtenerEstablecimientoEmisorAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarEstablecimientosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarEstablecimientosAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default)
        => catalogos.ListarDepartamentosAsync(cancellationToken);

    public Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default)
        => catalogos.ListarProvinciasAsync(Requerir(codigoDepartamento, "codigoDepartamento"), cancellationToken);

    public Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default)
        => catalogos.ListarDistritosAsync(Requerir(codigoDepartamento, "codigoDepartamento"), Requerir(codigoProvincia, "codigoProvincia"), cancellationToken);

    private static string Requerir(string? valor, string campo)
        => string.IsNullOrWhiteSpace(valor) ? throw new BadRequestException($"El campo '{campo}' es requerido.") : valor.Trim();
}
