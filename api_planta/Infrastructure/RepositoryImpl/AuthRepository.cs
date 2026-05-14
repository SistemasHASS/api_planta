

using System.Text.Json;
using api_planta.Domain.DTOs.Auth;
using api_planta.Domain.Repository;
using api_planta.Infrastructure.Persistence;

namespace api_planta.Infrastructure.RepositoryImpl
{
    public class AuthRepository : BaseRepository, IAuthRepository
    {
        public AuthRepository(SistemaPaletsDbContext context) : base(context) { }

        public async Task<ApiResponse<List<UsuarioAcopioDto>>> GetUsuarioAcopioAsync(string json)
        {
          var resultado = await EjecutarStoredProcedureAsync(
        "SP_UsuarioAcopio",
        new Dictionary<string, object?>
        {
            { "@json", json }
        },
        result =>
        {
            var error = !result.IsDBNull(0)
                && Convert.ToBoolean(result.GetValue(0));

            var mensaje = result.IsDBNull(2)
                ? ""
                : Convert.ToString(result.GetValue(2));

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
}