using System.Text.Json;

namespace Planta.Domain.UseCase;

/// <summary>Flujo del anotador. Recibe el JSON del cliente tal cual (arreglo de un elemento) y lo pasa al SP; empresa, RUC, usuario, rol, acopio y nombre salen del token.</summary>
public interface IJornadasAnotadorUseCase
{
    /// <summary>Jornada abierta del anotador (o todas las del acopio si soloAbierta es false).</summary>
    Task<JsonElement> ListarAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Abre (modo nuevo) o cierra (modo editado) la jornada del acopio.</summary>
    Task<JsonElement> SincronizarAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Líneas del acopio del anotador con su situación (libre / mía / ocupada) en el proceso de su jornada abierta.</summary>
    Task<JsonElement> ListarLineasAsync(CancellationToken cancellationToken = default);

    /// <summary>Toma (modo nuevo) o suelta (modo editado) una línea.</summary>
    Task<JsonElement> SincronizarLineasAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Busca un trabajador activo de la empresa del token por su número de documento.</summary>
    Task<JsonElement> BuscarTrabajadorAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Empacadores asignados a los puestos de las líneas del acopio en la jornada abierta.</summary>
    Task<JsonElement> ListarPuestosAsync(CancellationToken cancellationToken = default);

    /// <summary>Asigna (nuevo), mueve (mover), quita (editado) o vacía (vaciar) los empacadores de los puestos.</summary>
    Task<JsonElement> SincronizarPuestosAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Tipos de empaque que la matriz de compatibilidad asocia al formato.</summary>
    Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Registra (nuevo) o anula las cajas de un empacador en la recolección abierta del anotador.</summary>
    Task<JsonElement> SincronizarRecoleccionesCajasAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Recolecciones de cajas de la jornada abierta del acopio.</summary>
    Task<JsonElement> ListarRecoleccionesAsync(CancellationToken cancellationToken = default);

    /// <summary>Historial de recolecciones del anotador, o el detalle de una (idRecoleccion &gt; 0).</summary>
    Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Movimientos de stock de las cajas del anotador en un día (entradas, correcciones, ajustes y salidas a palet) con su resumen.</summary>
    Task<JsonElement> ListarMovimientosStockAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Cajas registradas por empacador en la recolección abierta del anotador.</summary>
    Task<JsonElement> ListarRecoleccionesCajasResumenAsync(CancellationToken cancellationToken = default);

    /// <summary>Stock actual del anotador (sus cajas que Logística aún no consumió en palets).</summary>
    Task<JsonElement> ListarStockAnotadorAsync(CancellationToken cancellationToken = default);

    /// <summary>Producción del día: cajas de todos los empacadores en el proceso de la jornada abierta del acopio.</summary>
    Task<JsonElement> ListarProduccionDiaAsync(CancellationToken cancellationToken = default);

    /// <summary>Corrige la cantidad (y el producto) de una anotación de la producción del día, con su observación.</summary>
    Task<JsonElement> CorregirCajasRecoleccionAsync(string json, CancellationToken cancellationToken = default);

    /// <summary>Inicia (nuevo) o cierra (editado) una recolección del anotador.</summary>
    Task<JsonElement> SincronizarRecoleccionesAsync(string json, CancellationToken cancellationToken = default);
}
