namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de api/procesos/listar: procesos de una campania y cultivo.</summary>
public sealed class ListarProcesosRequest
{
    public string IdProyecto { get; set; } = "";
    public string CodigoCultivo { get; set; } = "";
    /// <summary>Filtros opcionales (solo los usa el listado del administrador). Vacio = sin filtrar. Fechas yyyy-MM-dd.</summary>
    public string FechaDesde { get; set; } = "";
    public string FechaHasta { get; set; } = "";
    public string Turno { get; set; } = "";
    public string CodigoAcopio { get; set; } = "";
    public string Estado { get; set; } = "";
}

/// <summary>Cuerpo de api/procesos/buscar: ya existe un proceso para esa fecha y turno?</summary>
public sealed class BuscarProcesoRequest
{
    public string IdProyecto { get; set; } = "";
    public string CodigoCultivo { get; set; } = "";
    public string Turno { get; set; } = "";
    /// <summary>Fecha del proceso, formato yyyy-MM-dd.</summary>
    public string Fecha { get; set; } = "";
}

/// <summary>Cuerpo de las consultas de personal disponible: quien no esta asignado a otro proceso en esa fecha.</summary>
public sealed class PersonalDisponibleRequest
{
    public string IdProyecto { get; set; } = "";
    /// <summary>Formato yyyy-MM-dd.</summary>
    public string Fecha { get; set; } = "";
}

/// <summary>Cuerpo de api/procesos/sincronizar. Los tres json son texto JSON que el SP lee tal cual.</summary>
public sealed class SincronizarProcesoRequest
{
    /// <summary>nuevo, cerrar, reabrir o editar.</summary>
    public string Modo { get; set; } = "nuevo";
    public string IdProyecto { get; set; } = "";
    public string CodigoCultivo { get; set; } = "";
    public string JsonProceso { get; set; } = "{}";
    public string JsonDprocesoLogisticos { get; set; } = "[]";
    public string JsonDprocesoSupervisores { get; set; } = "[]";
}

/// <summary>Cuerpo de api/procesos/jornadas/listar: las jornadas de un proceso. El administrador elige el acopio; los demás usan el de su token.</summary>
public sealed class JornadasProcesoRequest
{
    public string CodigoAcopio { get; set; } = "";
    public string IdProceso { get; set; } = "";
}

/// <summary>Cuerpo de api/procesos/jornadas/reabrir: la jornada que se vuelve a abrir (solo administrador).</summary>
public sealed class ReabrirJornadaRequest
{
    public int Id { get; set; }
}
