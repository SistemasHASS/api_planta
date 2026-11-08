using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token, valida lo que llega y delega al servicio. Las reglas de negocio las aplica el SP.</summary>
public sealed class PaletsUseCaseImpl(IPaletsService palets, ICurrentUser currentUser) : IPaletsUseCase
{
    public Task<JsonElement> ListarPorProcesoAsync(PaletsPorProcesoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProceso, "idProceso");
        return palets.ListarPorProcesoAsync(ContextoDeUsuario.Crear(currentUser), request.IdProceso, cancellationToken);
    }

    public Task<JsonElement> ListarStockCajasPorProcesoAsync(PaletsPorProcesoRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProceso, "idProceso");
        return palets.ListarStockCajasPorProcesoAsync(ContextoDeUsuario.Crear(currentUser, exigirAcopio: true), request.IdProceso, cancellationToken);
    }

    public Task<JsonElement> SincronizarAsync(SincronizarPaletsRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.Palets, "palets");
        return palets.SincronizarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true), request.Palets, cancellationToken);
    }

    public Task<JsonElement> SincronizarDPaletsAsync(SincronizarDPaletsRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.DPalets, "dPalets");
        return palets.SincronizarDPaletsAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request.DPalets, cancellationToken);
    }

    public Task<JsonElement> ListarDPaletsPorAcopioAsync(CancellationToken cancellationToken = default)
        => palets.ListarDPaletsPorAcopioAsync(ContextoDeUsuario.Crear(currentUser), cancellationToken);

    public Task<JsonElement> ListarDPaletsPorPaletAsync(PaletRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdPalet, "idPalet");
        return palets.ListarDPaletsPorPaletAsync(ContextoDeUsuario.Crear(currentUser), request.IdPalet, cancellationToken);
    }

    public Task<JsonElement> ListarCodigosRanchoPorLugarAsync(CodigosRanchoPorLugarRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        return palets.ListarCodigosRanchoPorLugarAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<ArchivoGenerado> GenerarFichaComposicionAsync(PaletRequest request, string? webRootPath, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdPalet, "idPalet");
        return palets.GenerarFichaComposicionAsync(ContextoDeUsuario.Crear(currentUser), request.IdPalet, webRootPath, cancellationToken);
    }

    public Task<JsonElement> ListarDestinosPorMatrizAsync(DestinosPorMatrizRequest request, CancellationToken cancellationToken = default)
    {
        Requerir(request.IdProyecto, "idProyecto");
        Requerir(request.DocumentoConsignatario, "documentoConsignatario");
        return palets.ListarDestinosPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarFormatosPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        return palets.ListarFormatosPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarTiposEmpaqueGuiaPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        RequerirNumero(request.FormatoId, "formatoId");
        return palets.ListarTiposEmpaqueGuiaPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarPresentacionesPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        RequerirNumero(request.FormatoId, "formatoId");
        RequerirNumero(request.TipoEmpaqueGuiaId, "tipoEmpaqueGuiaId");
        return palets.ListarPresentacionesPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarTiposCajaPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        RequerirNumero(request.FormatoId, "formatoId");
        RequerirNumero(request.TipoEmpaqueGuiaId, "tipoEmpaqueGuiaId");
        return palets.ListarTiposCajaPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ListarTiposClamshellPorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        RequerirNumero(request.FormatoId, "formatoId");
        RequerirNumero(request.TipoEmpaqueGuiaId, "tipoEmpaqueGuiaId");
        return palets.ListarTiposClamshellPorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    public Task<JsonElement> ObtenerCalibreEmpaquePorMatrizAsync(MatrizCompatibilidadRequest request, CancellationToken cancellationToken = default)
    {
        RequerirBase(request);
        RequerirNumero(request.FormatoId, "formatoId");
        RequerirNumero(request.TipoEmpaqueGuiaId, "tipoEmpaqueGuiaId");
        RequerirNumero(request.TipoCajaId, "tipoCajaId");
        RequerirNumero(request.TipoClamshellId, "tipoClamshellId");
        return palets.ObtenerCalibreEmpaquePorMatrizAsync(ContextoDeUsuario.Crear(currentUser), request, cancellationToken);
    }

    /// <summary>Cultivo, consignatario y destino: lo minimo para consultar la matriz.</summary>
    private static void RequerirBase(MatrizCompatibilidadRequest request)
    {
        Requerir(request.CodigoCultivo, "codigoCultivo");
        Requerir(request.DocumentoConsignatario, "documentoConsignatario");
        Requerir(request.DestinoId, "destinoId");
    }

    private static void RequerirNumero(int? valor, string campo)
    {
        if (valor is null)
        {
            throw new BadRequestException($"El campo '{campo}' es requerido.");
        }
    }

    private static void Requerir(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new BadRequestException($"El campo '{campo}' es requerido.");
        }
    }
}
