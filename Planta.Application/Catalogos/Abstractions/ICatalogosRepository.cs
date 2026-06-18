

using System.Text.Json;
using Planta.Application.Catalogos.Models;

namespace Planta.Application.Catalogos.Abstractions;

public interface ICatalogosRepository
{
    Task<List<JsonElement>> ListarConsignatariosAsync(string idempresa, string ruc, string json);
    Task<CatalogosResponse<List<GrupoCliente>>> GetGrupoClienteAsync(string idempresa, string ruc);
    Task<CatalogosResponse<List<Parametro>>> GetParametroEmpresaAsync(string idempresa, string ruc, string idparametro);
    Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosAsync(string idempresa, string ruc, string json);
    Task<List<JsonElement>> SincronizarDestinatariosAsync(string idempresa, string ruc, string usuario, string idRol, string json);
    Task<List<JsonElement>> SincronizarConsignatariosAsync(string idempresa, string ruc, string usuario, string json);
    Task<List<JsonElement>> SincronizarVariedadEsEnsayoAsync(string idempresa, string ruc, string usuario, string json);
    Task<List<JsonElement>> SincronizarAcopiosAsync(string idempresa, string ruc, string usuario, string json, string json_detalle);
    Task<CatalogosResponse<List<TipoProcesoEmpacado>>> GetTipoProcesoEmpacadoAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Formato>>> GetFormatosAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<TipoClamshell>>> GetTiposClamshellAsync(string idempresa, string ruc, string codigoCultivo);
    Task<List<JsonElement>> SincronizarCatalogosAsync(string tabla, string json, string idempresa, string ruc, string usuario);
    Task<CatalogosResponse<List<UsuariosAcopio>>> GetUsuarioAcopioAsync(string json);
    Task<CatalogosResponse<List<PersonalLogistico>>> GetPersonalLogisticoAsync(string idempresa, string ruc,string idproyecto);
    Task<CatalogosResponse<List<Supervisor>>> GetSupervisoresAsync(string idempresa, string ruc,string idproyecto);
    Task<CatalogosResponse<List<Transportista>>> GetTransportistasAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Vehiculo>>> GetVehiculosAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Conductores>>> GetConductoresAsync(string idempresa, string ruc,string idproyecto);
    Task<CatalogosResponse<List<LugaresProduccion>>> GetLugaresProduccionAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<TipoCaja>>> GetTipoCajaAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Presentacion>>> GetPresentacionesAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<TiposEmpaqueGuia>>> GetTiposEmpaqueGuiaAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Categoria>>> GetCategoriaAsync(string idempresa, string ruc,string codigoCultivo);
    Task<CatalogosResponse<List<TiposEmpaque>>> GetTiposEmpaquesAsync(string idempresa, string ruc, string codigoCultivo);
    Task<CatalogosResponse<List<Acopios>>> GetAcopiosSeriesAsync(string idempresa, string idproyecto, string json);
    Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadAuxiliarAsync(string idempresa,string ruc, string json);
    Task<CatalogosResponse<List<CodigoRancho>>> GetCodigosRanchoAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<LugarProduccionConfig>>> GetLugaresProduccionConfigAsync(string idempresa, string ruc, string idproyecto);
    Task<CatalogosResponse<List<Parametro>>> ListarParametrosAsync(string idempresa, string ruc);
}