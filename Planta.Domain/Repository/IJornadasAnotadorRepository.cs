using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Repository;

/// <summary>Jornadas del anotador (SP PLANTA_ListarJornadasAnotador y PLANTA_SincronizarJornadasAnotador). Devuelven { error, data, mensaje }.</summary>
public interface IJornadasAnotadorRepository
{
    Task<JsonElement> ListarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
    Task<JsonElement> SincronizarAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Líneas manuales y curos de máquinas del acopio con su situación (libre / mía / ocupada) en el proceso de la jornada abierta (PLANTA_ListarJornadasAnotadorLineas; el SP decide si trae las máquinas según Flujo_Empaque).</summary>
    Task<JsonElement> ListarLineasAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);


    /// <summary>Toma o suelta líneas (PLANTA_SincronizarJornadasAnotadorLineas).</summary>
    Task<JsonElement> SincronizarLineasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Busca un trabajador activo por RUC y número de documento (PLANTA_BuscarTrabajador).</summary>
    Task<JsonElement> BuscarTrabajadorAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Empacadores asignados a los puestos de las líneas del acopio en la jornada abierta (PLANTA_ListarJornadasAnotadorPuestos).</summary>
    Task<JsonElement> ListarPuestosAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Asigna, mueve o quita empacadores de los puestos (PLANTA_SincronizarJornadasAnotadorPuestos).</summary>
    Task<JsonElement> SincronizarPuestosAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Tipos de empaque asociados al formato en la matriz de compatibilidad (PLANTA_ListarTiposEmpaquePorMatriz).</summary>
    Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Recolecciones de cajas de la jornada abierta del acopio (PLANTA_ListarJornadasAnotadorRecolecciones).</summary>
    Task<JsonElement> ListarRecoleccionesAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Historial de recolecciones del usuario o detalle de una (PLANTA_ListarRecoleccionesCajasHistorial).</summary>
    Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Movimientos de stock que registró el usuario del token en un día con su resumen por tipo (PLANTA_ListarMovimientosStock).</summary>
    Task<JsonElement> ListarMovimientosStockAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Cajas por empacador en la recolección abierta del usuario (PLANTA_ListarRecoleccionesCajasResumen).</summary>
    Task<JsonElement> ListarRecoleccionesCajasResumenAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Stock actual del anotador: sus cajas que Logística aún no consumió en palets, por formato y tipo de empaque (PLANTA_ListarStockAnotador).</summary>
    Task<JsonElement> ListarStockAnotadorAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Producción del día: cajas de todos los empacadores en el proceso de la jornada abierta, una fila por anotación (PLANTA_ListarProduccionDia).</summary>
    Task<JsonElement> ListarProduccionDiaAsync(ContextoUsuario contexto, CancellationToken cancellationToken = default);

    /// <summary>Corrige la cantidad de un registro de cajas con su AJUSTE de stock (PLANTA_CorregirCajasRecoleccion).</summary>
    Task<JsonElement> CorregirCajasRecoleccionAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Registra las cajas de un empacador en la recolección abierta (PLANTA_SincronizarRecoleccionesCajas).</summary>
    Task<JsonElement> SincronizarRecoleccionesCajasAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);

    /// <summary>Inicia o cierra una recolección del anotador (PLANTA_SincronizarJornadasAnotadorRecolecciones).</summary>
    Task<JsonElement> SincronizarRecoleccionesAsync(ContextoUsuario contexto, string json, CancellationToken cancellationToken = default);
}
