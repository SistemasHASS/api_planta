using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token, valida lo que llega y delega al servicio. Las reglas de negocio las aplica el SP.</summary>
public sealed class GuiasUseCaseImpl(IGuiasService guias, ICurrentUser currentUser) : IGuiasUseCase
{
    public Task<JsonElement> SincronizarAsync(SincronizarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Guias, "guias");
        return guias.SincronizarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ListarAsync(ListarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return guias.ListarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<ArchivoGenerado> ExportarExcelAsync(ExportarGuiasRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return guias.ExportarExcelAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ObtenerAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guias.ObtenerAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> EliminarAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guias.EliminarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> EmitirAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guias.EmitirAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ConsultarEstadoSunatAsync(GuiaRequest request, CancellationToken cancellationToken = default)
    {
        RequerirGuia(request);
        return guias.ConsultarEstadoSunatAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> AnularAsync(AnularGuiaRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodigoGuiaRemision, "codigoGuiaRemision");
        return guias.AnularAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> EditarAsync(EditarGuiaRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.Guias, "guias");
        return guias.EditarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);
    }

    public Task<JsonElement> ListarCodigosCajaAsync(CancellationToken cancellationToken = default)
        => guias.ListarCodigosCajaAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarProcesosConPaletsCerradosAsync(ProcesosGuiaRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return guias.ListarProcesosConPaletsCerradosAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
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
