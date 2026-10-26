using System.Text.Json;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>SP: base-datos/sp/PLANTA_Listar*.sql y afines. Solo llama al SP con sus parametros y devuelve su respuesta.</summary>
public sealed class CatalogosLocalRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), ICatalogosLocalRepository
{
    public Task<JsonElement> ListarGrupoClienteAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarGrupoCliente", cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc));

    public Task<JsonElement> ListarPorProyectoAsync(CatalogoPorProyecto catalogo, ContextoUsuario contexto, string idProyecto, CancellationToken cancellationToken = default)
    {
        var procedimiento = catalogo switch
        {
            CatalogoPorProyecto.TiposProcesoEmpacado => "PLANTA_ListarTipoProcesoEmpacado",
            CatalogoPorProyecto.CodigosRancho => "PLANTA_ListarCodigosRancho",
            CatalogoPorProyecto.LugaresProduccion => "PLANTA_ListarLugaresProduccion",
            CatalogoPorProyecto.Conductores => "PLANTA_ListarConductores",
            CatalogoPorProyecto.Vehiculos => "PLANTA_ListarVehiculos",
            CatalogoPorProyecto.Transportistas => "PLANTA_ListarTransportistas",
            CatalogoPorProyecto.Supervisores => "PLANTA_ListarSupervisores",
            CatalogoPorProyecto.PersonalLogistico => "PLANTA_ListarPersonalLogistica",
            _ => throw new ArgumentOutOfRangeException(nameof(catalogo))
        };

        return Consultar(procedimiento, cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc), ("idproyecto", idProyecto));
    }

    public Task<JsonElement> ListarPorCultivoAsync(CatalogoPorCultivo catalogo, ContextoUsuario contexto, string codigoCultivo, CancellationToken cancellationToken = default)
    {
        var procedimiento = catalogo switch
        {
            CatalogoPorCultivo.Formatos => "PLANTA_ListarFormatos",
            CatalogoPorCultivo.TiposClamshell => "PLANTA_ListarTiposClamshell",
            CatalogoPorCultivo.TiposCaja => "PLANTA_ListarTiposCaja",
            CatalogoPorCultivo.Presentaciones => "PLANTA_ListarPresentacion",
            CatalogoPorCultivo.TiposEmpaqueGuia => "PLANTA_ListarTiposEmpaqueGuia",
            CatalogoPorCultivo.Categorias => "PLANTA_ListarCategorias",
            CatalogoPorCultivo.TiposEmpaque => "PLANTA_ListarTiposEmpaque",
            _ => throw new ArgumentOutOfRangeException(nameof(catalogo))
        };

        return Consultar(procedimiento, cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc), ("codigoCultivo", codigoCultivo));
    }

    public Task<JsonElement> ListarDestinatariosAsync(ContextoUsuario contexto, string jsonClientes, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_GETDestinatarios", cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc), ("json", jsonClientes));

    public Task<JsonElement> ListarConsignatariosAsync(ContextoUsuario contexto, string jsonClientes, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarConsignatarios", cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc), ("json", jsonClientes));

    public Task<JsonElement> ListarVariedadesAsync(ContextoUsuario contexto, string jsonVariedades, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_GetVariedadAuxiliar", cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc), ("json", jsonVariedades));

    public Task<JsonElement> ListarAcopiosAsync(ContextoUsuario contexto, string idProyecto, string jsonAcopios, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_GETAcopio_Config", cancellationToken,
            ("idempresa", contexto.IdEmpresa), ("idproyecto", idProyecto), ("json", jsonAcopios));

    public Task<JsonElement> ListarMotivosTrasladoAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarMotivosTraslado", cancellationToken);

    public Task<JsonElement> ListarUnidadesMedidaAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarUnidadesMedida", cancellationToken);

    public Task<JsonElement> ListarTiposLocacionAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarTiposLocacion", cancellationToken);

    public Task<JsonElement> ListarPuertosAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarPuertos", cancellationToken);

    public Task<JsonElement> ListarAeroPuertosAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarAeroPuertos", cancellationToken);

    public Task<JsonElement> ObtenerEstablecimientoEmisorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_GetEstablecimientoEmisor", cancellationToken, ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc));

    public Task<JsonElement> ListarEstablecimientosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarEstablecimientos", cancellationToken, ("idempresa", contexto.IdEmpresa), ("ruc", contexto.Ruc));

    public Task<JsonElement> ListarDepartamentosAsync(CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarDepartamentos", cancellationToken);

    public Task<JsonElement> ListarProvinciasAsync(string codigoDepartamento, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarProvincias", cancellationToken, ("codigoDepartamento", codigoDepartamento));

    public Task<JsonElement> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia, CancellationToken cancellationToken = default)
        => Consultar("PLANTA_ListarDistritos", cancellationToken, ("codigoDepartamento", codigoDepartamento), ("codigoProvincia", codigoProvincia));

    /// <summary>Llama al SP con parametros (nombre sin @, valor). Un valor null se envia como texto vacio.</summary>
    private Task<JsonElement> Consultar(string procedimiento, CancellationToken cancellationToken, params (string Nombre, string? Valor)[] parametros)
        => EjecutarSobreAsync(procedimiento, command =>
        {
            foreach (var (nombre, valor) in parametros)
            {
                Agregar(command, "@" + nombre, valor ?? string.Empty);
            }
        }, cancellationToken);
}
