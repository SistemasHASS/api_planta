using Planta.Application.Catalogos.Models;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Maestros.Abstractions;
using System.Text.Json;

namespace Planta.Application.Catalogos;

public sealed class CatalogosUseCase(IMaestrosService maestrosService, ICatalogosService catalogosService) : Planta.Application.Catalogos.Abstractions.ICatalogosUseCase
{   

    public async Task<List<JsonElement>> SincronizarDestinatariosAsync(string idempresa, string ruc, string usuario, string idRol, string json)
    {
        return await catalogosService.SincronizarDestinatariosAsync(idempresa, ruc, usuario, idRol, json);
    }

    public async Task<CatalogosResponse<List<UbigeoDepartamento>>> ListarDepartamentosAsync()
    {
        return await catalogosService.ListarDepartamentosAsync();
    }

    public async Task<CatalogosResponse<List<UbigeoProvincia>>> ListarProvinciasAsync(string codigoDepartamento)
    {
        return await catalogosService.ListarProvinciasAsync(codigoDepartamento);
    }

    public async Task<CatalogosResponse<List<UbigeoDistrito>>> ListarDistritosAsync(string codigoDepartamento, string codigoProvincia)
    {
        return await catalogosService.ListarDistritosAsync(codigoDepartamento, codigoProvincia);
    }

    public async Task<CatalogosResponse<List<MotivoTraslado>>> ListarMotivosTrasladoAsync()
    {
        return await catalogosService.ListarMotivosTrasladoAsync();
    }

    public async Task<List<JsonElement>> SincronizarConsignatariosAsync(string idempresa, string ruc, string usuario, string json)
    {
        return await catalogosService.SincronizarConsignatariosAsync(idempresa, ruc, usuario, json);
    }

    public async Task<List<JsonElement>> SincronizarVariedadEsEnsayoAsync(string idempresa, string ruc, string usuario, string json)
    {
        return await catalogosService.SincronizarVariedadEsEnsayoAsync(idempresa, ruc, usuario, json);
    }

    public async Task<List<JsonElement>> SincronizarAcopiosAsync(string idempresa, string ruc, string usuario, string json, string json_detalle)
    {
        return await catalogosService.SincronizarAcopiosAsync(idempresa, ruc, usuario, json, json_detalle);
    }

    public async Task<CatalogosResponse<List<TipoProcesoEmpacado>>> GetTipoProcesoEmpacadoAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetTipoProcesoEmpacadoAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<List<JsonElement>> SincronizarCatalogosAsync(string tabla, string json, string idempresa, string ruc, string usuario)
    {
        return await catalogosService.SincronizarCatalogosAsync(tabla, json, idempresa, ruc, usuario);
    }

    public async Task<CatalogosResponse<List<UsuariosAcopio>>> GetListaUsuariosAsync(string usuario, string ruc)
    {
        var resp= await maestrosService.GetListaUsuariosAsync(usuario, ruc);

        if (resp is null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        var usuariosAcopio= await catalogosService.GetUsuarioAcopioAsync(JsonSerializer.Serialize(resp));

        return usuariosAcopio;
    }
    public async Task<CatalogosResponse<List<PersonalLogistico>>> GetPersonalLogisticoAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetPersonalLogisticoAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<Supervisor>>> GetSupervisoresAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetSupervisoresAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<Transportista>>> GetTransportistasAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetTransportistasAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<Vehiculo>>> GetVehiculosAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetVehiculosAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<Conductores>>> GetConductoresAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetConductoresAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<LugaresProduccion>>> GetLugaresProduccionAsync(string idempresa, string ruc, string idproyecto)
    {
        return await catalogosService.GetLugaresProduccionAsync(idempresa, ruc, idproyecto);
    }
    
    public async Task<CatalogosResponse<List<TipoCaja>>> GetTipoCajaAsync(string idempresa, string ruc, string codigoCultivo)
    {
        return await catalogosService.GetTipoCajaAsync(idempresa, ruc, codigoCultivo);
    }
    
    public async Task<CatalogosResponse<List<Presentacion>>> GetPresentacionesAsync(string idempresa, string ruc, string codigoCultivo)
    {
        return await catalogosService.GetPresentacionesAsync(idempresa, ruc, codigoCultivo);
    }
    
    public async Task<CatalogosResponse<List<TiposEmpaqueGuia>>> GetTiposEmpaqueGuiaAsync(string idempresa, string ruc, string codigoCultivo)
    {
        return await catalogosService.GetTiposEmpaqueGuiaAsync(idempresa, ruc, codigoCultivo);
    }

    public async Task<CatalogosResponse<List<Categoria>>> GetCategoriaAsync(string idempresa, string ruc,string codigoCultivo)
    {
        return await catalogosService.GetCategoriaAsync(idempresa, ruc,codigoCultivo);
    }

    public async Task<CatalogosResponse<List<TiposEmpaque>>> GetTiposEmpaquesAsync(string idempresa, string ruc, string codigoCultivo)
    {
        return await catalogosService.GetTiposEmpaquesAsync(idempresa, ruc, codigoCultivo);
    }

    public async Task<CatalogosResponse<List<Destinatarios>>> GetDestinatariosAsync(string idempresa, string ruc)
    {
        var resp= await maestrosService.GetClientesAsync(idempresa);

        var respDestinatarios = await catalogosService.GetDestinatariosAsync(idempresa, ruc, JsonSerializer.Serialize(resp));

        return respDestinatarios;
    }

    public async Task<List<JsonElement>> GetConsignatariosAsync(string idempresa, string ruc)
    {
        var resp= await maestrosService.GetClientesAsync(idempresa);

        var respConsignatarios = await catalogosService.ListarConsignatariosAsync(idempresa, ruc, JsonSerializer.Serialize(resp));
        return respConsignatarios;
    }

    public async Task<CatalogosResponse<List<Acopios>>> GetAcopiosAsync(string idempresa, string idproyecto)
    {
        var resp= maestrosService.GetAcopiosAsync(idempresa);

        var respAcopios = await catalogosService.GetAcopiosSeriesAsync(idempresa, idproyecto, JsonSerializer.Serialize(resp.Result));
        
        return respAcopios;
    }

    public Task<IReadOnlyList<FundoExterno>?> GetFundosAsync(string idempresa)
    {
        return maestrosService.GetFundosAsync(idempresa);
    }

    public async Task<CatalogosResponse<List<Formato>>> GetFormatosAsync(string idempresa, string ruc, string codigoCultivo)
    {
        return await catalogosService.GetFormatosAsync(idempresa, ruc, codigoCultivo);
    }

    public async Task<CatalogosResponse<List<GrupoCliente>>> GetClientesAsync(string idempresa, string ruc)
    {
        return await catalogosService.GetGrupoClienteAsync(idempresa, ruc);
    }

    public async  Task<CatalogosResponse<List<VariedadRepository>>> GetVariedadesAsync(string idempresa,string ruc)
    {
        var resp= maestrosService.GetVariedadesAsync(idempresa);

        var respVariedades = await catalogosService.GetVariedadAuxiliarAsync(idempresa, ruc, JsonSerializer.Serialize(resp.Result));

        return respVariedades;
    }

    public Task<IReadOnlyList<CultivoExterno>?> GetCultivosAsync(string idempresa)
    {
        return maestrosService.GetCultivosAsync(idempresa);
    }

    public Task<IReadOnlyList<CampaniaExterna>?> GetCampaniasAsync(string ruc)
    {
        return maestrosService.GetCampaniasAsync(ruc);
    }

    public Task<IReadOnlyList<PaisExterno>?> GetPaisesAsync()
    {
        return maestrosService.GetPaisesAsync();
    }

    public Task<IReadOnlyList<CalibreExterno>?> GetCalibresAsync()
    {
        return maestrosService.GetCalibresAsync();
    }

    public Task<IReadOnlyList<TransporteExterno>?> GetTransportesAsync()
    {
        return maestrosService.GetTransportesAsync();
    }

    public Task<CatalogosResponse<List<TipoClamshell>>>GetTiposClamshellAsync(string idempresa,string ruc,string codigoCultivo)
    {
        return catalogosService.GetTiposClamshellAsync(idempresa, ruc, codigoCultivo);
    }

    public Task<CatalogosResponse<List<CodigoRancho>>> GetCodigosRanchoAsync(string idempresa, string ruc, string idproyecto)
    {
        return catalogosService.GetCodigosRanchoAsync(idempresa, ruc, idproyecto);
    }

    public Task<CatalogosResponse<List<LugarProduccionConfig>>> GetLugaresProduccionConfigAsync(string idempresa, string ruc, string idproyecto)
    {
        return catalogosService.GetLugaresProduccionConfigAsync(idempresa, ruc, idproyecto);
    }

    public Task<CatalogosResponse<List<Parametro>>> ListarParametrosAsync(string idempresa, string ruc)
    {
        return catalogosService.ListarParametrosAsync(idempresa, ruc);
    }
}