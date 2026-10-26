using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

/// <summary>
/// Acceso a apimaestra (API externa de maestros). El nombre de la aplicacion que se envia
/// sale de la configuracion (App:Name = PLANTA), no de quien llama.
/// </summary>
public interface IMaestrosService
{
    /// <summary>Valida usuario y clave en apimaestra. Devuelve el primer usuario o null si las credenciales no son validas.</summary>
    Task<UsuarioMaestrosDto?> ValidarUsuarioAsync(string usuario, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Consulta un catalogo de apimaestra y devuelve la lista (ya sin el envoltorio { data }), leida con los mismos
    /// modelos de la API anterior. La empresa y el RUC salen del token; <paramref name="usuario"/> solo lo usa Usuarios.
    /// <paramref name="formatoSp"/> = true devuelve el JSON como lo esperan los SP (nombres sin camelCase).
    /// </summary>
    Task<JsonElement> ConsultarAsync(MaestroExterno maestro, string idEmpresa, string ruc, string? usuario = null, bool formatoSp = false, CancellationToken cancellationToken = default);
}
