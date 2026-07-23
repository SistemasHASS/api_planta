using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Text.Json;
using Planta.Application.Proceso.Abstractions;
using Planta.Application.Proceso.Models;
using System.Collections.Generic;
using System.Linq;

using System.IO;

namespace Planta.Infrastructure.ServiceImpl
{
    public class PdfServiceImpl : IPdfService
    {
        public PdfServiceImpl()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerarFichaComposicionPaletAsync(object data, string? webRootPath = null)
        {
            var model = (FichaComposicionPaletModel)data;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9).FontFamily(Fonts.Verdana));

                    // Header Table
                    page.Header().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // RAZON SOCIAL
                            columns.RelativeColumn(2); // RUC
                            columns.RelativeColumn(5); // DOMICILIO
                            columns.RelativeColumn(2); // ACTIVIDAD
                            columns.RelativeColumn(2); // N TRABAJADORES / LOGO
                        });

                        // Row 1: Title & Logo
                        table.Cell().ColumnSpan(4).Border(1).Padding(2).AlignCenter().AlignMiddle().Text("FICHA DE COMPOSICIÓN DE PALET").Bold().FontSize(11);
                        
                        // Dynamic Logo Cell
                        var logoCell = table.Cell().RowSpan(2).Border(1).Padding(2).AlignCenter().AlignMiddle();
                        
                        if (!string.IsNullOrEmpty(webRootPath) && !string.IsNullOrEmpty(model.Cabecera.Ruc))
                        {
                            var logoPath = Path.Combine(webRootPath, "assets", "logos", $"{model.Cabecera.Ruc}.jpg");
                            if (File.Exists(logoPath))
                            {
                                logoCell.Width(55).Image(logoPath);
                            }
                            else
                            {
                                logoCell.Text("LOGO").FontSize(8);
                            }
                        }
                        else
                        {
                            logoCell.Text("LOGO").FontSize(8);
                        }

                        // Row 2: Headers Row 1 (HACCP, CODIGO, FV)
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("HACCP").Bold().FontSize(8);
                        table.Cell().ColumnSpan(2).Border(1).Padding(1).AlignCenter().AlignMiddle().Text($"CÓDIGO: {model.Cabecera.Codigo}").Bold().FontSize(8);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text($"F.V. {model.Cabecera.FV}").Bold().FontSize(8);

                        // Row 3: Company Headers
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("RAZÓN SOCIAL O DENOMINACIÓN SOCIAL").Bold().FontSize(7);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("RUC").Bold().FontSize(7);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("DOMICILIO\n(Dirección, distrito, departamento y provincia)").Bold().FontSize(7);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("ACTIVIDAD ECONÓMICA").Bold().FontSize(7);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("N° TRABAJADORES").Bold().FontSize(7);

                        // Row 4: Company Data
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text(model.Cabecera.RazonSocial ?? string.Empty).FontSize(8);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text(model.Cabecera.Ruc ?? string.Empty).FontSize(8);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("CTRA. PANAMERICANA NORTE KM 492.5 CHAO - LA LIBERTAD").FontSize(7);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text(model.Cabecera.ActividadEconomica ?? string.Empty).FontSize(8);
                        table.Cell().Border(1).Padding(1).AlignCenter().AlignMiddle().Text("").FontSize(8);
                    });

                    page.Content().PaddingVertical(5).Column(column =>
                    {
                        // Info Section as a Table for better alignment
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Left labels
                                columns.RelativeColumn(2); // Right labels
                            });

                            table.Cell().Column(c =>
                            {
                                c.Item().Text(t => { t.Span("FECHA : ").Bold(); t.Span(model.Cabecera.FechaProceso); });
                                
                                var nombresClientes = model.Cabecera.Clientes.Select(c => c.Cliente).Where(n => !string.IsNullOrWhiteSpace(n));
                                c.Item().Text(t => { t.Span("CLIENTE : ").Bold(); t.Span(string.Join(", ", nombresClientes)); });
                                
                                c.Item().Text(t => { t.Span("FORMATO : ").Bold(); t.Span(model.Cabecera.Formato); });
                                
                                var nombresDestinos = model.Cabecera.Destinos.Select(d => d.Destino).Where(n => !string.IsNullOrWhiteSpace(n));
                                c.Item().Text(t => { t.Span("DESTINO : ").Bold(); t.Span(string.Join(", ", nombresDestinos)); });
                                
                                c.Item().Text(t => { t.Span("HORA FINAL : ").Bold(); t.Span(model.Cabecera.HoraFinal); });
                            });

                            table.Cell().AlignRight().Column(c =>
                            {
                                c.Item().Text(t => { t.Span("PLANTA DE EMPAQUE : ").Bold(); t.Span(model.Cabecera.PlantaDeEmpaque); });
                                c.Item().Text(t => { t.Span("ID PALLET : ").Bold(); t.Span(model.Cabecera.IdPalet_numero); });
                            });
                        });

                        column.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Variedad
                                columns.RelativeColumn(2); // Tipo de empaque
                                columns.RelativeColumn(1); // N° Cajas
                                columns.RelativeColumn(3); // LDP / C. Rancho
                            });

                            table.Header(header =>
                            {
                                header.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text("VARIEDAD").Bold();
                                header.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text("TIPO DE EMPAQUE").Bold();
                                header.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text("N° CAJAS").Bold();
                                header.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text("LDP / C. RANCHO").Bold();
                            });

                            foreach (var item in model.Detalle)
                            {
                                var nombresVariedades = item.Variedad.Select(v => string.IsNullOrWhiteSpace(v.Variedad) ? v.VariedadId : v.Variedad);
                                table.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text(string.Join(", ", nombresVariedades));
                                table.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text(item.Tipo_De_Empaque);
                                table.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text(item.CantidadCajas.ToString());
                                table.Cell().Border(1).Padding(2).AlignCenter().AlignMiddle().Text(item.LDP_C_Rancho);
                            }
                        });

                        // Total and Legend (Commented)
                        column.Item().PaddingTop(10).Row(row =>
                        {
                            row.RelativeItem().Column(c => {
                                /*
                                c.Item().Table(t =>
                                {
                                    t.ColumnsDefinition(cd =>
                                    {
                                        cd.RelativeColumn(3);
                                        cd.RelativeColumn(1);
                                    });
                                    t.Header(h =>
                                    {
                                        h.Cell().ColumnSpan(2).Border(1).Background(Colors.Grey.Lighten4).AlignCenter().Text("LEYENDA").Bold();
                                    });
                                    t.Cell().Border(1).Padding(2).Text("Convencional"); t.Cell().Border(1).Padding(2).AlignCenter().Text("C");
                                    t.Cell().Border(1).Padding(2).Text("Jumbo"); t.Cell().Border(1).Padding(2).AlignCenter().Text("J");
                                    t.Cell().Border(1).Padding(2).Text("Sweetest Batch"); t.Cell().Border(1).Padding(2).AlignCenter().Text("SB");
                                });
                                */
                            });

                            row.RelativeItem().AlignBottom().AlignRight().Text(t => 
                            { 
                                t.Span("TOTAL DE CAJAS: ").Bold().FontSize(14); 
                                t.Span(model.Cabecera.TotalCajas.ToString()).Bold().FontSize(16).Underline(); 
                            });
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
