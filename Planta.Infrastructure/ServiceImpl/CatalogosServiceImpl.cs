

using System.Text.Json;
using Planta.Application.Auth.Models;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Catalogos.Models;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class CatalogosServiceImpl(ICatalogosRepository catalogosRepository): ICatalogosService
{
    public Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosAsync(string idempresa, string ruc, string json)
        => catalogosRepository.GetDestinatariosAsync(idempresa, ruc, json);
    
    public Task<List<JsonElement>> SincronizarDestinatariosAsync(string idempresa, string ruc, string usuario, string idRol, string json)
        => catalogosRepository.SincronizarDestinatariosAsync(idempresa, ruc, usuario, idRol, json);
    
    public Task<List<JsonElement>> SincronizarAcopiosAsync(string idempresa, string ruc, string usuario, string json, string json_detalle)
        => catalogosRepository.SincronizarAcopiosAsync(idempresa, ruc, usuario, json, json_detalle);

    public Task<CatalogosResponse<List<TipoProcesoEmpacado>>> GetTipoProcesoEmpacadoAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetTipoProcesoEmpacadoAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<Formato>>> GetFormatosAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetFormatosAsync(idempresa, ruc, codigoCultivo);
    
    public Task<CatalogosResponse<List<TipoClamshell>>> GetTiposClamshellAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetTiposClamshellAsync(idempresa, ruc, codigoCultivo);
    
    public Task<List<JsonElement>> SincronizarCatalogosAsync(string tabla, string json, string idempresa, string ruc, string usuario)
        => catalogosRepository.SincronizarCatalogosAsync(tabla, json, idempresa, ruc, usuario);
    
    public Task<CatalogosResponse<List<UsuariosAcopio>>> GetUsuarioAcopioAsync(string json)
        => catalogosRepository.GetUsuarioAcopioAsync(json);
    
    public Task<CatalogosResponse<List<PersonalLogistico>>> GetPersonalLogisticoAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetPersonalLogisticoAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<Supervisor>>> GetSupervisoresAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetSupervisoresAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<Transportista>>> GetTransportistasAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetTransportistasAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<Vehiculo>>> GetVehiculosAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetVehiculosAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<Conductores>>> GetConductoresAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetConductoresAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<LugaresProduccion>>> GetLugaresProduccionAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetLugaresProduccionAsync(idempresa, ruc, idproyecto);
    
    public Task<CatalogosResponse<List<TipoCaja>>> GetTipoCajaAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetTipoCajaAsync(idempresa, ruc, codigoCultivo);
    
    public Task<CatalogosResponse<List<Presentacion>>> GetPresentacionesAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetPresentacionesAsync(idempresa, ruc, codigoCultivo);
    
    public Task<CatalogosResponse<List<TiposEmpaqueGuia>>> GetTiposEmpaqueGuiaAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetTiposEmpaqueGuiaAsync(idempresa, ruc, codigoCultivo);
    
    public Task<CatalogosResponse<List<Categoria>>> GetCategoriaAsync(string idempresa, string ruc,string codigoCultivo)
        => catalogosRepository.GetCategoriaAsync(idempresa, ruc,codigoCultivo);


     public Task<CatalogosResponse<List<TiposEmpaque>>> GetTiposEmpaquesAsync(string idempresa, string ruc, string codigoCultivo)
        => catalogosRepository.GetTiposEmpaquesAsync(idempresa, ruc, codigoCultivo);

    public Task<CatalogosResponse<List<Acopios>>> GetAcopiosSeriesAsync(string idempresa, string json)
        => catalogosRepository.GetAcopiosSeriesAsync(idempresa, json);
    
    public Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadAuxiliarAsync(string idempresa, string ruc, string json)
        => catalogosRepository.GetVariedadAuxiliarAsync(idempresa, ruc, json);

    public Task<CatalogosResponse<List<CodigoRancho>>> GetCodigosRanchoAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetCodigosRanchoAsync(idempresa, ruc, idproyecto);

    public Task<CatalogosResponse<List<LugarProduccionConfig>>> GetLugaresProduccionConfigAsync(string idempresa, string ruc, string idproyecto)
        => catalogosRepository.GetLugaresProduccionConfigAsync(idempresa, ruc, idproyecto);
}   