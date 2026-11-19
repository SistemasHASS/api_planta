namespace Planta.Domain.Dtos;

/// <summary>
/// Cuerpo de api/administracion/kilos-pago/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ formatoId, tipoEmpaqueGrupoId, kilosPago }]. Crea la combinación o actualiza sus kilos; el SP guarda también id, código y nombre del tipo de empaque agrupador (no hay tipo de empaque, categoría ni calibre). Solo con el parámetro Flujo_Empaque activo.
/// </summary>
public sealed class SincronizarKilosPagoRequest
{
    public string Json { get; set; } = "[]";
}
