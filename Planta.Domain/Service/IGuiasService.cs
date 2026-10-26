using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IGuiasService
{
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarGuiasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default);

    /// <summary>Detalle de una guia con los nombres de catalogo completados (apimaestra y catalogos propios).</summary>
    Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> EliminarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EditarAsync(ContextoUsuario contexto, EditarGuiaRequest request, CancellationToken cancellationToken = default);

    /// <summary>Emite la guia ante SUNAT: SP de emision, envio a ApiDocumentosElectronicos y actualizacion del estado SUNAT.</summary>
    Task<JsonElement> EmitirAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> ConsultarEstadoSunatAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> AnularAsync(ContextoUsuario contexto, AnularGuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarCodigosCajaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarProcesosConPaletsCerradosAsync(ContextoUsuario contexto, ProcesosGuiaRequest request, CancellationToken cancellationToken = default);

    Task<ArchivoGenerado> ExportarExcelAsync(ContextoUsuario contexto, ExportarGuiasRequest request, CancellationToken cancellationToken = default);
}
