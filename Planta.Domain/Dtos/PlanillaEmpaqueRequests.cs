namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/reportes/planilla-empaque/procesos: los procesos de una fecha. La empresa sale del token.</summary>
public sealed class PlanillaEmpaqueProcesosRequest
{
    /// <summary>AAAA-MM-DD, la fecha del proceso.</summary>
    public string Fecha { get; set; } = string.Empty;

    /// <summary>Vacío = todas las plantas; admite varias separadas por coma.</summary>
    public string? CodigoAcopio { get; set; }
}

/// <summary>Cuerpo de api/reportes/planilla-empaque/anotadores: los anotadores de un proceso.</summary>
public sealed class PlanillaEmpaqueAnotadoresRequest
{
    public string CodigoAcopio { get; set; } = string.Empty;
    public string IdProceso { get; set; } = string.Empty;
}

/// <summary>Cuerpo de api/reportes/planilla-empaque/produccion y /tareo. IdUsuarioAnotador vacío = todos los anotadores del proceso; admite varios separados por coma.</summary>
public sealed class PlanillaEmpaqueRequest
{
    public string CodigoAcopio { get; set; } = string.Empty;
    public string IdProceso { get; set; } = string.Empty;
    public string? IdUsuarioAnotador { get; set; }
}
