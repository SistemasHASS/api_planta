using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.UseCase;

/// <summary>Jornadas del anotador. Empresa, RUC, usuario, rol y acopio salen del token.</summary>
public interface IJornadasAnotadorUseCase
{
    Task<JsonElement> ListarAsync(ListarJornadasAnotadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Abre (modo nuevo) o cierra (modo editado) la jornada del anotador.</summary>
    Task<JsonElement> SincronizarAsync(SincronizarJornadaAnotadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Líneas del acopio del anotador con su situación (libre / mía / ocupada) en el proceso de su jornada abierta.</summary>
    Task<JsonElement> ListarLineasAsync(CancellationToken cancellationToken = default);

    /// <summary>Toma (modo nuevo) o suelta (modo editado) una línea.</summary>
    Task<JsonElement> SincronizarLineasAsync(SincronizarJornadaAnotadorLineaRequest request, CancellationToken cancellationToken = default);

    /// <summary>Busca un trabajador activo de la empresa del token por su número de documento.</summary>
    Task<JsonElement> BuscarTrabajadorAsync(BuscarTrabajadorRequest request, CancellationToken cancellationToken = default);

    /// <summary>Empacadores asignados a los puestos de las líneas del acopio en la jornada abierta.</summary>
    Task<JsonElement> ListarPuestosAsync(CancellationToken cancellationToken = default);

    /// <summary>Asigna (nuevo), mueve (mover) o quita (editado) un empacador de un puesto.</summary>
    Task<JsonElement> SincronizarPuestosAsync(SincronizarJornadaAnotadorPuestoRequest request, CancellationToken cancellationToken = default);

    /// <summary>Tipos de empaque que la matriz de compatibilidad asocia al formato.</summary>
    Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ListarTiposEmpaquePorMatrizRequest request, CancellationToken cancellationToken = default);

    /// <summary>Registra las cajas de un empacador en la recolección abierta del anotador.</summary>
    Task<JsonElement> SincronizarRecoleccionesCajasAsync(SincronizarRecoleccionCajaRequest request, CancellationToken cancellationToken = default);

    /// <summary>Recolecciones de cajas de la jornada abierta del acopio.</summary>
    Task<JsonElement> ListarRecoleccionesAsync(CancellationToken cancellationToken = default);

    /// <summary>Historial de recolecciones del anotador, o el detalle de una (IdRecoleccion &gt; 0).</summary>
    Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ListarRecoleccionesCajasHistorialRequest request, CancellationToken cancellationToken = default);

    /// <summary>Cajas registradas por empacador en la recolección abierta del anotador.</summary>
    Task<JsonElement> ListarRecoleccionesCajasResumenAsync(CancellationToken cancellationToken = default);

    /// <summary>Inicia (nuevo) o cierra (editado) una recolección del anotador.</summary>
    Task<JsonElement> SincronizarRecoleccionesAsync(SincronizarJornadaAnotadorRecoleccionRequest request, CancellationToken cancellationToken = default);
}
