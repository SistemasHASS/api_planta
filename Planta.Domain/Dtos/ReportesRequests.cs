namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/reportes/reporte-diario.</summary>
public sealed class ReporteDiarioRequest
{
    /// <summary>Fecha del reporte, formato yyyy-MM-dd.</summary>
    public string Fecha { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    /// <summary>Codigos de acopio separados por coma. Vacio = todos los acopios de la campania.</summary>
    public string Acopios { get; set; } = "";
}

/// <summary>Cuerpo de api/reportes/reporte-semanal. Cada lista vacia significa "todos".</summary>
public sealed class ReporteSemanalRequest
{
    public string IdProyecto { get; set; } = "";
    public List<string> Semanas { get; set; } = [];
    public List<string> Variedades { get; set; } = [];
    public List<string> Formatos { get; set; } = [];
    public List<string> Destinos { get; set; } = [];
    public List<string> Clientes { get; set; } = [];
    public List<string> Consignatarios { get; set; } = [];
}

/// <summary>Parametros del SP PLANTA_ReporteSemanal_Datos: cada filtro es un arreglo JSON, o null para "todos".</summary>
public sealed record ReporteSemanalParametros(
    string IdProyecto,
    string? Semanas,
    string? Variedades,
    string? Formatos,
    string? Destinos,
    string? Clientes,
    string? Consignatarios);

/// <summary>Cuerpo de api/reportes/reporte-kg-acopio/campanias.</summary>
public sealed class RucRequest
{
    public string Ruc { get; set; } = "";
}

/// <summary>Cuerpo de api/reportes/reporte-kg-acopio. El RUC es el de la empresa a consultar (la propia o la externa autorizada).</summary>
public sealed class ReporteKgAcopioRequest
{
    public string Ruc { get; set; } = "";
    /// <summary>Fecha del reporte, formato yyyy-MM-dd.</summary>
    public string Fecha { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    public string CodFundo { get; set; } = "";
    /// <summary>Codigos de acopio separados por coma. Vacio = todos.</summary>
    public string Acopios { get; set; } = "";
}

/// <summary>Cuerpo de api/reportes/reporte-diario-propio-externo.</summary>
public sealed class ReporteDiarioPropioExternoRequest
{
    /// <summary>Fecha del reporte, formato yyyy-MM-dd.</summary>
    public string Fecha { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    /// <summary>Codigos de acopio separados por coma. Vacio = todos.</summary>
    public string Acopios { get; set; } = "";
    /// <summary>Origenes separados por coma: BH (propio) y/o CAO (externo). Vacio = ambos.</summary>
    public string Origenes { get; set; } = "";
}
