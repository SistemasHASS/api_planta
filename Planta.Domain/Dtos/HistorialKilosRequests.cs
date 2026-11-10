namespace Planta.Domain.Dtos;

/// <summary>
/// Cuerpo de api/administracion/historial-kilos/listar. Fecha como AAAA-MM-DD (fecha de apertura de la jornada); CodigoAcopio vacío = todos los acopios
/// (solo el administrador: el anotador siempre ve su propio acopio, el del token). La empresa sale del token. Solo con el parámetro Flujo_Empaque activo.
/// </summary>
public sealed class HistorialKilosRequest
{
    public string Fecha { get; set; } = string.Empty;
    public string? CodigoAcopio { get; set; }
}
