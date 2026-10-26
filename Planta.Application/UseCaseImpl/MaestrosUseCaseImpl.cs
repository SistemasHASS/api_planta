using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

public sealed class MaestrosUseCaseImpl(IMaestrosService maestros, ICurrentUser currentUser) : IMaestrosUseCase
{
    public Task<JsonElement> ConsultarAsync(MaestroExterno maestro, CancellationToken cancellationToken = default)
        => maestros.ConsultarAsync(maestro, Requerir(currentUser.IdEmpresa, "la empresa"), Requerir(currentUser.Ruc, "el RUC"), currentUser.UserName, false, cancellationToken);

    private static string Requerir(string? valor, string nombre)
    {
        var texto = valor?.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            throw new BadRequestException($"No se encontro {nombre} en el token del usuario.");
        }

        return texto;
    }
}
