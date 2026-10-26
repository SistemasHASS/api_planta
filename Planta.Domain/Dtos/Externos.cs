using System.Text.Json.Serialization;

namespace Planta.Domain.Dtos;

/// <summary>
/// Modelos de lo que devuelve apimaestra. Se conservan igual que en la API anterior porque los SP leen estos
/// nombres exactos al recibir el JSON (por eso las propiedades sin [JsonPropertyName] salen en PascalCase al enviarse a un SP).
/// </summary>
public sealed class FundoExterno
{
    public int Id { get; set; }
    public int Empresa { get; set; }
    public int Fundo { get; set; }
    public string CodigoFundo { get; set; } = "";
    public string NombreFundo { get; set; } = "";
    public string Direccion { get; set; } = "";
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

public sealed class AcopiosExterno
{
    public int Id { get; set; }

    public string Ruc { get; set; } = "";
    public int Nave { get; set; }
    public string codigo_acopio { get; set; } = "";
    public string Acopio { get; set; } = "";
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


public sealed class EmpresaExterna
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("empresa")]
    public int Empresa { get; set; }

    [JsonPropertyName("idempresa")]
    public string IdEmpresa { get; set; } = "";

    [JsonPropertyName("companyowner")]
    public string CompanyOwner { get; set; } = "";

    [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = "";

    [JsonPropertyName("razonsocial")]
    public string RazonSocial { get; set; } = "";

    [JsonPropertyName("direccionfiscal")]
    public string DireccionFiscal { get; set; } = "";
}
