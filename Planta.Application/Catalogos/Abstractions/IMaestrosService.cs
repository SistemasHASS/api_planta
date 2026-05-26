

using Planta.Application.Catalogos.Models;

namespace Planta.Application.Maestros.Abstractions;

public interface IMaestrosService
{
    Task<List<ListaUsuariosExterno>?> GetListaUsuariosAsync(string usuario, string ruc);
    Task<IReadOnlyList<AcopiosExterno>?> GetAcopiosAsync(string idempresa);
    Task<IReadOnlyList<FundoExterno>?> GetFundosAsync(string idempresa);
    Task<IReadOnlyList<FormatoExterno>?> GetFormatosAsync(string idempresa);
    Task<IReadOnlyList<ClienteExterno>?> GetClientesAsync(string idempresa);
    Task<IReadOnlyList<VariedadExterna>?> GetVariedadesAsync(string idempresa);
    Task<IReadOnlyList<CultivoExterno>?> GetCultivosAsync(string idempresa);
    Task<IReadOnlyList<CampaniaExterna>?> GetCampaniasAsync(string ruc);
    Task<IReadOnlyList<PaisExterno>?> GetPaisesAsync();
    Task<IReadOnlyList<CalibreExterno>?> GetCalibresAsync();
    Task<IReadOnlyList<TransporteExterno>?> GetTransportesAsync();
    Task<IReadOnlyList<TipoClamshellExterno>?> GetTiposClamshellAsync();
}