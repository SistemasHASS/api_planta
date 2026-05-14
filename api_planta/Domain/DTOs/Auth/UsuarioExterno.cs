using System.Text.Json.Serialization;

namespace api_planta.Domain.DTOs.Auth;

public class UsuarioExterno
{   
    // [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    // [JsonPropertyName("sociedad")]
    public int Sociedad { get; set; }
    
    // [JsonPropertyName("idempresa")]
    public string Idempresa { get; set; } = "";
    
    // [JsonPropertyName("ruc")]
    public string Ruc { get; set; } = "";
    
    // [JsonPropertyName("razonsocial")]
    public string RazonSocial { get; set; } = "";
    
    // [JsonPropertyName("proyecto")]
    public string Proyecto { get; set; } = "";
    
    // [JsonPropertyName("documentoIdentidad")]
    public string Documentoidentidad { get; set; } = "";
    
    // [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = "";
    
    // [JsonPropertyName("usuario")]
    public string Usuario { get; set; } = "";
    
    // [JsonPropertyName("idRol")]
    public string Idrol { get; set; } = "";
    
    // [JsonPropertyName("rol")]
    public string Rol { get; set; } = "";
    
    // [JsonPropertyName("aplicacion")]
    public string Aplicacion { get; set; } = "";
    
    // [JsonPropertyName("modoTrabajo")]
    public int Modotrabajo { get; set; }
    
    // [JsonPropertyName("fechaCompensacion")]
    public string Fechacompensacion { get; set; } = "";
}


public class ListaUsuariosResponse
{
     [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("data")]
    public List<UsuarioExterno> Data { get; set; } = [];

    [JsonPropertyName("mensaje")]
    public string Mensaje { get; set; } = "";
}

public class UsuarioAcopioDto
{
    public int id { get; set; }
    public string idempresa { get; set; } = "";
    public string ruc { get; set; } = "";
    public string razonSocial { get; set; } = "";
    public string documentoIdentidad { get; set; } = "";
    public string nombre { get; set; } = "";
    public string usuario { get; set; } = "";
    public string idRol { get; set; } = "";
    public string aplicacion { get; set; } = "";
    public int acopioId { get; set; }
    public string acopioNombre { get; set; } = "";
    public string serieGuia { get; set; } = "";
}


