namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Formato de la planilla de empaque (el que antes venía en la plantilla de Excel): fuentes, colores, bordes, celdas combinadas, anchos y alturas.
/// Equivale a planilla_empaque_formato.go de la API Go: las dos describen exactamente la misma hoja (columnas B..AA, filas 1..36).
/// </summary>
internal static class PlanillaEmpaqueFormato
{
    /// <summary>Un estilo de celda. Bordes lleva las letras de los lados con borde fino: L (izquierda), R (derecha), T (arriba), B (abajo).</summary>
    internal sealed record Estilo(string Fuente, float Tamano, bool Negrita, bool Cursiva, string ColorFuente, string Relleno, string Bordes, string Horizontal, string Vertical, bool Ajustar, int Rotacion, bool Reducir);

    /// <summary>Un estilo y las zonas (rangos A1) de la hoja que lo llevan.</summary>
    internal sealed record Zona(Estilo Estilo, string[] Rangos);

    internal const double AnchoColumnaPorDefecto = 10.77734375;
    internal const double AltoFilaPorDefecto = 13.2;
    internal const int Zoom = 30;

    internal static readonly Zona[] Zonas =
    [
        new(new Estilo("Arial Narrow", 34f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["B1"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "TB", "", "", false, 0, false),
            ["C1:U1", "C2", "F2:N4", "Q2:U4", "X3:Z4", "F12:Y12"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "RTB", "", "", false, 0, false),
            ["V1:V4", "D2", "O2:O4", "C3:C4", "AA3:AA4", "Z12:Z34"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["W1", "B2", "E2", "B4", "E4", "P4", "W4"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "T", "", "", false, 0, false),
            ["X1:Z1"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "RT", "", "", false, 0, false),
            ["AA1"]),
        new(new Estilo("Calibri", 14f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["P2"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "LB", "", "", false, 0, false),
            ["W2"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "B", "", "", false, 0, false),
            ["X2:Z2", "X9:AA9", "E36:H36"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "RB", "", "", false, 0, false),
            ["AA2"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "93CDDD", "LRTB", "center", "center", true, 0, false),
            ["B3", "E3", "P3", "W3"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "93CDDD", "LTB", "center", "center", true, 0, false),
            ["D3"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "", "LTB", "center", "center", true, 0, false),
            ["D4"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "", "", "center", "top", true, 0, false),
            ["B5:T5", "X5:AA5"]),
        new(new Estilo("Arial", 14f, true, false, "1F497D", "", "", "left", "top", true, 0, false),
            ["U5:W5"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "", "left", "", true, 0, false),
            ["B6:B7", "U6:U7", "B9:B10", "U9:U10"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "B", "", "", true, 0, false),
            ["D6"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "", "center", "top", true, 0, false),
            ["E6:T11"]),
        new(new Estilo("Arial", 26f, false, true, "000000", "FFFFFF", "", "left", "", true, 0, false),
            ["D7"]),
        new(new Estilo("Arial", 20f, true, false, "1F497D", "", "", "", "", false, 0, false),
            ["B8:C8"]),
        new(new Estilo("Arial", 20f, true, false, "1F497D", "", "T", "", "", false, 0, false),
            ["D8"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "", "left", "center", true, 0, false),
            ["U8:AA8", "U11:V11"]),
        new(new Estilo("Arial", 18f, true, false, "000000", "", "B", "center", "center", true, 0, false),
            ["W9"]),
        new(new Estilo("Arial", 20f, true, false, "1F497D", "", "", "left", "center", true, 0, false),
            ["B11:D11"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "T", "left", "center", true, 0, false),
            ["W11:AA11"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "LRTB", "center", "center", true, 90, false),
            ["B12"]),
        new(new Estilo("Arial", 20f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["C12"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "LRTB", "center", "center", false, 0, true),
            ["D12"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "93CDDD", "LRTB", "center", "center", true, 0, false),
            ["E12"]),
        new(new Estilo("Times New Roman", 18f, true, false, "1F497D", "93CDDD", "LRTB", "center", "top", true, 0, false),
            ["AA12"]),
        new(new Estilo("Times New Roman", 10f, false, false, "000000", "", "LRB", "", "", false, 0, false),
            ["B13:D13"]),
        new(new Estilo("Calibri", 18f, true, false, "1F497D", "", "LRTB", "center", "center", false, 0, true),
            ["E13:X13"]),
        new(new Estilo("Calibri", 16f, true, false, "1F497D", "", "LRTB", "center", "center", false, 0, true),
            ["Y13"]),
        new(new Estilo("Calibri", 16f, true, false, "1F497D", "", "LRTB", "", "top", false, 0, true),
            ["AA13"]),
        new(new Estilo("Calibri", 16f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["B14:B34"]),
        new(new Estilo("Tw Cen MT", 22f, false, true, "000000", "", "LTB", "center", "center", true, 0, false),
            ["C14:C15", "C18:C20", "C22", "C33:C34"]),
        new(new Estilo("Tw Cen MT", 22f, false, true, "000000", "", "LRTB", "left", "center", true, 0, false),
            ["D14:D34"]),
        new(new Estilo("Tw Cen MT", 22f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["E14:Y34"]),
        new(new Estilo("Calibri", 12f, true, false, "1F497D", "", "LRTB", "center", "center", true, 0, false),
            ["AA14:AA34"]),
        new(new Estilo("Tw Cen MT", 22f, false, false, "000000", "", "LTB", "center", "center", true, 0, false),
            ["C16", "C21", "C23:C32"]),
        new(new Estilo("Tw Cen MT", 22f, false, false, "000000", "", "LRTB", "center", "center", true, 0, false),
            ["C17"]),
        new(new Estilo("Times New Roman", 14f, false, false, "1F497D", "", "", "", "center", true, 0, false),
            ["W35:X35"]),
        new(new Estilo("Calibri", 14f, true, false, "1F497D", "", "", "center", "center", true, 0, false),
            ["Y35:AA35"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "", "center", "top", false, 0, false),
            ["B36"]),
        new(new Estilo("Baskerville Old Face", 24f, true, false, "000000", "", "B", "left", "top", false, 0, false),
            ["D36"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "B", "left", "top", false, 0, false),
            ["I36:N36", "Z36:AA36"]),
        new(new Estilo("Arial", 18f, true, false, "1F497D", "", "", "left", "top", false, 0, false),
            ["O36:V36", "Y36"]),
    ];

    /// <summary>Las celdas combinadas.</summary>
    internal static readonly string[] Combinadas = ["B1:V1", "W1:AA2", "B2:D2", "E2:O2", "P2:V2", "B3:C3", "E3:O3", "P3:V3", "W3:AA3", "B4:C4", "E4:O4", "P4:V4", "W4:AA4", "B6:C6", "U6:AA6", "B7:C7", "U7:AA7", "B9:D9", "U9:V9", "W9:AA9", "B10:D10", "U10:AA10", "B12:B13", "C12:C13", "D12:D13", "E12:Z12", "Y13:Z13", "Y14:Z14", "Y15:Z15", "Y16:Z16", "Y17:Z17", "Y18:Z18", "Y19:Z19", "Y20:Z20", "Y21:Z21", "Y22:Z22", "Y23:Z23", "Y24:Z24", "Y25:Z25", "Y26:Z26", "Y27:Z27", "Y28:Z28", "Y29:Z29", "Y30:Z30", "Y31:Z31", "Y32:Z32", "Y33:Z33", "Y34:Z34", "B36:C36", "D36:H36", "V36:X36"];

    /// <summary>Ancho (en caracteres) de las columnas, de la primera a la última (base 1).</summary>
    internal static readonly (int Desde, int Hasta, double Ancho)[] AnchosColumnas =
    [
        (1, 1, 34),
        (2, 2, 18.5547),
        (3, 3, 33.4414),
        (4, 4, 95.5547),
        (5, 24, 10.6641),
        (25, 26, 10.5),
        (27, 27, 35.6641),
        (29, 29, 3.10938),
    ];

    /// <summary>Alto (en puntos) de las filas que no usan el alto por defecto.</summary>
    internal static readonly Dictionary<int, double> AlturasFilas = new()
    {
        [1] = 79.5,
        [2] = 30.75,
        [3] = 34.5,
        [4] = 30,
        [5] = 18,
        [6] = 28.5,
        [7] = 28.5,
        [8] = 1.5,
        [9] = 28.5,
        [10] = 30.75,
        [11] = 9.75,
        [12] = 27.9,
        [13] = 37.5,
        [14] = 51.6,
        [15] = 51.6,
        [16] = 51.6,
        [17] = 51.6,
        [18] = 51.6,
        [19] = 51.6,
        [20] = 51.6,
        [21] = 51.6,
        [22] = 51.6,
        [23] = 51.6,
        [24] = 51.6,
        [25] = 51.6,
        [26] = 51.6,
        [27] = 51.6,
        [28] = 51.6,
        [29] = 51.6,
        [30] = 51.6,
        [31] = 51.6,
        [32] = 51.6,
        [33] = 51.6,
        [34] = 51.6,
        [35] = 18,
        [36] = 30.6,
    };

    /// <summary>Lo que no cambia de una planilla a otra (títulos, rótulos, razón social y RUC).</summary>
    internal static readonly (string Celda, object Valor)[] TextosFijos =
    [
        ("B1", "REGISTRO DE CONTROL DE ASISTENCIA Y SALIDA DIARIA - ETIQUETA 2"),
        ("B2", "PRODUCCIÓN ARÁNDANO"),
        ("P2", "FV. 26/05/20"),
        ("B3", "RAZÓN SOCIAL O DENOMINACIÓN SOCIAL"),
        ("D3", "RUC"),
        ("P3", "ACTIVIDA ECONÓMICA"),
        ("W3", "N° DE TRABAJADORES "),
        ("B4", "HASS PERÚ S.A"),
        ("D4", 20481121966L),
        ("P4", "CIIU:0113666"),
        ("B6", "PLANTA EMPACADORA: "),
        ("B7", "FECHA Y HORA: "),
        ("B9", "NOMBRE DE LABOR: Empaque"),
        ("U9", "LOTE:"),
        ("B10", "CODIGO DE LABOR:   0809"),
        ("B12", "CANT."),
        ("C12", "D.N.I."),
        ("D12", "APELLIDOS Y NOMBRES"),
        ("E12", "RECOLECCIÓN DE CAJAS"),
        ("AA12", "FIRMA"),
        ("Y13", "TOTAL"),
        ("B36", "SUPERVISOR:"),
        ("V36", "CODIGO:"),
    ];
}
