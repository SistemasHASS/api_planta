namespace Planta.Domain.Dtos;

/// <summary>
/// Usuario tal como lo devuelve apimaestra (get-usuarios). Se reenvia como JSON a PLANTA_GetUsuarioAcopio,
/// que lee estos nombres exactos ($.Idempresa, $.Documentoidentidad, $.Idrol...), por eso NO se renombran
/// ni se les aplica camelCase al serializarlos.
/// </summary>
public sealed class UsuarioMaestrosDto
{
    public string Id { get; set; } = "";
    public int Sociedad { get; set; }
    public string Idempresa { get; set; } = "";
    public string Ruc { get; set; } = "";
    public string RazonSocial { get; set; } = "";
    public string Proyecto { get; set; } = "";
    public string Documentoidentidad { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Usuario { get; set; } = "";
    public string Idrol { get; set; } = "";
    public string Rol { get; set; } = "";
    public string Aplicacion { get; set; } = "";
    public int Modotrabajo { get; set; }
    public string Fechacompensacion { get; set; } = "";
}
