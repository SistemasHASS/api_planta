namespace Planta.Domain.Dtos;

/// <summary>
/// Cuerpo de api/administracion/control-interno/listar y /exportar-excel. Las fechas van como AAAA-MM-DD (fecha de apertura de la jornada);
/// CodigoAcopio vacío = todas las plantas. La empresa sale del token. Solo con el parámetro Flujo_Empaque activo.
/// </summary>
public sealed class ControlInternoRequest
{
    public string FechaDesde { get; set; } = string.Empty;
    public string FechaHasta { get; set; } = string.Empty;
    public string? CodigoAcopio { get; set; }
}
