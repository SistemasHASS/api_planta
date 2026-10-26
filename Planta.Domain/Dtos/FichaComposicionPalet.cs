using System.Text.Json.Serialization;

namespace Planta.Domain.Dtos;

/// <summary>Datos de la ficha de composicion de un palet, tal como los devuelve PLANTA_PDF_FichaComposicionPalet.</summary>
public sealed class FichaComposicionPalet
{
    public FichaCabecera Cabecera { get; set; } = new();
    public List<FichaDetalle> Detalle { get; set; } = [];
}

public sealed class FichaCabecera
{
    public string FechaProceso { get; set; } = "";
    public string PlantaDeEmpaque { get; set; } = "";
    public string IdPalet_numero { get; set; } = "";
    public string Formato { get; set; } = "";
    public int TotalCajas { get; set; }
    public List<FichaCliente> Clientes { get; set; } = [];
    public List<FichaDestino> Destinos { get; set; } = [];
    public string Codigo { get; set; } = "";
    public string FV { get; set; } = "";

    [JsonPropertyName("Activadad Economica")]
    public string ActividadEconomica { get; set; } = "";

    public string Ruc { get; set; } = "";

    [JsonPropertyName("Hora Final")]
    public string HoraFinal { get; set; } = "";

    [JsonPropertyName("razon_social")]
    public string RazonSocial { get; set; } = "";
}

public sealed class FichaDetalle
{
    public List<FichaCliente> Clientes { get; set; } = [];
    public List<FichaVariedad> Variedad { get; set; } = [];

    [JsonPropertyName("Tipo De Empaque")]
    public string TipoDeEmpaque { get; set; } = "";

    public int CantidadCajas { get; set; }

    [JsonPropertyName("LDP_C_Rancho")]
    public string LdpCRancho { get; set; } = "";
}

public sealed class FichaCliente
{
    public string Cliente { get; set; } = "";
    public string Documento { get; set; } = "";
}

public sealed class FichaDestino
{
    public string DestinoId { get; set; } = "";
    public string Destino { get; set; } = "";
}

public sealed class FichaVariedad
{
    public string Variedad { get; set; } = "";
    public string VariedadId { get; set; } = "";
    public string CodigoCultivo { get; set; } = "";
}
