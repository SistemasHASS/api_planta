using System.Text.Json;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: BD/sp/PLANTA_GetUsuarioAcopio.sql. Devuelve el envelope { error, data, mensaje }.</summary>
public sealed class UsuarioAcopioRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IUsuarioAcopioRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<RespuestaSp<List<UsuarioDto>>> GetUsuarioAcopioAsync(string json, CancellationToken cancellationToken = default)
    {
        var resultado = await EjecutarListaAsync(
            "PLANTA_GetUsuarioAcopio",
            command => Agregar(command, "@json", json),
            record =>
            {
                var error = !record.IsDBNull(0) && Convert.ToBoolean(record.GetValue(0));
                var mensaje = record.IsDBNull(2) ? "" : Convert.ToString(record.GetValue(2)) ?? "";

                List<UsuarioDto>? data = null;
                if (!record.IsDBNull(1))
                {
                    var texto = Convert.ToString(record.GetValue(1));
                    if (!string.IsNullOrWhiteSpace(texto))
                    {
                        data = JsonSerializer.Deserialize<List<UsuarioDto>>(texto, JsonOptions);
                    }
                }

                return new RespuestaSp<List<UsuarioDto>> { Error = error, Data = data, Mensaje = mensaje };
            },
            cancellationToken);

        return resultado.FirstOrDefault()
               ?? new RespuestaSp<List<UsuarioDto>> { Error = true, Mensaje = "Sin resultados" };
    }
}
