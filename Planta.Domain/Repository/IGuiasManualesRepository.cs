using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>SP de guias de remision manuales (base-datos/sp/PLANTA_*GuiaRemisionManual*.sql). Cada metodo llama a un SP y devuelve su sobre { error, data, mensaje }.</summary>
public interface IGuiasManualesRepository
{
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default);
    Task<JsonElement> ListarParaExcelAsync(ContextoUsuario contexto, ListarGuiasRequest filtros, CancellationToken cancellationToken = default);
    Task<JsonElement> ObtenerAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default);
    Task<JsonElement> EditarAsync(ContextoUsuario contexto, string idProyecto, string jsonGuias, CancellationToken cancellationToken = default);
    Task<JsonElement> EliminarAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default);
    Task<JsonElement> EmitirAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, CancellationToken cancellationToken = default);
    Task<JsonElement> ActualizarEstadoSunatAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoEstadoSunat, EnvioDocumentoElectronico? envio, bool enviadoBizlinks, string respuestaBizlinks, CancellationToken cancellationToken = default);
    Task<JsonElement> AnularAsync(ContextoUsuario contexto, string idProyecto, string codigoGuiaRemision, string codigoAcopio, CancellationToken cancellationToken = default);
}
