namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/jornadas-anotador/listar. Empresa, RUC y anotador salen del token. SoloAbierta = true devuelve solo la jornada abierta.</summary>
public sealed class ListarJornadasAnotadorRequest
{
    public bool SoloAbierta { get; set; } = true;
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/sincronizar. Modo "nuevo" abre una jornada (IdProyecto e IdProceso obligatorios);
/// modo "editado" cierra la jornada Id. El acopio, la empresa, el RUC, el usuario y el rol salen del token.
/// </summary>
public sealed class SincronizarJornadaAnotadorRequest
{
    public string Modo { get; set; } = "nuevo";
    public int Id { get; set; }
    public string IdProyecto { get; set; } = "";
    public string IdProceso { get; set; } = "";
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/lineas/sincronizar. Modo "nuevo" toma la línea IdLinea; modo "editado" suelta la fila Id
/// (id de PLANTA_Jornadas_Anotador_Lineas). El anotador, su nombre, el acopio y la jornada salen del token y del SP.
/// </summary>
public sealed class SincronizarJornadaAnotadorLineaRequest
{
    public string Modo { get; set; } = "nuevo";
    public int Id { get; set; }
    public int IdLinea { get; set; }
}

/// <summary>Cuerpo de api/jornadas-anotador/trabajador/buscar. El RUC sale del token.</summary>
public sealed class BuscarTrabajadorRequest
{
    public string NumeroDocumento { get; set; } = "";
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/puestos/sincronizar. Modo "nuevo" asigna el empacador (NumeroDocumento, NombreCompleto, ApellidoPaterno, ApellidoMaterno, tal como los devuelve la búsqueda del trabajador) al puesto Puesto de la línea IdLinea
/// (si el puesto tiene a otro, lo reemplaza); modo "mover" mueve la fila Id al puesto Puesto de la línea IdLinea; modo "editado" quita la fila Id; modo "vaciar" libera todos los empacadores de la línea IdLinea.
/// El acopio, el anotador y la jornada salen del token y del SP.
/// </summary>
public sealed class SincronizarJornadaAnotadorPuestoRequest
{
    public string Modo { get; set; } = "nuevo";
    public int Id { get; set; }
    public int IdLinea { get; set; }
    public int Puesto { get; set; }
    public string NumeroDocumento { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
    public string ApellidoPaterno { get; set; } = "";
    public string ApellidoMaterno { get; set; } = "";
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/recolecciones/sincronizar. Modo "nuevo" inicia una recolección del anotador; modo "editado" cierra la recolección Id (solo su dueño).
/// El acopio, el anotador y su nombre salen del token.
/// </summary>
public sealed class SincronizarJornadaAnotadorRecoleccionRequest
{
    public string Modo { get; set; } = "nuevo";
    public int Id { get; set; }
}

/// <summary>Cuerpo de api/jornadas-anotador/recolecciones/cajas/historial: IdRecoleccion = 0 lista las recolecciones del anotador; mayor que 0 devuelve el detalle de esa recolección.</summary>
public sealed class ListarRecoleccionesCajasHistorialRequest
{
    public int IdRecoleccion { get; set; }
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/produccion/corregir: corrige un registro de cajas (una anotación de un empacador) de la producción del día o del historial: la cantidad y, si se envían
/// FormatoId y TipoEmpaqueId (siempre los dos juntos), también el formato y el tipo de empaque. CalibreNombre es el calibre del tipo de empaque elegido (el nombre sale de apimaestra).
/// El acopio y el usuario salen del token.
/// </summary>
public sealed class CorregirCajasRecoleccionRequest
{
    public int Id { get; set; }
    public int CantidadCajas { get; set; }
    public int? FormatoId { get; set; }
    public int? TipoEmpaqueId { get; set; }
    public string? CalibreNombre { get; set; }
}

/// <summary>Cuerpo de api/jornadas-anotador/tipos-empaque/listar: tipos de empaque que la matriz de compatibilidad asocia al formato. Empresa y RUC salen del token.</summary>
public sealed class ListarTiposEmpaquePorMatrizRequest
{
    public string CodigoCultivo { get; set; } = "";
    public int FormatoId { get; set; }
}

/// <summary>
/// Cuerpo de api/jornadas-anotador/recolecciones/cajas/sincronizar. Hay una sola fila de cajas por empacador y recolección: modo "nuevo" la crea o, si ya existe, la actualiza
/// (cantidad final, formato y tipo de empaque) dejando la traza en el stock; modo "anular" quita al empacador de la recolección (solo IdLinea y Puesto). Modo "nuevo" registra CantidadCajas del formato y tipo de empaque elegidos en la línea IdLinea,
/// para el empacador que está en el puesto Puesto, dentro de la recolección abierta del anotador. El acopio y el anotador salen del token; el empacador, la línea,
/// el formato y el tipo de empaque se leen de la base. Solo CalibreNombre viene del cuerpo (el nombre del calibre es de apimaestra).
/// </summary>
public sealed class SincronizarRecoleccionCajaRequest
{
    public string Modo { get; set; } = "nuevo";
    public int Id { get; set; }
    public int IdLinea { get; set; }
    public int Puesto { get; set; }
    public int FormatoId { get; set; }
    public int TipoEmpaqueId { get; set; }
    public int CantidadCajas { get; set; }
    public string CalibreNombre { get; set; } = "";
}
