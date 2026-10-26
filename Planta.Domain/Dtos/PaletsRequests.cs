namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/palets/por-proceso.</summary>
public sealed class PaletsPorProcesoRequest
{
    public string IdProceso { get; set; } = "";
}

/// <summary>Cuerpo de las consultas por palet (dpalets y ficha de composicion).</summary>
public sealed class PaletRequest
{
    public string IdPalet { get; set; } = "";
}

/// <summary>Cuerpo de api/palets/sincronizar. Palets es un texto JSON que el SP lee tal cual.</summary>
public sealed class SincronizarPaletsRequest
{
    public string Palets { get; set; } = "";
}

/// <summary>Cuerpo de api/palets/sincronizar-dpalets. DPalets es un texto JSON que el SP lee tal cual.</summary>
public sealed class SincronizarDPaletsRequest
{
    public string DPalets { get; set; } = "";
}

/// <summary>Cuerpo de api/palets/codigos-rancho-por-lugar.</summary>
public sealed class CodigosRanchoPorLugarRequest
{
    public string IdProyecto { get; set; } = "";
    public int IdLugaresDeProduccion { get; set; }
}

/// <summary>Cuerpo de api/palets/matriz/destinos.</summary>
public sealed class DestinosPorMatrizRequest
{
    public string IdProyecto { get; set; } = "";
    public string DocumentoConsignatario { get; set; } = "";
}

/// <summary>
/// Cuerpo de las consultas encadenadas de la matriz de compatibilidad
/// (formato, tipo de empaque de guia, presentacion, tipo de caja, clamshell).
/// Cada ruta exige solo los campos que necesita; los demas pueden ir vacios.
/// </summary>
public sealed class MatrizCompatibilidadRequest
{
    public string CodigoCultivo { get; set; } = "";
    public string DocumentoConsignatario { get; set; } = "";
    public string DestinoId { get; set; } = "";
    public int? FormatoId { get; set; }
    public int? TipoEmpaqueGuiaId { get; set; }
    public int? PresentacionId { get; set; }
    public int? TipoCajaId { get; set; }
    public int? TipoClamshellId { get; set; }
}

/// <summary>
/// Cuerpo de api/administracion/acopios-config/sincronizar. Los dos json son texto JSON que el SP lee tal cual:
/// Json = [{ codigoAcopio, acopioNombre, serieGuia }], JsonDetalle = [{ codigoAcopio, codigoTipoProcesoEmpacado, activo }].
/// </summary>
public sealed class SincronizarAcopioConfigRequest
{
    public string Json { get; set; } = "[]";
    public string JsonDetalle { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/usuarios/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "editado", usuario, documentoIdentidad, nombre, idRol, codigoAcopio, acopioNombre, serieGuia }].
/// </summary>
public sealed class SincronizarUsuariosRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/destinatarios/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, idCliente, documentoFiscal, documento, domicilioFiscal, puntoLlegada, domicilioFiscalDistrito, puntoLlegadaDistrito, activo, nombre, tipoDocumento, esCliente }].
/// </summary>
public sealed class SincronizarDestinatariosRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/consignatarios/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ documento, nombre, codigoGrupoCliente, codigosRancho: [{ codigo, activo }] }].
/// </summary>
public sealed class SincronizarConsignatariosRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/variedades/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id (vacío si es nueva), cultivo, idcultivo, variedad, procedencia, esEnsayo, eliminado }].
/// </summary>
public sealed class SincronizarVariedadesRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/correlativos-documentos/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "nuevo" | "editado", serie, numero, eliminado }].
/// </summary>
public sealed class SincronizarCorrelativosRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/catalogos-operativos/sincronizar. Tabla: una de las permitidas en AdministracionServiceImpl (conductores, vehículos,
/// transportistas, supervisores, personal de logística y los catálogos del menú Catálogos). Json es texto JSON que el SP lee tal cual: [{ id, idproyecto, activo, ...campos de la tabla }].
/// </summary>
public sealed class SincronizarCatalogoOperativoRequest
{
    public string Tabla { get; set; } = "";
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/reglas-sobrepeso/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "nuevo" | "editado", idProyecto, codigoCultivo, documentoConsignatario, formatoId, destinoId, transporteId, porcentaje, vigenciaDesde, vigenciaHasta, descripcion, activo }].
/// </summary>
public sealed class SincronizarReglasSobrepesoRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/matrices-compatibilidad/sincronizar. Json es texto JSON que el SP lee tal cual:
/// [{ id, modo: "nuevo" | "editado", idProyecto, codigoCultivo, destinoId, documentoCliente, documentoConsignatario, formatoId,
///    tiposEmpaqueId, calibreId, tipoCajaId, tipoClamshellId, presentacionId, activo, tipoEmpaqueGuiaId, categoriaId }].
/// </summary>
public sealed class SincronizarMatricesCompatibilidadRequest
{
    public string Json { get; set; } = "[]";
}

/// <summary>
/// Cuerpo de api/administracion/codigos-caja/exportar-excel: los id de las combinaciones que la persona ve en la tabla
/// (ya filtradas). El Excel sale solo con esas filas; la empresa sale del token.
/// </summary>
public sealed class ExportarCodigosCajaRequest
{
    public List<long> Ids { get; set; } = [];
}

/// <summary>Cuerpo de los listados de administracion (matrices, reglas de sobrepeso). Ambos campos son opcionales.</summary>
public sealed class ListarAdministracionRequest
{
    public string? IdProyecto { get; set; }
    public string? CodigoCultivo { get; set; }
}
