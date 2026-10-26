namespace Planta.Domain.Constants;

/// <summary>
/// Codigos de rol tal como los devuelve apimaestra en el campo IdRol (aplicacion PLANTA).
/// Se usan en [Authorize(Roles = Roles.Admin)].
/// </summary>
public static class Roles
{
    public const string Admin = "ADPLA";
    public const string Logistica = "LOPLA";
    public const string Monitor = "MOPLA";
    public const string GuiasManuales = "GMPLA";
    public const string Coordinacion = "COPLA";
    public const string Operaciones = "OPPLA";
}
