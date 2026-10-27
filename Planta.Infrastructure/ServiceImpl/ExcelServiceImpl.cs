using System.Drawing;
using System.Text.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>Excel con EPPlus (licencia no comercial). Encabezado azul con letras blancas, filtro y columnas autoajustadas.</summary>
public sealed class ExcelServiceImpl : IExcelService
{
    /// <summary>Columnas que se escriben como numero (y su formato). El resto va como texto.</summary>
    private static readonly Dictionary<string, string> ColumnasNumericas = new()
    {
        ["SOBRE PESO"] = "0.00",
        ["JARRA BLANCA"] = "#,##0",
        ["JARRA VERDE"] = "#,##0",
        ["DESCARTE"] = "#,##0",
        ["TOTAL"] = "#,##0",
        ["% DESCARTE"] = "0.0",
        ["DÍAS"] = "0",
        ["VIAJES"] = "0",
        ["COSECHADORES"] = "0",
        ["PROMEDIO"] = "0.0",
    };

    static ExcelServiceImpl()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public byte[] GenerarTabla(string nombreHoja, JsonElement filas)
    {
        if (filas.ValueKind != JsonValueKind.Array || filas.GetArrayLength() == 0)
        {
            throw new ArgumentException("No hay filas para exportar.", nameof(filas));
        }

        using var paquete = new ExcelPackage();
        var hoja = paquete.Workbook.Worksheets.Add(nombreHoja);

        var columnas = filas[0].EnumerateObject().Select(p => p.Name).ToList();

        for (var c = 0; c < columnas.Count; c++)
        {
            var celda = hoja.Cells[1, c + 1];
            celda.Value = columnas[c];
            celda.Style.Font.Bold = true;
            celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
            celda.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 121));
            celda.Style.Font.Color.SetColor(Color.White);
            celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        var fila = 2;
        foreach (var registro in filas.EnumerateArray())
        {
            for (var c = 0; c < columnas.Count; c++)
            {
                var celda = hoja.Cells[fila, c + 1];
                registro.TryGetProperty(columnas[c], out var valor);

                if (ColumnasNumericas.TryGetValue(columnas[c], out var formato) && valor.ValueKind == JsonValueKind.Number && valor.TryGetDecimal(out var numero))
                {
                    celda.Value = numero;
                    celda.Style.Numberformat.Format = formato;
                }
                else
                {
                    celda.Value = valor.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null ? string.Empty : valor.ToString();
                }
            }

            fila++;
        }

        var rango = hoja.Cells[1, 1, fila - 1, columnas.Count];
        rango.AutoFilter = true;
        rango.AutoFitColumns();

        return paquete.GetAsByteArray();
    }
}
