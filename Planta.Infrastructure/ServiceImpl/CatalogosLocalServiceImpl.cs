using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class CatalogosLocalServiceImpl(ICatalogosLocalRepository repository, IMaestrosService maestros) : ICatalogosLocalService
{
    public Task<JsonElement> ListarGrupoClienteAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarGrupoClienteAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => repository.ListarPorProyectoAsync(catalogo, contexto, idProyecto, cancellationToken);

    public Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, ContextoUsuario contexto, string codigoCultivo, CancellationToken cancellationToken = default)
        => repository.ListarPorCultivoAsync(catalogo, contexto, codigoCultivo, cancellationToken);

    public async Task<JsonElement> ListarDestinatariosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => await repository.ListarDestinatariosAsync(contexto, await DeApimaestra(MaestroExterno.Clientes, contexto, cancellationToken), cancellationToken);

    public async Task<JsonElement> ListarConsignatariosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => await repository.ListarConsignatariosAsync(contexto, await DeApimaestra(MaestroExterno.Clientes, contexto, cancellationToken), cancellationToken);

    public async Task<JsonElement> ListarVariedadesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => await repository.ListarVariedadesAsync(contexto, await DeApimaestra(MaestroExterno.Variedades, contexto, cancellationToken), cancellationToken);

    public async Task<JsonElement> ListarAcopiosAsync(ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
        => await repository.ListarAcopiosAsync(contexto, idProyecto, await DeApimaestra(MaestroExterno.Acopios, contexto, cancellationToken), cancellationToken);

    public Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default)
        => repository.ListarMotivosTrasladoAsync(cancellationToken);

    public Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default)
        => repository.ListarUnidadesMedidaAsync(cancellationToken);

    public Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default)
        => repository.ListarTiposLocacionAsync(cancellationToken);

    public Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default)
        => repository.ListarPuertosAsync(cancellationToken);

    public Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default)
        => repository.ListarAeroPuertosAsync(cancellationToken);

    public Task<JsonElement> ObtenerEstablecimientoEmisorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ObtenerEstablecimientoEmisorAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarEstablecimientosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => repository.ListarEstablecimientosAsync(contexto, cancellationToken);

    public Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default)
        => repository.ListarDepartamentosAsync(cancellationToken);

    public Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default)
        => repository.ListarProvinciasAsync(codigoDepartamento, cancellationToken);

    public Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default)
        => repository.ListarDistritosAsync(codigoDepartamento, codigoProvincia, cancellationToken);

    /// <summary>Consulta apimaestra y entrega el resultado como texto JSON para el SP (formatoSp: nombres de propiedad que el SP espera).</summary>
    private async Task<string> DeApimaestra(MaestroExterno maestro, ContextoUsuario contexto, CancellationToken cancellationToken)
        => (await maestros.ConsultarAsync(maestro, contexto.IdEmpresa, contexto.Ruc, contexto.Usuario, formatoSp: true, cancellationToken: cancellationToken)).GetRawText();
}
