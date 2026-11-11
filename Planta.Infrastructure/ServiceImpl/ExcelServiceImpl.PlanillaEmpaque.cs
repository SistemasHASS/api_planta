using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using Planta.Domain.Dtos;

namespace Planta.Infrastructure.ServiceImpl;

public sealed partial class ExcelServiceImpl
{
    private const int PrimeraFilaPersonas = 14;
    private const int FilaNumerosRecoleccion = 13;
    private const int PrimeraColumnaRecoleccion = 5; // E
    private const int RecoleccionesPorHojaPlanilla = 20;
    private const int ColumnaTotalPlanilla = 25;     // Y

    /// <summary>
    /// Dibuja la planilla de empaque: una hoja por cada página de <paramref name="planilla"/>, con el logo de la empresa, la cabecera de la planta y el pie del anotador.
    /// Todo el formato está en PlanillaEmpaqueFormato (no usa ningún archivo de plantilla); es el mismo que dibuja la API Go.
    /// </summary>
    public byte[] GenerarPlanillaEmpaque(PlanillaEmpaque planilla)
    {
        if (planilla.Paginas.Count == 0)
        {
            throw new ArgumentException("No hay hojas para exportar.", nameof(planilla));
        }

        using var paquete = new ExcelPackage();
        var medidas = planilla.Logo is { Length: > 0 } ? DimensionesDeImagen(planilla.Logo) : null;

        foreach (var pagina in planilla.Paginas)
        {
            var hoja = paquete.Workbook.Worksheets.Add(pagina.Hoja);
            DibujarHojaPlanilla(hoja);
            if (medidas is { } m)
            {
                PonerLogoPlanilla(hoja, planilla.Logo!, m.Ancho, m.Alto);
            }

            LlenarPaginaPlanilla(hoja, planilla, pagina);
        }

        // Que no queden hojas agrupadas: solo la primera seleccionada.
        for (var i = 0; i < paquete.Workbook.Worksheets.Count; i++)
        {
            paquete.Workbook.Worksheets[i].View.TabSelected = i == 0;
        }

        paquete.Workbook.View.ActiveTab = 0;
        return paquete.GetAsByteArray();
    }

    /// <summary>Deja una hoja con el formato de la planilla y sus textos fijos, todavía sin datos.</summary>
    private static void DibujarHojaPlanilla(ExcelWorksheet hoja)
    {
        hoja.DefaultColWidth = PlanillaEmpaqueFormato.AnchoColumnaPorDefecto;
        hoja.DefaultRowHeight = PlanillaEmpaqueFormato.AltoFilaPorDefecto;
        hoja.View.ShowGridLines = false;
        hoja.View.ZoomScale = PlanillaEmpaqueFormato.Zoom;

        foreach (var (desde, hasta, ancho) in PlanillaEmpaqueFormato.AnchosColumnas)
        {
            for (var c = desde; c <= hasta; c++)
            {
                hoja.Column(c).Width = ancho;
            }
        }

        foreach (var (fila, altura) in PlanillaEmpaqueFormato.AlturasFilas)
        {
            hoja.Row(fila).Height = altura;
        }

        foreach (var zona in PlanillaEmpaqueFormato.Zonas)
        {
            foreach (var rango in zona.Rangos)
            {
                AplicarEstiloPlanilla(hoja.Cells[rango], zona.Estilo);
            }
        }

        foreach (var rango in PlanillaEmpaqueFormato.Combinadas)
        {
            hoja.Cells[rango].Merge = true;
        }

        foreach (var (celda, valor) in PlanillaEmpaqueFormato.TextosFijos)
        {
            hoja.Cells[celda].Value = valor;
        }

        // Impresión: A4 horizontal, todo en una sola página, centrada y con el área B1:AA36.
        var impresion = hoja.PrinterSettings;
        impresion.Orientation = eOrientation.Landscape;
        impresion.PaperSize = ePaperSize.A4;
        impresion.FitToPage = true;
        impresion.FitToWidth = 1;
        impresion.FitToHeight = 1;
        impresion.HorizontalCentered = true;
        impresion.VerticalCentered = true;
        impresion.LeftMargin = 0.0388888888888889m;
        impresion.RightMargin = 0.0388888888888889m;
        impresion.TopMargin = 0.354166666666667m;
        impresion.BottomMargin = 0.354166666666667m;
        impresion.HeaderMargin = 0.313888888888889m;
        impresion.FooterMargin = 0.313888888888889m;
        impresion.PrintArea = hoja.Cells["B1:AA36"];
    }

    private static void AplicarEstiloPlanilla(ExcelRange rango, PlanillaEmpaqueFormato.Estilo e)
    {
        var estilo = rango.Style;
        estilo.Font.Name = e.Fuente;
        estilo.Font.Size = e.Tamano;
        estilo.Font.Bold = e.Negrita;
        estilo.Font.Italic = e.Cursiva;
        estilo.Font.Color.SetColor(ColorDe(e.ColorFuente));

        if (e.Relleno.Length > 0)
        {
            estilo.Fill.PatternType = ExcelFillStyle.Solid;
            estilo.Fill.BackgroundColor.SetColor(ColorDe(e.Relleno));
        }

        foreach (var lado in e.Bordes)
        {
            var borde = lado switch
            {
                'L' => estilo.Border.Left,
                'R' => estilo.Border.Right,
                'T' => estilo.Border.Top,
                _ => estilo.Border.Bottom,
            };
            borde.Style = ExcelBorderStyle.Thin;
            borde.Color.SetColor(Color.Black);
        }

        if (e.Horizontal.Length > 0)
        {
            estilo.HorizontalAlignment = e.Horizontal == "center" ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Left;
        }

        if (e.Vertical.Length > 0)
        {
            estilo.VerticalAlignment = e.Vertical == "center" ? ExcelVerticalAlignment.Center : ExcelVerticalAlignment.Top;
        }

        estilo.WrapText = e.Ajustar;
        estilo.ShrinkToFit = e.Reducir;
        if (e.Rotacion != 0)
        {
            estilo.TextRotation = e.Rotacion;
        }
    }

    private static Color ColorDe(string rgb)
        => Color.FromArgb(Convert.ToInt32(rgb[..2], 16), Convert.ToInt32(rgb[2..4], 16), Convert.ToInt32(rgb[4..6], 16));

    // La caja del logo: arranca en X1 (con este desfase, en píxeles) y mide esto; el logo se acomoda dentro sin deformarse.
    private const int LogoColumnaX = 23; // X (base 0)
    private const int LogoDesfaseX = 55;
    private const int LogoDesfaseY = 10;
    private const double LogoAncho = 192;
    private const double LogoAlto = 106;

    private static void PonerLogoPlanilla(ExcelWorksheet hoja, byte[] logo, int ancho, int alto)
    {
        var escala = Math.Min(LogoAncho / ancho, LogoAlto / alto);
        var w = Math.Max(1, (int)Math.Round(ancho * escala));
        var h = Math.Max(1, (int)Math.Round(alto * escala));

        using var flujo = new MemoryStream(logo);
        var imagen = hoja.Drawings.AddPicture("Logo", flujo);
        imagen.SetPosition(0, LogoDesfaseY + (int)((LogoAlto - h) / 2), LogoColumnaX, LogoDesfaseX);
        imagen.SetSize(w, h);
        imagen.EditAs = eEditAs.OneCell;
    }

    /// <summary>Ancho y alto en píxeles de un JPEG o un PNG; null si no se puede leer (entonces la planilla sale sin logo).</summary>
    private static (int Ancho, int Alto)? DimensionesDeImagen(byte[] b)
    {
        // PNG: firma de 8 bytes y, en el bloque IHDR, el ancho y el alto en 4 bytes cada uno.
        if (b.Length > 24 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47)
        {
            var ancho = (b[16] << 24) | (b[17] << 16) | (b[18] << 8) | b[19];
            var alto = (b[20] << 24) | (b[21] << 16) | (b[22] << 8) | b[23];
            return ancho > 0 && alto > 0 ? (ancho, alto) : null;
        }

        // JPEG: se recorren los bloques hasta el SOF, que trae el alto y el ancho.
        if (b.Length > 4 && b[0] == 0xFF && b[1] == 0xD8)
        {
            var i = 2;
            while (i + 9 < b.Length)
            {
                if (b[i] != 0xFF)
                {
                    i++;
                    continue;
                }

                var marcador = b[i + 1];
                if (marcador == 0xFF)
                {
                    i++;
                    continue;
                }

                if (marcador is 0xD8 or 0x01 or (>= 0xD0 and <= 0xD7))
                {
                    i += 2;
                    continue;
                }

                if (marcador is >= 0xC0 and <= 0xCF and not 0xC4 and not 0xC8 and not 0xCC)
                {
                    var alto = (b[i + 5] << 8) | b[i + 6];
                    var ancho = (b[i + 7] << 8) | b[i + 8];
                    return ancho > 0 && alto > 0 ? (ancho, alto) : null;
                }

                i += 2 + ((b[i + 2] << 8) | b[i + 3]);
            }
        }

        return null;
    }

    private static void LlenarPaginaPlanilla(ExcelWorksheet hoja, PlanillaEmpaque planilla, PaginaPlanilla pagina)
    {
        hoja.Cells["D6"].Value = planilla.Acopio;
        hoja.Cells["D7"].Value = planilla.Fecha;
        hoja.Cells["U7"].Value = "TURNO: " + planilla.Turno;
        hoja.Cells["W4"].Value = pagina.Personas.Count;
        hoja.Cells["W9"].Value = pagina.Lote;

        // PROYECTO es el supervisor del proceso; si el proceso no tiene ninguno queda "PROYECTO:" para rellenar a mano.
        var supervisor = planilla.Supervisor.Trim();
        hoja.Cells["U6"].Value = supervisor.Length > 0 ? "PROYECTO: " + supervisor : "PROYECTO:";

        // VARIEDAD lleva el producto entero; el tareo no tiene variedad y manda su propia cabecera, que se respeta tal cual.
        var cabecera = pagina.Cabecera.Trim();
        var variedad = pagina.Variedad.Trim();
        hoja.Cells["U10"].Value = cabecera.Length > 0 ? cabecera : variedad.Length > 0 ? "VARIEDAD: " + variedad : "VARIEDAD:";

        // El que anota firma abajo, con su documento al lado de CODIGO.
        hoja.Cells["D36"].Value = pagina.Anotador;
        if (pagina.AnotadorDni.Trim().Length > 0)
        {
            hoja.Cells["Y36"].Value = pagina.AnotadorDni.Trim();
        }

        var inicio = Math.Max(pagina.InicioRecoleccion, 1);
        for (var i = 0; i < RecoleccionesPorHojaPlanilla; i++)
        {
            hoja.Cells[FilaNumerosRecoleccion, PrimeraColumnaRecoleccion + i].Value = inicio + i;
        }

        for (var i = 0; i < pagina.Personas.Count; i++)
        {
            var persona = pagina.Personas[i];
            var fila = PrimeraFilaPersonas + i;
            var indice = pagina.IndiceInicio < 1 ? 1 + i : pagina.IndiceInicio + i;
            hoja.Cells[fila, 2].Value = indice;
            hoja.Cells[fila, 3].Value = persona.Dni;
            // Los nombres siempre en mayúscula: en la base conviven las dos formas y en el papel se ve descuidado. Se cambia al escribir, no en la base.
            hoja.Cells[fila, 4].Value = persona.Nombre.ToUpperInvariant();

            var total = 0;
            for (var j = 0; j < RecoleccionesPorHojaPlanilla; j++)
            {
                if (persona.Recolecciones.TryGetValue(inicio + j, out var cajas) && cajas > 0)
                {
                    hoja.Cells[fila, PrimeraColumnaRecoleccion + j].Value = cajas;
                    total += cajas;
                }
            }

            if (total > 0)
            {
                hoja.Cells[fila, ColumnaTotalPlanilla].Value = total;
            }
        }
    }
}
