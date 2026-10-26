namespace Planta.Domain.Constants;

/// <summary>
/// Nombres de los claims propios que lleva el JWT. Se escriben aqui una sola vez para que
/// quien crea el token (Infrastructure) y quien lo lee (Api) usen exactamente el mismo texto.
/// </summary>
public static class ClaimNames
{
    public const string NombreCompleto = "NombreCompleto";
    public const string DocumentoIdentidad = "DocumentoIdentidad";
    public const string IdEmpresa = "IdEmpresa";
    public const string Ruc = "Ruc";
    public const string IdProyecto = "IdProyecto";
    public const string SesionExpiraUtc = "SesionExpiraUtc";
    public const string Aplicacion = "Aplicacion";
    public const string CodigoAcopio = "CodigoAcopio";
    public const string AcopioNombre = "AcopioNombre";
    public const string SerieGuia = "SerieGuia";
}
