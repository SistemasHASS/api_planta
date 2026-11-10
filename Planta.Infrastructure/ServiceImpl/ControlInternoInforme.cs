using System.Text.Json;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Arma el informe de control interno a partir de data = { columnas, detalle } de PLANTA_ListarControlInterno:
/// una fila por jornada + empacador, con las cajas y los kilos de pago de cada producto (columna).
/// Solo sale quien tuvo movimiento. Lo único que puede tener varios valores en una fila es el anotador (se juntan con comas).
/// </summary>
internal sealed class ControlInternoInforme
{
    internal sealed record Columna(string Clave, string Formato, string TipoEmpaque, string Categoria, string CalibreId, string Calibre, bool FueraDeMatriz);

    internal sealed class Fila
    {
        public string Fecha { get; init; } = string.Empty;
        public string Acopio { get; init; } = string.Empty;
        public string Dni { get; init; } = string.Empty;
        public string Empacador { get; init; } = string.Empty;
        public SortedSet<string> Anotadores { get; } = new(StringComparer.CurrentCultureIgnoreCase);
        public Dictionary<string, long> Cajas { get; } = new();
        public Dictionary<string, decimal> Kilos { get; } = new();
        public long Total { get; set; }
        public decimal TotalKilos { get; set; }
    }

    public List<Columna> Columnas { get; } = [];
    public List<Fila> Filas { get; } = [];

    public static ControlInternoInforme Armar(JsonElement data)
    {
        var informe = new ControlInternoInforme();

        if (data.ValueKind == JsonValueKind.Object && data.TryGetProperty("columnas", out var columnas) && columnas.ValueKind == JsonValueKind.Array)
        {
            foreach (var c in columnas.EnumerateArray())
            {
                informe.Columnas.Add(new Columna(
                    Texto(c, "clave"), Texto(c, "formato"), Texto(c, "tipoEmpaque"), Texto(c, "categoria"),
                    Texto(c, "calibreId"), Texto(c, "calibre"),
                    c.TryGetProperty("fueraDeMatriz", out var f) && f.ValueKind == JsonValueKind.True));
            }
        }

        var filas = new Dictionary<string, Fila>();
        if (data.ValueKind == JsonValueKind.Object && data.TryGetProperty("detalle", out var detalle) && detalle.ValueKind == JsonValueKind.Array)
        {
            foreach (var d in detalle.EnumerateArray())
            {
                var clave = $"{Texto(d, "idJornada")}|{Texto(d, "dni")}";
                if (!filas.TryGetValue(clave, out var fila))
                {
                    fila = new Fila { Fecha = Texto(d, "fecha"), Acopio = Texto(d, "codigoAcopio"), Dni = Texto(d, "dni"), Empacador = Texto(d, "empacador") };
                    filas[clave] = fila;
                }

                var anotador = Texto(d, "anotador");
                fila.Anotadores.Add(anotador.Length == 0 ? "(sin anotador)" : anotador);

                var producto = $"{Texto(d, "formatoId")}-{Texto(d, "tipoEmpaqueId")}-{Texto(d, "categoriaId")}";
                var cajas = d.TryGetProperty("cajas", out var cj) && cj.TryGetInt64(out var n) ? n : 0;
                var kilos = d.TryGetProperty("kilos", out var kg) && kg.TryGetDecimal(out var k) ? k : 0m;
                fila.Cajas[producto] = fila.Cajas.GetValueOrDefault(producto) + cajas;
                fila.Kilos[producto] = fila.Kilos.GetValueOrDefault(producto) + kilos;
                fila.Total += cajas;
                fila.TotalKilos += kilos;
            }
        }

        informe.Filas.AddRange(filas.Values
            .OrderBy(f => f.Fecha, StringComparer.Ordinal)
            .ThenBy(f => f.Acopio, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(f => f.Empacador, StringComparer.CurrentCultureIgnoreCase));
        return informe;
    }

    private static string Texto(JsonElement e, string propiedad)
        => e.TryGetProperty(propiedad, out var v) && v.ValueKind != JsonValueKind.Null ? (v.ToString() ?? string.Empty).Trim() : string.Empty;
}
