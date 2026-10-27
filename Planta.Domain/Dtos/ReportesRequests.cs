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

/// <summary>
/// Cuerpo de api/reportes/reporte-avance-cosechador. El RUC es el de la empresa elegida en el select (viene de apimaestra, no del token).
/// Los filtros opcionales vacios se envian como NULL al SP.
/// </summary>
public sealed class ReporteAvanceCosechadorRequest
{
    public string Ruc { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd.</summary>
    public string FechaDesde { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd. Vacio = hasta la fecha desde.</summary>
    public string FechaHasta { get; set; } = "";
    public string DniSupervisor { get; set; } = "";
    /// <summary>Grupos elegidos. Vacio = todos.</summary>
    public List<string> Grupos { get; set; } = [];
    public string CodFundo { get; set; } = "";
    /// <summary>DNI de los cosechadores elegidos. Vacio = todos.</summary>
    public List<string> DniCosechadores { get; set; } = [];
}

/// <summary>Cuerpo de api/reportes/reporte-avance-grupo. El RUC es el de la empresa elegida en el select (viene de apimaestra, no del token).</summary>
public sealed class ReporteAvanceGrupoRequest
{
    public string Ruc { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd.</summary>
    public string FechaDesde { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd. Vacio = hasta la fecha desde.</summary>
    public string FechaHasta { get; set; } = "";
    /// <summary>Grupos elegidos. Vacio = todos.</summary>
    public List<string> Grupos { get; set; } = [];
    /// <summary>DNI de los supervisores elegidos. Vacio = todos.</summary>
    public List<string> DniSupervisores { get; set; } = [];
}

/// <summary>Cuerpo de api/reportes/reporte-avance-cosechador/filtros: opciones de grupo y cosechador para el rango de fechas.</summary>
public sealed class ReporteAvanceCosechadorFiltrosRequest
{
    public string Ruc { get; set; } = "";
    public string IdProyecto { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd.</summary>
    public string FechaDesde { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd. Vacio = hasta la fecha desde.</summary>
    public string FechaHasta { get; set; } = "";
    /// <summary>Si se envian, los cosechadores se limitan a esos grupos.</summary>
    public List<string> Grupos { get; set; } = [];
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
