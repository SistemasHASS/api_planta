using System.Text.Json;

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
}
