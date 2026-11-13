namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/reportes/historico-empacador/listar: los empacadores asignados a un puesto en un rango de fechas. La empresa sale del token.</summary>
public sealed class HistoricoEmpacadoresRequest
{
    /// <summary>AAAA-MM-DD, fecha del proceso (inclusive).</summary>
    public string FechaDesde { get; set; } = string.Empty;

    /// <summary>AAAA-MM-DD, fecha del proceso (inclusive).</summary>
    public string FechaHasta { get; set; } = string.Empty;

    /// <summary>Vacío = todas las plantas; admite varias separadas por coma (el anotador siempre usa el de su token).</summary>
    public string? CodigoAcopio { get; set; }
}

/// <summary>Cuerpo de api/reportes/historico-empacador/detalle: las asignaciones de un empacador en el mismo rango.</summary>
public sealed class HistoricoEmpacadorDetalleRequest
{
    public string NumeroDocumento { get; set; } = string.Empty;
    public string FechaDesde { get; set; } = string.Empty;
    public string FechaHasta { get; set; } = string.Empty;
    public string? CodigoAcopio { get; set; }
}
