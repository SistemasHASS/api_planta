using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

public sealed class AdministracionUseCaseImpl(IAdministracionService administracion, ICurrentUser currentUser) : IAdministracionUseCase
{
    public Task<JsonElement> ListarMatricesCompatibilidadAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarMatricesCompatibilidadAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<JsonElement> ListarReglasSobrepesoAsync(ListarAdministracionRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarReglasSobrepesoAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<JsonElement> ListarCorrelativosDocumentosAsync(CancellationToken cancellationToken = default)
        => administracion.ListarCorrelativosDocumentosAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ObtenerParametroAsync(ParametroRequest request, CancellationToken cancellationToken = default)
        => administracion.ObtenerParametroAsync(ContextoDeUsuario.Crear(currentUser), request.IdParametro, cancellationToken);

    public Task<JsonElement> ListarCodigosCajaAsync(CancellationToken cancellationToken = default)
        => administracion.ListarCodigosCajaAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<ArchivoGenerado> ExportarCodigosCajaExcelAsync(ExportarCodigosCajaRequest request, CancellationToken cancellationToken = default)
        => administracion.ExportarCodigosCajaExcelAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<JsonElement> SincronizarAcopioConfigAsync(SincronizarAcopioConfigRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarAcopioConfigAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarUsuariosAcopioAsync(CancellationToken cancellationToken = default)
        => administracion.ListarUsuariosAcopioAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> SincronizarUsuariosAsync(SincronizarUsuariosRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarUsuariosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarDestinatariosAsync(SincronizarDestinatariosRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarDestinatariosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarConsignatariosAsync(SincronizarConsignatariosRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarConsignatariosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarVariedadesAsync(SincronizarVariedadesRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarVariedadesAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarCorrelativosDocumentosAsync(SincronizarCorrelativosRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarCorrelativosDocumentosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarCatalogoOperativoAsync(SincronizarCatalogoOperativoRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarCatalogoOperativoAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarReglasSobrepesoAsync(SincronizarReglasSobrepesoRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarReglasSobrepesoAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> SincronizarMatricesCompatibilidadAsync(SincronizarMatricesCompatibilidadRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarMatricesCompatibilidadAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarAcopioLineasAsync(ListarAcopioLineasRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarAcopioLineasAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<JsonElement> SincronizarAcopioLineasAsync(SincronizarAcopioLineasRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarAcopioLineasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarAcopioMaquinasAsync(ListarAcopioMaquinasRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarAcopioMaquinasAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<JsonElement> SincronizarAcopioMaquinasAsync(SincronizarAcopioMaquinasRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarAcopioMaquinasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);


    public Task<JsonElement> ListarKilosPagoAsync(CancellationToken cancellationToken = default)
        => administracion.ListarKilosPagoAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> SincronizarKilosPagoAsync(SincronizarKilosPagoRequest request, CancellationToken cancellationToken = default)
        => administracion.SincronizarKilosPagoAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarControlInternoAsync(ControlInternoRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarControlInternoAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);

    public Task<ArchivoGenerado> ExportarControlInternoExcelAsync(ControlInternoRequest request, CancellationToken cancellationToken = default)
        => administracion.ExportarControlInternoExcelAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarHistorialKilosAsync(HistorialKilosRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarHistorialKilosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);

    public Task<JsonElement> ListarHistorialKilosAdminAsync(HistorialKilosRequest request, CancellationToken cancellationToken = default)
        => administracion.ListarHistorialKilosAdminAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);
}
