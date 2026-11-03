namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/administracion/acopio-lineas/listar: acopio del que se piden las líneas (activas e inactivas). La empresa y el RUC salen del token.</summary>
public sealed class ListarAcopioLineasRequest
{
    public string CodigoAcopio { get; set; } = "";
}

/// <summary>
/// Cuerpo de api/administracion/acopio-lineas/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "nuevo" | "editado", codigoAcopio, codigoTipoProcesoEmpacado, cantidadPuestos, activo }].
/// El número de la línea lo asigna el SP. Activar o desactivar = activo. Las líneas no se eliminan.
/// </summary>
public sealed class SincronizarAcopioLineasRequest
{
    public string Json { get; set; } = "[]";
}
