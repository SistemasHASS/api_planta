namespace Planta.Domain.Dtos;

/// <summary>
/// Cuerpo de api/administracion/kilos-pago/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ formatoId, tipoEmpaqueId, categoriaId, kilosPago, calibreDescripcion? }]. Crea la combinación o actualiza sus kilos; el SP guarda también id, código y descripción del tipo de empaque, la categoría y el calibre (la descripción del calibre, de la apimaestra, viene en el JSON). Solo con el parámetro Flujo_Empaque activo.
/// </summary>
public sealed class SincronizarKilosPagoRequest
{
    public string Json { get; set; } = "[]";
}
