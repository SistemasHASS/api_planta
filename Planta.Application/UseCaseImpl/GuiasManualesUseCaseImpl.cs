using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token, valida lo que llega y delega al servicio. Las reglas de negocio las aplica el SP.</summary>
public sealed class GuiasManualesUseCaseImpl(IGuiasManualesService guiasManuales, ICurrentUser currentUser) : IGuiasManualesUseCase
{
    public Task<JsonElement> SincronizarAsync(SincronizarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Guias, "guias");
        return guiasManuales.SincronizarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ListarAsync(ListarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return guiasManuales.ListarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<ArchivoGenerado> ExportarExcelAsync(ExportarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return guiasManuales.ExportarExcelAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ObtenerAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guiasManuales.ObtenerAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> EditarAsync(EditarGuiaRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Guias, "guias");
        return guiasManuales.EditarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> EliminarAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guiasManuales.EliminarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> EmitirAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guiasManuales.EmitirAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ReenviarAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guiasManuales.ReenviarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ConsultarEstadoSunatAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guiasManuales.ConsultarEstadoSunatAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> AnularAsync(AnularGuiaRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoGuiaRemision, "codigoGuiaRemision");
        return guiasManuales.AnularAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request, cancellationToken);
    }

    private static void RequerirGuia(GuiaRequest request)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoGuiaRemision, "codigoGuiaRemision");
    }

    private static void Requerir(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new BadRequestException($"El campo {campo} es obligatorio.");
        }
    }
}
