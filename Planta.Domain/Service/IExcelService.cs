using System.Text.Json;
using Planta.Domain.Dtos;

namespace Planta.Domain.Service;

public interface IExcelService
{
    /// <summary>
    /// Arma un Excel con una hoja. <paramref name="filas"/> es un arreglo JSON de objetos: las columnas salen de las
    /// propiedades del primer objeto. La columna "SOBRE PESO" se escribe como numero con dos decimales.
    /// </summary>
    byte[] GenerarTabla(string nombreHoja, JsonElement filas);

    /// <summary>
    /// Excel de control interno: <paramref name="data"/> es el data de PLANTA_ListarControlInterno ({ columnas, detalle }). Una fila por jornada y empacador;
    /// cada producto ocupa dos columnas (cajas y kilos de pago) y hay una fila de totales.
    /// </summary>
    byte[] GenerarControlInterno(JsonElement data, string desde, string hasta, string planta, string generadoPor);

    /// <summary>
    /// Excel de la planilla de empaque con el formato de la empresa (logo, cabecera, 21 personas por hoja): una hoja por cada página de <paramref name="planilla"/>.
    /// Sirve para la planilla de producción (con cajas por recolección) y para el tareo (sin cajas).
    /// </summary>
    byte[] GenerarPlanillaEmpaque(PlanillaEmpaque planilla);
}
