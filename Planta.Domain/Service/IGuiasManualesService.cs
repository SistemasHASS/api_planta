using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IGuiasManualesService
{
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, SincronizarGuiasRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EditarAsync(ContextoUsuario contexto, EditarGuiaRequest request, CancellationToken cancellationToken = default);
    Task<JsonElement> EliminarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);

    /// <summary>Cierra la guia con el SP y la envia a SUNAT.</summary>
    Task<JsonElement> EmitirAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);

    /// <summary>Reenvia a SUNAT una guia CERRADA que aun no fue enviada.</summary>
    Task<JsonElement> ReenviarAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);

    Task<JsonElement> ConsultarEstadoSunatAsync(ContextoUsuario contexto, GuiaRequest request, CancellationToken cancellationToken = default);
    /// <summary>El acopio se envia porque el administrador no tiene acopio en su token; si viene vacio se usa el del token.</summary>
    Task<JsonElement> AnularAsync(ContextoUsuario contexto, AnularGuiaRequest request, CancellationToken cancellationToken = default);
    Task<ArchivoGenerado> ExportarExcelAsync(ContextoUsuario contexto, ExportarGuiasRequest request, CancellationToken cancellationToken = default);
}
