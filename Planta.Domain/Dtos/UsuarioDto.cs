namespace Planta.Domain.Dtos;

/// <summary>
/// Usuario autenticado de PLANTA: datos de apimaestra mas los datos de acopio que devuelve
/// el procedimiento PLANTA_GetUsuarioAcopio (codigoAcopio, acopioNombre, serieGuia).
/// </summary>
public sealed class UsuarioDto
{
    public string? Id { get; set; }
    public string? IdEmpresa { get; set; }
    public string? Ruc { get; set; }
    public string? RazonSocial { get; set; }
    public string? IdProyecto { get; set; }
    public string? DocumentoIdentidad { get; set; }
    public string? Usuario { get; set; }
    public string? Nombre { get; set; }
    public string? IdRol { get; set; }
    public string? Aplicacion { get; set; }
    public string? CodigoAcopio { get; set; }
    public string? AcopioNombre { get; set; }
    public string? SerieGuia { get; set; }
}
