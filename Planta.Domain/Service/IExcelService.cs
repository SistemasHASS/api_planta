using System.Text.Json;

namespace Planta.Domain.Service;

public interface IExcelService
{
    /// <summary>
    /// Arma un Excel con una hoja. <paramref name="filas"/> es un arreglo JSON de objetos: las columnas salen de las
    /// propiedades del primer objeto. La columna "SOBRE PESO" se escribe como numero con dos decimales.
    /// </summary>
    byte[] GenerarTabla(string nombreHoja, JsonElement filas);
}
