

using System.Text.Json.Serialization;

namespace Planta.Application.Catalogos.Models;

public sealed class Formato
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo { get; set; } = "";
    
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";
    
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";
    
    [JsonPropertyName("pesoPorCaja")]
    public decimal PesoPorCaja { get; set; }
    
    [JsonPropertyName("limiteCajasPorPalet")]
    public int LimiteCajasPorPalet { get; set; }
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
    
    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class TipoProcesoEmpacado
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("idproyecto")]
    public string IdProyecto { get; set; } = "";
    
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
    
    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";

   
}

public sealed class TipoClamshell
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    // [JsonPropertyName("idempresa")]
    // public string IdEmpresa { get; set; } = "";
    
    // [JsonPropertyName("ruc")]
    // public string Ruc { get; set; } = "";
      
    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo { get; set; } = "";
    
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
    
    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class CatalogosResponse<T>
{
    public bool Error { get; set; }
    public T? Data { get; set; }
    public string Mensaje { get; set; } = "";
}

public sealed class GrupoCliente
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
    
    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Parametro
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("idparametro")]
    public string IdParametro { get; set; } = "";
    
    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";
    
    [JsonPropertyName("valor")]
    public string Valor { get; set; } = "";
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
}


public sealed class Destinatarios
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("idCliente")]
    public int IdCliente { get; set; }
    
    [JsonPropertyName("documentoFiscal")]
    public string DocumentoFiscal { get; set; } = "";
    
    [JsonPropertyName("documento")]
    public string Documento { get; set; } = "";
    
    [JsonPropertyName("tipoDocumento")]
    public string TipoDocumento { get; set; } = "";
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    
    [JsonPropertyName("esCliente")]
    public string EsCliente { get; set; } = "";
    
    [JsonPropertyName("domicilioFiscal")]
    public string DomicilioFiscal { get; set; } = "";
    
    [JsonPropertyName("puntoLlegada")]
    public string PuntoLlegada { get; set; } = "";
    
    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
}

public sealed class UsuariosAcopio
{
    public string id { get; set; } = "";
    public string idempresa { get; set; } = "";
    public string ruc { get; set; } = "";
    public string razonSocial { get; set; } = "";
    public string documentoIdentidad { get; set; } = "";
    public string nombre { get; set; } = "";
    public string usuario { get; set; } = "";
    public string idRol { get; set; } = "";
    public string aplicacion { get; set; } = "";
    public string codigoAcopio { get; set; } = "";
    public string acopioNombre { get; set; } = "";
    public string serieGuia { get; set; } = "";
}

public sealed class ListaUsuariosExterno
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = "";

    [JsonPropertyName("aplicacion")]
    public string Aplicacion { get; set; } = "";

    [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = "";

    [JsonPropertyName("idrol")]
    public string Idrol { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("documentoidentidad")]
    public string Documentoidentidad { get; set; } = "";

    [JsonPropertyName("idempresa")]
    public string Idempresa { get; set; } = "";

    [JsonPropertyName("razonsocial")]
    public string RazonSocial { get; set; } = "";
}

public sealed class PersonalLogistico
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa {get; set;} = "";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";

    [JsonPropertyName("dni")]
    public string Dni { get; set; } = "";

    [JsonPropertyName("nombreCompleto")]
    public string NombreCompleto { get; set; } = "";
    
    [JsonPropertyName("celular")]
    public string Celular { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Supervisor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa {get; set;} = "";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";
    
    [JsonPropertyName("idproyecto")]
    public string Idproyecto {get; set;} = "";

    [JsonPropertyName("dni")]
    public string Dni { get; set; } = "";
    
    [JsonPropertyName("celular")]
    public string Celular { get; set; } = "";

    [JsonPropertyName("nombreCompleto")]
    public string NombreCompleto { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Transportista
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa {get; set;} = "";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";
    
    [JsonPropertyName("idproyecto")]
    public string Idproyecto { get; set; } = "";

    [JsonPropertyName("ruc_Transportistas")]
    public string Ruc_Transportistas { get; set; } = "";

    [JsonPropertyName("razonSocial")]
    public string RazonSocial { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Vehiculo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";
    [JsonPropertyName("idproyecto")]
    public string Idproyecto {get; set;} = "";

    [JsonPropertyName("placaPrincipal")]
    public string PlacaPrincipal { get; set; } = "";

    [JsonPropertyName("marca")]
    public string Marca { get; set; } = "";

    [JsonPropertyName("certificadoInscripcion")]
    public string CertificadoInscripcion { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Conductores
{

    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";
    [JsonPropertyName("idproyecto")]
    public string Idproyecto {get; set;} = "";

    [JsonPropertyName("documentoIdentidad")]
    public string DocumentoIdentidad { get; set; } = "";

    [JsonPropertyName("licenciaConducir")]
    public string LicenciaConducir { get; set; } = "";

    [JsonPropertyName("nombreCompleto")]
    public string NombreCompleto { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";

}

public sealed class LugaresProduccion
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class TipoCaja
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";

    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo {get; set;} = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}
public sealed class Presentacion
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";

    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo {get; set;} = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}


public sealed class TiposEmpaqueGuia
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";
    
    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo {get; set;} = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    [JsonPropertyName("codigoTipoEmpaque")]
    public string CodigoTipoEmpaque { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Categoria
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";
       [JsonPropertyName("calibreId")]
    public string CalibreId {get; set;} = "";

      [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo {get; set;} = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class TiposEmpaque
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    //  [JsonPropertyName("idempresa")]
    // public string Idempresa{get;set;} ="";

    // [JsonPropertyName("ruc")]
    // public string Ruc {get; set;} = "";

    [JsonPropertyName("codigoCultivo")]
    public string CodigoCultivo {get; set;} = "";
        
    [JsonPropertyName("codigoCategoria")]
    public string CodigoCategoria {get; set;} = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";

}

public sealed class TipoProcesoEmpacadoDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }
    
    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class Acopios
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("codigoAcopio")]
    public string CodigoAcopio { get; set; } = "";

    [JsonPropertyName("acopioNombre")]
    public string AcopioNombre { get; set; } = "";

    [JsonPropertyName("serieGuia")]
    public string SerieGuia { get; set; } = "";
    
    [JsonPropertyName("tiposProcesoEmpacado")]
    public List<TipoProcesoEmpacadoDto> TiposProcesoEmpacado { get; set; } = new();
}


public sealed class AcopiosExterno
{
    public int Id { get; set; }

    public string Ruc { get; set; } = "";
    public int Nave { get; set; }
    public string codigo_acopio { get; set; } = "";
    public string Acopio { get; set; } = "";
}

public sealed class FundoExterno
{
    public int Id { get; set; }
    public int Empresa { get; set; }
    public int Fundo { get; set; }
    public string CodigoFundo { get; set; } = "";
    public string NombreFundo { get; set; } = "";
    public string Direccion { get; set; } = "";
}

public sealed class FormatoExterno
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("envase")]
    public int Envase { get; set; }

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("clasificacion_envase")]
    public int ClasificacionEnvase { get; set; }

    [JsonPropertyName("peso")]
    public decimal Peso { get; set; }

    [JsonPropertyName("descripcion2")]
    public string Descripcion2 { get; set; } = "";

    [JsonPropertyName("kg_reales")]
    public decimal KgReales { get; set; }

    [JsonPropertyName("kg_conversion")]
    public decimal KgConversion { get; set; }
}

public sealed class ClienteExterno
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("persona")]
    public int Persona { get; set; }

    [JsonPropertyName("documentoFiscal")]
    public string DocumentoFiscal { get; set; } = "";

    [JsonPropertyName("documento")]
    public string Documento { get; set; } = "";

    [JsonPropertyName("tipoDocumento")]
    public string TipoDocumento { get; set; } = "";

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("esCliente")]
    public string EsCliente { get; set; } = "";
}

public sealed class VariedadRepository
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("idempresa")]
    public string IdEmpresa { get; set; } = "";

    [JsonPropertyName("ruc")]
    public string ruc { get; set; } = "";

    [JsonPropertyName("cultivo")]
    public string Cultivo { get; set; } = "";

    [JsonPropertyName("idcultivo")]
    public string IdCultivo { get; set; } = "";

    [JsonPropertyName("idvariedad")]
    public string IdVariedad { get; set; } = "";

    [JsonPropertyName("variedad")]
    public string Variedad { get; set; } = "";

    [JsonPropertyName("procedencia")]
    public string Procedencia { get; set; } = "";

    [JsonPropertyName("esEnsayo")]
    public bool EsEnsayo { get; set; }
    [JsonPropertyName("eliminado")]
    public bool Eliminado { get; set; }

}

public sealed class VariedadExterna
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";
    
    [JsonPropertyName("cultivo")]
    public string Cultivo { get; set; } = "";


    [JsonPropertyName("idcultivo")]
    public string IdCultivo { get; set; } = "";

    [JsonPropertyName("idvariedad")]
    public string IdVariedad { get; set; } = "";

    [JsonPropertyName("variedad")]
    public string Variedad { get; set; } = "";
}

public sealed class CultivoExterno
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("empresa")]
    public int Empresa { get; set; }

    [JsonPropertyName("cultivo")]
    public int Cultivo { get; set; }

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("estado")]
    public int Estado { get; set; }
}

public sealed class CampaniaExterna
{
    [JsonPropertyName("idproyecto")]
    public string IdProyecto { get; set; } = "";

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = "";

    [JsonPropertyName("fecha_inicio")]
    public DateTime FechaInicio { get; set; }

    [JsonPropertyName("fecha_fin")]
    public DateTime FechaFin { get; set; }

    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = "";

    [JsonPropertyName("estado")]
    public int Estado { get; set; }

    [JsonPropertyName("codcultivo")]
    public string CodCultivo { get; set; } = "";

    [JsonPropertyName("aplicacion")]
    public string Aplicacion { get; set; } = "";

    [JsonPropertyName("idfundo")]
    public string IdFundo { get; set; } = "";

    [JsonPropertyName("fruta")]
    public string Fruta { get; set; } = "";
}

public sealed class PaisExterno
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("pais")]
    public string Pais { get; set; } = "";

    [JsonPropertyName("nacionalidad")]
    public string Nacionalidad { get; set; } = "";
}

public sealed class CalibreExterno
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("calibre")]
    public string Calibre { get; set; } = "";

    [JsonPropertyName("idCultivo")]
    public string IdCultivo { get; set; } = "";
}

public sealed class TransporteExterno
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("transporte")]
    public string Transporte { get; set; } = "";

    [JsonPropertyName("factorTEUtoPallet")]
    public string FactorTeuToPallet { get; set; } = "";

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = "";
}

public sealed class TipoClamshellExterno
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("clamshell")]
    public string Clamshell { get; set; } = "";

    [JsonPropertyName("peso")]
    public float Peso { get; set; }
}

public sealed class CodigoRancho
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    // [JsonPropertyName("idproyecto")]
    // public string IdProyecto { get; set; } = "";

    [JsonPropertyName("codigo")]
    public string Codigo { get; set; } = "";

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}

public sealed class LugarProduccionConfig
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("idproyecto")]
    public string IdProyecto { get; set; } = "";

    [JsonPropertyName("idCodigoRancho")]
    public int IdCodigoRancho { get; set; }

    [JsonPropertyName("idLugaresDeProduccion")]
    public int IdLugaresDeProduccion { get; set; }

    [JsonPropertyName("activo")]
    public bool Activo { get; set; }

    [JsonPropertyName("fechaCreacion")]
    public string FechaCreacion { get; set; } = "";
}