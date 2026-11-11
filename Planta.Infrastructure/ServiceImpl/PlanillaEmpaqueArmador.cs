using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Planta.Domain.Dtos;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Arma las hojas de la planilla de empaque (equivale a reportes.planilla.armado.go de la API Go). Es la parte que decide qué lleva cada hoja:
/// una hoja por anotador, producto y origen (línea manual o máquina), con 21 personas por hoja y 20 columnas de recolección; el Excel solo la escribe.
/// Recibe el data de PLANTA_ListarPlanillaEmpaqueProduccion o de PLANTA_ListarPlanillaEmpaqueTareo: { proceso, detalle }.
/// </summary>
internal static partial class PlanillaEmpaqueArmador
{
    private const int PersonasPorHoja = 21;      // filas 14..34 de la hoja
    private const int RecoleccionesPorHoja = 20; // columnas E..X de la hoja
    private const int LongitudMaximaNombreHoja = 31;
    private const int SitioSinPuesto = 9999;     // el que no tiene puesto conocido va al final

    internal sealed record Proceso(string IdProceso, string CodigoAcopio, string AcopioNombre, string Fecha, string Turno, string Supervisores);

    /// <summary>La planilla armada, el proceso del que sale y el anotador si todo el detalle es de uno solo (vacío si hay varios).</summary>
    internal sealed record Armado(PlanillaEmpaque Planilla, Proceso Proceso, string Anotador);

    // ───────────────────────── lectura del JSON ─────────────────────────

    private static string Texto(JsonElement e, string propiedad)
        => e.ValueKind == JsonValueKind.Object && e.TryGetProperty(propiedad, out var v) && v.ValueKind != JsonValueKind.Null ? v.ToString().Trim() : string.Empty;

    private static int Entero(JsonElement e, string propiedad)
    {
        if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty(propiedad, out var v))
        {
            return 0;
        }

        return v.ValueKind switch
        {
            JsonValueKind.Number when v.TryGetInt32(out var n) => n,
            JsonValueKind.Number => (int)Math.Round(v.GetDouble()),
            JsonValueKind.String when int.TryParse(v.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var s) => s,
            _ => 0,
        };
    }

    private static double Numero(JsonElement e, string propiedad)
        => e.ValueKind == JsonValueKind.Object && e.TryGetProperty(propiedad, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetDouble() : 0;

    private static bool Booleano(JsonElement e, string propiedad)
    {
        if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty(propiedad, out var v))
        {
            return false;
        }

        return v.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.Number => v.TryGetInt32(out var n) && n != 0,
            JsonValueKind.String => v.GetString() is "1" or "true" or "True",
            _ => false,
        };
    }

    private static Proceso LeerProceso(JsonElement data)
        => data.TryGetProperty("proceso", out var p) && p.ValueKind == JsonValueKind.Object
            ? new Proceso(Texto(p, "idProceso"), Texto(p, "codigoAcopio"), Texto(p, "acopioNombre"), Texto(p, "fecha"), Texto(p, "turno"), Texto(p, "supervisores"))
            : new Proceso(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    private static List<JsonElement> LeerDetalle(JsonElement data)
        => data.ValueKind == JsonValueKind.Object && data.TryGetProperty("detalle", out var d) && d.ValueKind == JsonValueKind.Array
            ? d.EnumerateArray().ToList()
            : [];

    // ───────────────────────── sitio y personas ─────────────────────────

    /// <summary>
    /// Dónde estuvo el empacador. El papel sale en el orden de la planta y no por abecedario: líneas manuales primero y luego las máquinas;
    /// dentro de cada una, por máquina, curo, número de línea y puesto.
    /// </summary>
    private readonly record struct Sitio(bool EsMaquina, string Maquina, int Curo, int Linea, int Puesto)
    {
        public static Sitio De(bool esMaquina, string maquina, int curo, int linea, int puesto)
        {
            if (puesto <= 0)
            {
                puesto = SitioSinPuesto;
            }

            if (!esMaquina)
            {
                maquina = string.Empty;
                curo = 0;
            }

            return new Sitio(esMaquina, maquina.Trim().ToLowerInvariant(), curo, linea, puesto);
        }

        /// <summary>Menor que 0 si este va antes que el otro, 0 si es el mismo sitio y mayor que 0 si va después.</summary>
        public int Comparar(Sitio otro)
        {
            if (EsMaquina != otro.EsMaquina)
            {
                return EsMaquina ? 1 : -1;
            }

            if (Maquina != otro.Maquina)
            {
                return string.CompareOrdinal(Maquina, otro.Maquina);
            }

            if (Curo != otro.Curo)
            {
                return Curo - otro.Curo;
            }

            return Linea != otro.Linea ? Linea - otro.Linea : Puesto - otro.Puesto;
        }
    }

    /// <summary>Una persona de una hoja mientras se arma: su sitio es el primero (en el orden de la planta) en el que estuvo.</summary>
    private sealed class PersonaTrabajo(string dni)
    {
        public string Dni { get; } = dni;
        public string Nombre { get; private set; } = string.Empty;
        public Sitio Sitio { get; private set; }
        public bool HayDato { get; private set; }
        public Dictionary<int, int> Cajas { get; } = [];

        public void Visto(string nombre, Sitio sitio)
        {
            if (Nombre.Length == 0)
            {
                Nombre = nombre.Trim();
            }

            if (!HayDato || sitio.Comparar(Sitio) < 0)
            {
                Sitio = sitio;
                HayDato = true;
            }
        }
    }

    private static PersonaTrabajo Persona(Dictionary<string, PersonaTrabajo> personas, string dni)
    {
        if (!personas.TryGetValue(dni, out var persona))
        {
            persona = new PersonaTrabajo(dni);
            personas[dni] = persona;
        }

        return persona;
    }

    private static int CompararTexto(string a, string b)
        => string.CompareOrdinal(a.ToLowerInvariant(), b.ToLowerInvariant());

    /// <summary>Las personas en el orden de la planta; dos personas no comparten puesto, pero si los datos vinieran raros el nombre desempata.</summary>
    private static List<PersonaTrabajo> Ordenadas(Dictionary<string, PersonaTrabajo> personas)
        => personas.Values
            .OrderBy(p => p, Comparer<PersonaTrabajo>.Create((a, b) =>
            {
                var c = a.Sitio.Comparar(b.Sitio);
                if (c != 0)
                {
                    return c;
                }

                c = CompararTexto(a.Nombre, b.Nombre);
                return c != 0 ? c : string.CompareOrdinal(a.Dni, b.Dni);
            }))
            .ToList();

    /// <summary>
    /// Pone las cajas de cada persona en columnas consecutivas, sin huecos (cada una llena de izquierda a derecha); si tiene más de 20, las primeras 20
    /// se suman en la columna 1 y las demás siguen en la 2, 3... Devuelve las cajas por columna y el número de columnas que usa.
    /// </summary>
    private static (Dictionary<int, int> Columnas, int Usadas) CompactarRecolecciones(Dictionary<int, int> cajas)
    {
        var numeros = cajas.Where(kv => kv.Value > 0).Select(kv => kv.Key).OrderBy(n => n).ToList();
        var columnas = new Dictionary<int, int>();
        if (numeros.Count > RecoleccionesPorHoja)
        {
            columnas[1] = numeros.Take(RecoleccionesPorHoja).Sum(n => cajas[n]);
            var resto = numeros.Skip(RecoleccionesPorHoja).ToList();
            for (var i = 0; i < resto.Count; i++)
            {
                columnas[i + 2] = cajas[resto[i]];
            }

            return (columnas, numeros.Count - RecoleccionesPorHoja + 1);
        }

        for (var i = 0; i < numeros.Count; i++)
        {
            columnas[i + 1] = cajas[numeros[i]];
        }

        return (columnas, numeros.Count);
    }

    // ───────────────────────── textos ─────────────────────────

    /// <summary>El producto entero, formato y tipo juntos, con el peso por caja: "4.4 oz Jumbo 0.125kg". El tipo no se repite si el formato ya lo dice.</summary>
    private static string VariedadDe(string formato, string tipoEmpaque, double pesoPorCaja)
    {
        var partes = new List<string>();
        formato = formato.Trim();
        if (formato.Length > 0)
        {
            partes.Add(formato);
        }

        var tipo = tipoEmpaque.Trim();
        if (tipo.Length > 0 && !formato.ToUpperInvariant().Contains(tipo.ToUpperInvariant(), StringComparison.Ordinal))
        {
            partes.Add(tipo);
        }

        // 2.50 -> "2.5kg" y 3.00 -> "3kg": los ceros de la decimal no dicen nada en el papel.
        if (pesoPorCaja > 0)
        {
            var redondeado = Math.Floor((pesoPorCaja * 1000) + 0.5) / 1000;
            partes.Add(redondeado.ToString("0.###", CultureInfo.InvariantCulture) + "kg");
        }

        return string.Join(" ", partes);
    }

    private static readonly char[] SeparadoresDeNombre = [' ', '\t', '\r', '\n'];

    /// <summary>La primera palabra del nombre en mayúscula (en la base van primero los apellidos): sirve para distinguir las hojas de cada anotador.</summary>
    internal static string ApellidoDe(string nombre)
    {
        var campos = (nombre ?? string.Empty).Split(SeparadoresDeNombre, StringSplitOptions.RemoveEmptyEntries);
        return campos.Length == 0 ? "SIN ANOTADOR" : campos[0].ToUpperInvariant();
    }

    private static string Recortar(string valor, int maximo)
        => valor.Length <= maximo ? valor : valor[..maximo].Trim();

    /// <summary>Un nombre de hoja que Excel acepte: sin / \ * ? : [ ], con espacios simples y de 31 caracteres como máximo.</summary>
    private static string NombreHojaValido(string valor)
    {
        var sb = new StringBuilder(valor.Length);
        foreach (var c in valor)
        {
            sb.Append(c is '/' or '\\' or '*' or '?' or ':' or '[' or ']' ? ' ' : c);
        }

        var limpio = string.Join(" ", sb.ToString().Split(SeparadoresDeNombre, StringSplitOptions.RemoveEmptyEntries));
        return Recortar(limpio.Length == 0 ? "Hoja" : limpio, LongitudMaximaNombreHoja);
    }

    /// <summary>Reparte nombres de hoja sin repetir (Excel no distingue mayúsculas): el que ya existe lleva " 2", " 3"...</summary>
    private sealed class NombresDeHoja
    {
        private readonly HashSet<string> usados = new(StringComparer.OrdinalIgnoreCase);

        public string Unico(string valor)
        {
            var baseNombre = NombreHojaValido(valor);
            var nombre = baseNombre;
            var contador = 2;
            while (usados.Contains(nombre))
            {
                var sufijo = " " + contador.ToString(CultureInfo.InvariantCulture);
                nombre = Recortar(baseNombre, LongitudMaximaNombreHoja - sufijo.Length) + sufijo;
                contador++;
            }

            usados.Add(nombre);
            return nombre;
        }
    }

    /// <summary>AAAA-MM-DD -> DD/MM/AAAA.</summary>
    private static string FechaCorta(string fecha)
    {
        var partes = fecha.Trim().Split('-');
        return partes.Length == 3 ? $"{partes[2]}/{partes[1]}/{partes[0]}" : fecha.Trim();
    }

    private static PlanillaEmpaque Cabecera(Proceso proceso, IEnumerable<PaginaPlanilla> paginas)
    {
        var acopio = proceso.CodigoAcopio.Trim();
        var nombre = proceso.AcopioNombre.Trim();
        if (nombre.Length > 0 && !string.Equals(nombre, acopio, StringComparison.OrdinalIgnoreCase))
        {
            acopio = (acopio + " " + nombre).Trim();
        }

        var turno = proceso.Turno.Trim();
        var planilla = new PlanillaEmpaque
        {
            Acopio = acopio,
            Fecha = FechaCorta(proceso.Fecha),
            Turno = turno.Length == 0 ? "DIA" : turno,
            Supervisor = proceso.Supervisores.Trim(),
        };
        planilla.Paginas.AddRange(paginas);
        return planilla;
    }

    private static PlanillaEmpaque CabeceraVacia(Proceso proceso) => Cabecera(proceso, []);

    private static string AnotadorUnico(IEnumerable<string> nombres)
    {
        var lista = nombres.Select(n => n.Trim()).ToList();
        return lista.Count > 0 && lista.All(n => n == lista[0]) ? lista[0] : string.Empty;
    }

    [GeneratedRegex("[^A-Za-z0-9_-]+")]
    private static partial Regex CaracteresDeArchivo();

    /// <summary>produccion_empaque_11-11-2026_DIA_H1106[_GRANADOS].xlsx. El anotador solo va si se pidió uno.</summary>
    internal static string NombreArchivo(string prefijo, Armado armado, string? idAnotadorPedido)
    {
        var cabecera = CabeceraVacia(armado.Proceso);
        var partes = new List<string> { prefijo, cabecera.Fecha.Replace('/', '-'), cabecera.Turno, armado.Proceso.CodigoAcopio };
        if (!string.IsNullOrWhiteSpace(idAnotadorPedido) && armado.Anotador.Trim().Length > 0)
        {
            partes.Add(ApellidoDe(armado.Anotador));
        }

        return string.Join("_", partes.Select(p => CaracteresDeArchivo().Replace(p.Trim(), string.Empty))) + ".xlsx";
    }

    // ───────────────────────── hojas ─────────────────────────

    /// <summary>
    /// Parte una lista de personas en hojas de 21 filas y, si hay más de 20 recolecciones, en bloques de 20 columnas. <paramref name="nombrar"/> da el nombre
    /// de cada hoja según su posición (fila de bloques, columna de bloques, y si hay más de una hoja).
    /// </summary>
    private static List<PaginaPlanilla> PaginasDeGrupo(List<PersonaTrabajo> personas, int columnas, PaginaPlanilla plantilla, NombresDeHoja nombres, Func<int, int, bool, string> nombrar)
    {
        columnas = Math.Max(columnas, 1);
        var bloquesFila = Math.Max((personas.Count + PersonasPorHoja - 1) / PersonasPorHoja, 1);
        var bloquesColumna = (columnas + RecoleccionesPorHoja - 1) / RecoleccionesPorHoja;
        var variasHojas = bloquesFila > 1 || bloquesColumna > 1;

        var paginas = new List<PaginaPlanilla>();
        for (var bf = 0; bf < bloquesFila; bf++)
        {
            var desde = Math.Min(bf * PersonasPorHoja, personas.Count);
            var hasta = Math.Min((bf + 1) * PersonasPorHoja, personas.Count);
            for (var bc = 0; bc < bloquesColumna; bc++)
            {
                var filas = personas.Skip(desde).Take(hasta - desde)
                    .Select(p => new PersonaPlanilla(p.Dni, p.Nombre, CompactarRecolecciones(p.Cajas).Columnas))
                    .ToList();
                paginas.Add(plantilla with
                {
                    Hoja = nombres.Unico(nombrar(bf, bc, variasHojas)),
                    InicioRecoleccion = (bc * RecoleccionesPorHoja) + 1,
                    IndiceInicio = (bf * PersonasPorHoja) + 1,
                    Personas = filas,
                });
            }
        }

        return paginas;
    }

    private sealed class GrupoProduccion
    {
        public string IdAnotador { get; init; } = string.Empty;
        public string Anotador { get; init; } = string.Empty;
        public string DniAnotador { get; init; } = string.Empty;
        public string Formato { get; init; } = string.Empty;
        public string TipoEmpaque { get; init; } = string.Empty;
        public double Peso { get; init; }
        public bool EsMaquina { get; init; }
        public string Maquina { get; init; } = string.Empty;
        public Dictionary<string, PersonaTrabajo> Personas { get; } = [];
    }

    /// <summary>
    /// La planilla de producción. Una hoja por anotador + formato + tipo de empaque + origen: abajo va el anotador que registró a esas personas (su nombre
    /// y su documento) y arriba el LOTE, que dice de qué máquina vienen las cajas (en las líneas manuales va vacío: no vienen de ninguna).
    /// Devuelve null si no hay cajas.
    /// </summary>
    internal static Armado? Produccion(JsonElement data)
    {
        var proceso = LeerProceso(data);
        var filas = LeerDetalle(data);
        if (filas.Count == 0)
        {
            return null;
        }

        var grupos = new Dictionary<string, GrupoProduccion>();
        var anotadores = new HashSet<string>();
        foreach (var f in filas)
        {
            var idAnotador = Texto(f, "idUsuarioAnotador");
            var esMaquina = Booleano(f, "esMaquina");
            var maquina = esMaquina ? Texto(f, "maquina") : string.Empty;
            var clave = $"{idAnotador}|{Texto(f, "formatoId")}|{Texto(f, "tipoEmpaqueId")}|{esMaquina}|{maquina.ToLowerInvariant()}";
            if (!grupos.TryGetValue(clave, out var grupo))
            {
                grupo = new GrupoProduccion
                {
                    IdAnotador = idAnotador,
                    Anotador = Texto(f, "anotador"),
                    DniAnotador = Texto(f, "dniAnotador"),
                    Formato = Texto(f, "formato"),
                    TipoEmpaque = Texto(f, "tipoEmpaque"),
                    Peso = Numero(f, "pesoPorCaja"),
                    EsMaquina = esMaquina,
                    Maquina = maquina,
                };
                grupos[clave] = grupo;
            }

            anotadores.Add(idAnotador);

            var persona = Persona(grupo.Personas, Texto(f, "dni"));
            persona.Visto(Texto(f, "empacador"), Sitio.De(esMaquina, Texto(f, "maquina"), Entero(f, "curo"), Entero(f, "numeroLinea"), Entero(f, "puesto")));
            var recoleccion = Entero(f, "numeroRecoleccion");
            persona.Cajas[recoleccion] = persona.Cajas.GetValueOrDefault(recoleccion) + Entero(f, "cajas");
        }

        var lista = grupos.Values
            .OrderBy(g => g, Comparer<GrupoProduccion>.Create((a, b) =>
            {
                var c = CompararTexto(a.Anotador, b.Anotador);
                if (c != 0)
                {
                    return c;
                }

                c = string.CompareOrdinal(a.IdAnotador, b.IdAnotador);
                if (c != 0)
                {
                    return c;
                }

                c = CompararTexto(a.Formato, b.Formato);
                if (c != 0)
                {
                    return c;
                }

                c = CompararTexto(a.TipoEmpaque, b.TipoEmpaque);
                if (c != 0)
                {
                    return c;
                }

                return a.EsMaquina != b.EsMaquina ? (a.EsMaquina ? 1 : -1) : CompararTexto(a.Maquina, b.Maquina);
            }))
            .ToList();

        var nombres = new NombresDeHoja();
        var variosAnotadores = anotadores.Count > 1;
        var paginas = new List<PaginaPlanilla>();
        foreach (var g in lista)
        {
            var personas = Ordenadas(g.Personas);
            var columnas = personas.Select(p => CompactarRecolecciones(p.Cajas).Usadas).DefaultIfEmpty(0).Max();

            var baseNombre = g.Formato + " - " + g.TipoEmpaque;
            if (variosAnotadores)
            {
                baseNombre = ApellidoDe(g.Anotador) + " · " + baseNombre;
            }

            if (g.EsMaquina && g.Maquina.Length > 0)
            {
                baseNombre += " · " + g.Maquina;
            }

            var plantilla = new PaginaPlanilla
            {
                Variedad = VariedadDe(g.Formato, g.TipoEmpaque, g.Peso),
                Lote = g.Maquina,
                Anotador = g.Anotador,
                AnotadorDni = g.DniAnotador,
            };
            paginas.AddRange(PaginasDeGrupo(personas, columnas, plantilla, nombres,
                (bf, bc, varias) => varias ? $"{baseNombre} {bf + 1}.{bc + 1}" : baseNombre));
        }

        return new Armado(Cabecera(proceso, paginas), proceso, AnotadorUnico(filas.Select(f => Texto(f, "anotador"))));
    }

    private sealed class AnotadorTareo(string id, string nombre, string dni)
    {
        public string Id { get; } = id;
        public string Nombre { get; } = nombre;
        public string Dni { get; } = dni;
        public Dictionary<string, PersonaTrabajo> Personas { get; } = [];
    }

    /// <summary>
    /// El tareo / asistencia: la misma hoja, pero con una sola lista por anotador: todos los empacadores que asignó, hayan anotado cajas o no.
    /// Las columnas de recolección quedan en blanco a propósito, para marcar a mano. Devuelve null si no hay empacadores.
    /// </summary>
    internal static Armado? Tareo(JsonElement data)
    {
        var proceso = LeerProceso(data);
        var filas = LeerDetalle(data);
        if (filas.Count == 0)
        {
            return null;
        }

        var porAnotador = new Dictionary<string, AnotadorTareo>();
        foreach (var f in filas)
        {
            var id = Texto(f, "idUsuarioAnotador");
            if (!porAnotador.TryGetValue(id, out var anotador))
            {
                anotador = new AnotadorTareo(id, Texto(f, "anotador"), Texto(f, "dniAnotador"));
                porAnotador[id] = anotador;
            }

            Persona(anotador.Personas, Texto(f, "dni"))
                .Visto(Texto(f, "empacador"), Sitio.De(Booleano(f, "esMaquina"), Texto(f, "maquina"), Entero(f, "curo"), Entero(f, "numeroLinea"), Entero(f, "puesto")));
        }

        var lista = porAnotador.Values
            .OrderBy(a => a, Comparer<AnotadorTareo>.Create((a, b) =>
            {
                var c = CompararTexto(a.Nombre, b.Nombre);
                return c != 0 ? c : string.CompareOrdinal(a.Id, b.Id);
            }))
            .ToList();

        var nombres = new NombresDeHoja();
        var paginas = new List<PaginaPlanilla>();
        foreach (var a in lista)
        {
            var baseNombre = lista.Count > 1 ? "Tareo " + ApellidoDe(a.Nombre) : "Tareo";
            var titulo = a.Nombre.Length > 0 ? "Anotador: " + a.Nombre : "Todos los anotadores";
            var plantilla = new PaginaPlanilla
            {
                Cabecera = "TAREO / ASISTENCIA — " + titulo,
                Anotador = a.Nombre,
                AnotadorDni = a.Dni,
            };
            paginas.AddRange(PaginasDeGrupo(Ordenadas(a.Personas), 1, plantilla, nombres,
                (bf, _, varias) => varias ? $"{baseNombre} {bf + 1}" : baseNombre));
        }

        return new Armado(Cabecera(proceso, paginas), proceso, AnotadorUnico(filas.Select(f => Texto(f, "anotador"))));
    }
}
