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

    private static readonly Color Azul = Color.FromArgb(31, 78, 121);
    private static readonly Color Ladrillo = Color.FromArgb(156, 66, 33);

    private const int ColumnasFijas = 5; // FECHA, PLANTA, ANOTADOR, DNI, EMPACADOR

    public byte[] GenerarControlInterno(JsonElement data, string desde, string hasta, string planta, string generadoPor)
    {
        var informe = ControlInternoInforme.Armar(data);
        if (informe.Filas.Count == 0)
        {
            throw new ArgumentException("No hay filas para exportar.", nameof(data));
        }

        var columnas = informe.Columnas;
        var filas = informe.Filas;
        var colTotalCajas = ColumnasFijas + (columnas.Count * 2) + 1;
        var colTotalKilos = colTotalCajas + 1;
        var colDe = (int i) => ColumnasFijas + (i * 2) + 1;
        var empacadores = filas.Select(f => f.Dni).Distinct().Count();

        using var paquete = new ExcelPackage();
        var hoja = paquete.Workbook.Worksheets.Add("Control Interno");

        hoja.Cells[1, 1].Value = "EXCEL DE CONTROL INTERNO";
        hoja.Cells[1, 1].Style.Font.Bold = true;
        hoja.Cells[1, 1].Style.Font.Size = 15;
        hoja.Cells[2, 1].Value = $"Del {FechaCorta(desde)} al {FechaCorta(hasta)}  ·  {planta}";
        hoja.Cells[2, 1].Style.Font.Color.SetColor(Color.FromArgb(85, 85, 85));
        hoja.Cells[3, 1].Value = $"Generado por {generadoPor} · {filas.Count} fila(s) · {empacadores} empacador(es) con movimiento";
        hoja.Cells[3, 1].Style.Font.Italic = true;
        hoja.Cells[3, 1].Style.Font.Color.SetColor(Color.FromArgb(119, 119, 119));

        // Cabecera en dos filas: arriba el producto (abarcando sus dos columnas) y debajo CAJAS y KG.
        const int filaProductos = 5;
        const int filaMedidas = 6;
        var titulosFijos = new[] { ("FECHA", 14), ("PLANTA", 12), ("ANOTADOR", 34), ("DNI", 14), ("EMPACADOR", 34) };

        void PintarCabecera(ExcelRange rango, Color color)
        {
            rango.Style.Font.Bold = true;
            rango.Style.Font.Size = 10;
            rango.Style.Font.Color.SetColor(Color.White);
            rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
            rango.Style.Fill.BackgroundColor.SetColor(color);
            rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            rango.Style.WrapText = true;
            rango.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(155, 183, 212));
        }

        for (var i = 0; i < titulosFijos.Length; i++)
        {
            var rango = hoja.Cells[filaProductos, i + 1, filaMedidas, i + 1];
            rango.Merge = true;
            hoja.Cells[filaProductos, i + 1].Value = titulosFijos[i].Item1;
            PintarCabecera(rango, Azul);
            hoja.Column(i + 1).Width = titulosFijos[i].Item2;
        }

        for (var i = 0; i < columnas.Count; i++)
        {
            var c = columnas[i];
            var col = colDe(i);
            var color = c.FueraDeMatriz ? Ladrillo : Azul;

            var producto = hoja.Cells[filaProductos, col, filaProductos, col + 1];
            producto.Merge = true;
            var categoriaCalibre = string.Join(" · ", new[] { c.Categoria, c.Calibre }.Where(t => t.Length > 0));
            hoja.Cells[filaProductos, col].Value = string.Join("\n", new[] { c.Formato.ToUpperInvariant(), c.TipoEmpaque.ToUpperInvariant(), categoriaCalibre.ToUpperInvariant() }.Where(t => t.Length > 0));
            PintarCabecera(producto, color);

            hoja.Cells[filaMedidas, col].Value = "CAJAS";
            hoja.Cells[filaMedidas, col + 1].Value = "KG";
            PintarCabecera(hoja.Cells[filaMedidas, col, filaMedidas, col + 1], color);

            hoja.Column(col).Width = 10;
            hoja.Column(col + 1).Width = 11;
        }

        hoja.Cells[filaProductos, colTotalCajas, filaMedidas, colTotalCajas].Merge = true;
        hoja.Cells[filaProductos, colTotalCajas].Value = "TOTAL\nCAJAS";
        PintarCabecera(hoja.Cells[filaProductos, colTotalCajas, filaMedidas, colTotalCajas], Azul);
        hoja.Cells[filaProductos, colTotalKilos, filaMedidas, colTotalKilos].Merge = true;
        hoja.Cells[filaProductos, colTotalKilos].Value = "TOTAL\nKG DE PAGO";
        PintarCabecera(hoja.Cells[filaProductos, colTotalKilos, filaMedidas, colTotalKilos], Azul);
        hoja.Column(colTotalCajas).Width = 13;
        hoja.Column(colTotalKilos).Width = 16;
        hoja.Row(filaProductos).Height = 56;
        hoja.Row(filaMedidas).Height = 18;

        // Los datos: cero donde no trabajó ese producto, para comparar periodos columna a columna.
        var fila = filaMedidas + 1;
        foreach (var f in filas)
        {
            hoja.Cells[fila, 1].Value = FechaCorta(f.Fecha);
            hoja.Cells[fila, 2].Value = f.Acopio;
            hoja.Cells[fila, 3].Value = string.Join(", ", f.Anotadores);
            hoja.Cells[fila, 4].Value = f.Dni;
            hoja.Cells[fila, 5].Value = f.Empacador;

            for (var i = 0; i < columnas.Count; i++)
            {
                var clave = columnas[i].Clave;
                var cajas = f.Cajas.GetValueOrDefault(clave);
                var kilos = Math.Round(f.Kilos.GetValueOrDefault(clave), 2);
                hoja.Cells[fila, colDe(i)].Value = cajas;
                hoja.Cells[fila, colDe(i) + 1].Value = kilos;
                if (cajas == 0)
                {
                    hoja.Cells[fila, colDe(i), fila, colDe(i) + 1].Style.Font.Color.SetColor(Color.FromArgb(191, 191, 191));
                }
            }

            hoja.Cells[fila, colTotalCajas].Value = f.Total;
            hoja.Cells[fila, colTotalKilos].Value = Math.Round(f.TotalKilos, 2);
            hoja.Cells[fila, colTotalCajas, fila, colTotalKilos].Style.Font.Bold = true;
            fila++;
        }

        // Fila de totales.
        hoja.Cells[fila, 1].Value = "TOTALES";
        hoja.Cells[fila, 5].Value = $"{filas.Count} fila(s) · {empacadores} empacador(es)";
        for (var i = 0; i < columnas.Count; i++)
        {
            var clave = columnas[i].Clave;
            hoja.Cells[fila, colDe(i)].Value = filas.Sum(f => f.Cajas.GetValueOrDefault(clave));
            hoja.Cells[fila, colDe(i) + 1].Value = Math.Round(filas.Sum(f => f.Kilos.GetValueOrDefault(clave)), 2);
        }

        hoja.Cells[fila, colTotalCajas].Value = filas.Sum(f => f.Total);
        hoja.Cells[fila, colTotalKilos].Value = Math.Round(filas.Sum(f => f.TotalKilos), 2);
        var totales = hoja.Cells[fila, 1, fila, colTotalKilos];
        totales.Style.Font.Bold = true;
        totales.Style.Fill.PatternType = ExcelFillStyle.Solid;
        totales.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(237, 242, 248));
        totales.Style.Border.Top.Style = ExcelBorderStyle.Double;
        totales.Style.Border.Top.Color.SetColor(Azul);

        // Cajas enteras y kilos con dos decimales (los productos alternan cajas y kilos).
        for (var i = 0; i < columnas.Count; i++)
        {
            hoja.Cells[filaMedidas + 1, colDe(i), fila, colDe(i)].Style.Numberformat.Format = "#,##0";
            hoja.Cells[filaMedidas + 1, colDe(i) + 1, fila, colDe(i) + 1].Style.Numberformat.Format = "#,##0.00";
        }

        hoja.Cells[filaMedidas + 1, colTotalCajas, fila, colTotalCajas].Style.Numberformat.Format = "#,##0";
        hoja.Cells[filaMedidas + 1, colTotalKilos, fila, colTotalKilos].Style.Numberformat.Format = "#,##0.00";
        hoja.Cells[filaMedidas + 1, ColumnasFijas + 1, fila, colTotalKilos].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        // El filtro va en la fila de CAJAS/KG: Excel no admite autofiltro sobre celdas combinadas.
        hoja.Cells[filaMedidas, 1, fila - 1, colTotalKilos].AutoFilter = true;
        hoja.View.FreezePanes(filaMedidas + 1, ColumnasFijas + 1);

        return paquete.GetAsByteArray();
    }

    /// <summary>AAAA-MM-DD -> DD/MM/AAAA</summary>
    private static string FechaCorta(string valor)
    {
        var t = (valor ?? string.Empty).Trim();
        if (t.Length >= 10)
        {
            var partes = t[..10].Split('-');
            if (partes.Length == 3) return $"{partes[2]}/{partes[1]}/{partes[0]}";
        }

        return t;
    }
}
