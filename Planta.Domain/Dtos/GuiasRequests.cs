namespace Planta.Domain.Dtos;

/// <summary>Crea guias de remision con sus palets. <c>Guias</c> es el JSON (una guia o una lista) que lee el SP.</summary>
public sealed class SincronizarGuiasRequest
{
    public string IdProyecto { get; set; } = "";
    public string Guias { get; set; } = "";
}

/// <summary>Edita una guia de remision. <c>Guias</c> es el JSON que lee el SP.</summary>
public sealed class EditarGuiaRequest
{
    public string IdProyecto { get; set; } = "";
    public string Guias { get; set; } = "";
}

/// <summary>Filtros del listado de guias. Estado, fechas y texto son opcionales.</summary>
public class ListarGuiasRequest
{
    public string IdProyecto { get; set; } = "";
    public string? Estado { get; set; }
    public string? FechaDesde { get; set; }
    public string? FechaHasta { get; set; }
    public string? Texto { get; set; }
}

/// <summary>Igual que el listado, mas el cultivo (sirve para traducir variedades y calibres en el Excel).</summary>
public sealed class ExportarGuiasRequest : ListarGuiasRequest
{
    public string? CodigoCultivo { get; set; }
}

/// <summary>Una guia de remision de una campania.</summary>
public sealed class ProcesosGuiaRequest
{
    public string IdProyecto { get; set; } = "";
}

/// <summary>Una guia de remision de una campania.</summary>
public sealed class GuiaRequest
{
    public string IdProyecto { get; set; } = "";
    public string CodigoGuiaRemision { get; set; } = "";
}

/// <summary>Anula una guia. El acopio se envia porque el administrador no tiene acopio en su token; si viene vacio se usa el del token.</summary>
public sealed class AnularGuiaRequest
{
    public string IdProyecto { get; set; } = "";
    public string CodigoGuiaRemision { get; set; } = "";
    public string? CodigoAcopio { get; set; }
}
