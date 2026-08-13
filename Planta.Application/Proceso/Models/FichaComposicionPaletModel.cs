using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Planta.Application.Proceso.Models
{
    public class FichaComposicionPaletModel
    {
        public FichaCabecera Cabecera { get; set; } = new();
        public List<FichaDetalle> Detalle { get; set; } = new();
    }

    public class FichaCabecera
    {
        public string FechaProceso { get; set; } = string.Empty;
        public string PlantaDeEmpaque { get; set; } = string.Empty;
        public string IdPalet_numero { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public int TotalCajas { get; set; }
        public List<ClienteItem> Clientes { get; set; } = new();
        public List<DestinoItem> Destinos { get; set; } = new();
        
        public string Codigo { get; set; } = string.Empty;
        public string FV { get; set; } = string.Empty;

        [JsonPropertyName("Activadad Economica")]
        public string ActividadEconomica { get; set; } = string.Empty;

        public string Ruc { get; set; } = string.Empty;

        [JsonPropertyName("Hora Final")]
        public string HoraFinal { get; set; } = string.Empty;

        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; } = string.Empty;
    }

    public class FichaDetalle
    {
        public List<ClienteItem> Clientes { get; set; } = new();

        public List<VariedadItem> Variedad { get; set; } = new();
        
        [JsonPropertyName("Tipo De Empaque")]
        public string Tipo_De_Empaque { get; set; } = string.Empty;
        
        public int CantidadCajas { get; set; }
        
        [JsonPropertyName("LDP_C_Rancho")]
        public string LDP_C_Rancho { get; set; } = string.Empty;
    }

    public class ClienteItem
    {
        public string Cliente { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
    }

    public class DestinoItem
    {
        public string DestinoId { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
    }

    public class VariedadItem
    {
        public string Variedad { get; set; } = string.Empty;
        public string VariedadId { get; set; } = string.Empty;
        public string CodigoCultivo { get; set; } = string.Empty;
    }
}
