using System.Text.Json;
using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth.Models;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public sealed class AuthRepository : BaseRepository, IAuthRepository
{
    public AuthRepository(SistemaPaletsDbContext context) : base(context)
    {
    }

    public async Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json)
    {
        var resultado = await EjecutarStoredProcedureAsync(
            "PLANTA_GetUsuarioAcopio",
            new Dictionary<string, object?>
            {
                { "@json", json }
            },
            result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                var mensaje = result.IsDBNull(2)
                    ? ""
                    : Convert.ToString(result.GetValue(2)) ?? "";

                List<UsuarioAcopioDto>? data = null;

                if (!result.IsDBNull(1))
                {
                    var dataStr = Convert.ToString(result.GetValue(1));

                    if (!string.IsNullOrWhiteSpace(dataStr))
                    {
                        data = JsonSerializer.Deserialize<List<UsuarioAcopioDto>>(dataStr);
                    }
                }

                return new ApiResponse<List<UsuarioAcopioDto>>
                {
                    Error = error,
                    Data = data,
                    Mensaje = mensaje
                };
            });

        return resultado.FirstOrDefault()
               ?? new ApiResponse<List<UsuarioAcopioDto>>
               {
                   Error = true,
                   Mensaje = "Sin resultados"
               };
    }
}
