using Planta.Application.Catalogos.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace Planta.Application.Catalogos.Abstractions;

public interface ICatalogosUseCase
{
    Task<List<JsonElement>> SincronizarCatalogosAsync(string tabla, string json, string idempresa, string ruc, string usuario);
    Task<CatalogosResponse<List<UsuariosAcopio>>> GetListaUsuariosAsync(string usuario, string ruc);
    Task<CatalogosResponse<List<PersonalLogistico>>> GetPersonalLogisticoAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Supervisor>>> GetSupervisoresAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Transportista>>> GetTransportistasAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Vehiculo>>> GetVehiculosAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Conductores>>> GetConductoresAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<LugaresProduccion>>> GetLugaresProduccionAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<TipoCaja>>> GetTipoCajaAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Presentacion>>> GetPresentacionesAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<TiposEmpaqueGuia>>> GetTiposEmpaqueGuiaAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Categoria>>> GetCategoriaAsync(string idempresa, string ruc,string codigoCultivo);
    Task<CatalogosResponse<List<TiposEmpaque>>> GetTiposEmpaquesAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Acopios>>> GetAcopiosAsync(string idempresa);
    Task<IReadOnlyList<FundoExterno>?> GetFundosAsync(string idempresa);
    Task<CatalogosResponse<List<Formato>>> GetFormatosAsync(string idempresa, string ruc, string codigoCultivo);
    Task<IReadOnlyList<ClienteExterno>?> GetClientesAsync(string idempresa);
    Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadesAsync(string idempresa, string ruc);
    Task<IReadOnlyList<CultivoExterno>?> GetCultivosAsync(string idempresa);
    Task<IReadOnlyList<CampaniaExterna>?> GetCampaniasAsync(string ruc);
    Task<IReadOnlyList<PaisExterno>?> GetPaisesAsync();
    Task<IReadOnlyList<CalibreExterno>?> GetCalibresAsync();
    Task<IReadOnlyList<TransporteExterno>?> GetTransportesAsync();
    Task<CatalogosResponse<List<TipoClamshell>>> GetTiposClamshellAsync(string idempresa, string ruc, string codigoCultivo);
}