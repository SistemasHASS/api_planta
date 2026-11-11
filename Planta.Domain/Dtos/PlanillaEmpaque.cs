namespace Planta.Domain.Dtos;

/// <summary>
/// Todo lo que la planilla de empaque (el Excel con el formato de la empresa: logo, cabecera, 21 personas por hoja) necesita para armarse.
/// La usan dos informes: la planilla de producción (cajas por recolección) y el tareo / asistencia (la misma lista de personas, sin cajas).
/// </summary>
public sealed class PlanillaEmpaque
{
    /// <summary>Código y nombre de la planta, como sale en PLANTA EMPACADORA.</summary>
    public string Acopio { get; init; } = string.Empty;

    /// <summary>DD/MM/AAAA, la del proceso.</summary>
    public string Fecha { get; init; } = string.Empty;

    public string Turno { get; init; } = string.Empty;

    /// <summary>El que se eligió al abrir el proceso; vacío si no hay ninguno (sale "PROYECTO:" a secas, para rellenar a mano).</summary>
    public string Supervisor { get; init; } = string.Empty;

    /// <summary>Logo de la empresa (wwwroot/assets/logos/{RUC}.jpg); null = la planilla sale sin logo.</summary>
    public byte[]? Logo { get; set; }

    public List<PaginaPlanilla> Paginas { get; } = [];
}

/// <summary>Una hoja del Excel. Cada hoja lleva abajo al anotador que registró a esas personas.</summary>
public sealed record PaginaPlanilla
{
    /// <summary>Nombre de la hoja (máx. 31 caracteres; quien la arma ya lo dejó válido y sin repetir).</summary>
    public string Hoja { get; init; } = string.Empty;

    /// <summary>Si no está vacía reemplaza a VARIEDAD tal cual (la usa el tareo, que no tiene variedad).</summary>
    public string Cabecera { get; init; } = string.Empty;

    /// <summary>Formato, tipo de empaque y peso por caja: "4.4 oz Jumbo 0.125kg".</summary>
    public string Variedad { get; init; } = string.Empty;

    /// <summary>La máquina de donde vienen las cajas; vacío en las líneas manuales.</summary>
    public string Lote { get; init; } = string.Empty;

    /// <summary>Nombre del anotador (va junto a SUPERVISOR, en el pie).</summary>
    public string Anotador { get; init; } = string.Empty;

    /// <summary>Su documento (va junto a CODIGO, en el pie).</summary>
    public string AnotadorDni { get; init; } = string.Empty;

    /// <summary>Número de la primera columna de recolección de esta hoja: 1, 21, 41...</summary>
    public int InicioRecoleccion { get; init; } = 1;

    /// <summary>Número de la primera persona de esta hoja (CANT.): 1, 22, 43...</summary>
    public int IndiceInicio { get; init; } = 1;

    public IReadOnlyList<PersonaPlanilla> Personas { get; init; } = [];
}

/// <summary>Una fila de la hoja. Recolecciones guarda las cajas por número de columna (1..n, ya sin huecos); en el tareo va vacío.</summary>
public sealed record PersonaPlanilla(string Dni, string Nombre, IReadOnlyDictionary<int, int> Recolecciones);
