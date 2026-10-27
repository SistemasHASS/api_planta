using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token y delega al servicio. Las reglas de negocio las aplica el SP.</summary>
public sealed class ReportesUseCaseImpl(IReportesService reportes, ICurrentUser currentUser) : IReportesUseCase
{
    public Task<JsonElement> ReporteDiarioAsync(ReporteDiarioRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Fecha, "fecha");
        Requerir(request.IdProyecto, "idProyecto");
        return reportes.ReporteDiarioAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ReporteSemanalFiltrosAsync(ProyectoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return reportes.ReporteSemanalFiltrosAsync(ContextoDeUsuario.Crear(currentUser), request.IdProyecto, cancellationToken);
    }

    public Task<JsonElement> ReporteSemanalAsync(ReporteSemanalRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return reportes.ReporteSemanalAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ReporteCampaniaAsync(ReporteSemanalRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return reportes.ReporteCampaniaAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public async Task<JsonElement> ReporteKgAcopioEmpresaExternaAsync(CancellationToken cancellationToken = default)
    {
        var ruc = await reportes.RucExternoKgAcopioAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);
        return JsonSerializer.SerializeToElement(new { rucExterno = ruc });
    }

    public Task<JsonElement> ReporteKgAcopioCampaniasAsync(RucRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        return reportes.CampaniasKgAcopioAsync(ContextoDeUsuario.Crear(currentUser), request.Ruc, cancellationToken);
    }

    public Task<JsonElement> ReporteKgAcopioAsync(ReporteKgAcopioRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Fecha, "fecha");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.CodFundo, "codFundo");
        var contexto = ContextoDeUsuario.Crear(currentUser);
        if (string.IsNullOrWhiteSpace(request.Ruc))
        {
            request.Ruc = contexto.Ruc;
        }

        return reportes.ReporteKgAcopioAsync(contexto, request, cancellationToken);
    }

    public async Task<JsonElement> ReporteBhHabilitadoAsync(CancellationToken cancellationToken = default)
    {
        var habilitado = await reportes.ReporteBhHabilitadoAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);
        return JsonSerializer.SerializeToElement(new { habilitado });
    }

    public Task<JsonElement> ReporteDiarioPropioExternoAsync(ReporteDiarioPropioExternoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Fecha, "fecha");
        Requerir(request.IdProyecto, "idProyecto");
        return reportes.ReporteDiarioPropioExternoAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ReporteAvanceCosechadorCampaniasAsync(RucRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        return reportes.CampaniasAvanceCosechadorAsync(ContextoDeUsuario.Crear(currentUser), request.Ruc, cancellationToken);
    }

    public Task<JsonElement> ReporteAvanceGrupoAsync(ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.FechaDesde, "fechaDesde");
        return reportes.ReporteAvanceGrupoAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<ArchivoGenerado> ExportarAvanceGrupoExcelAsync(ReporteAvanceGrupoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.FechaDesde, "fechaDesde");
        return reportes.ExportarAvanceGrupoExcelAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ReporteAvanceCosechadorFiltrosAsync(ReporteAvanceCosechadorFiltrosRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.FechaDesde, "fechaDesde");
        return reportes.ReporteAvanceCosechadorFiltrosAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<ArchivoGenerado> ExportarAvanceCosechadorExcelAsync(ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.FechaDesde, "fechaDesde");
        return reportes.ExportarAvanceCosechadorExcelAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ReporteAvanceCosechadorAsync(ReporteAvanceCosechadorRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Ruc, "ruc");
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.FechaDesde, "fechaDesde");
        return reportes.ReporteAvanceCosechadorAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    private static void Requerir(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new BadRequestException($"El campo '{campo}' es requerido.");
        }
    }
}
