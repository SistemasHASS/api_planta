namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/administracion/acopio-maquinas/listar: acopio del que se piden las máquinas (activas e inactivas). La empresa y el RUC salen del token.</summary>
public sealed class ListarAcopioMaquinasRequest
{
    public string CodigoAcopio { get; set; } = "";
}

/// <summary>
/// Cuerpo de api/administracion/acopio-maquinas/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "nuevo" | "editado", codigoAcopio, codigo, nombre, cantidadCuros, activo }].
/// Cada curo tiene 8 puestos (se calculan). El código no se cambia al editar. Activar o desactivar = activo. Las máquinas no se eliminan.
/// </summary>
public sealed class SincronizarAcopioMaquinasRequest
{
    public string Json { get; set; } = "[]";
}
