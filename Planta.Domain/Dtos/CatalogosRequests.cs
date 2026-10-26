namespace Planta.Domain.Dtos;

/// <summary>Cuerpo de los catalogos que se consultan por campania: { "idProyecto": "013" }.</summary>
public sealed class ProyectoRequest
{
    public string IdProyecto { get; set; } = "";
}

/// <summary>Cuerpo de los catalogos que se consultan por cultivo: { "codigoCultivo": "0802" }.</summary>
public sealed class CultivoRequest
{
    public string CodigoCultivo { get; set; } = "";
}

/// <summary>Provincias de un departamento (codigo de 2 digitos).</summary>
public sealed class ProvinciasRequest
{
    public string CodigoDepartamento { get; set; } = "";
}

/// <summary>Distritos de una provincia (codigos de departamento y provincia, 2 digitos cada uno).</summary>
public sealed class DistritosRequest
{
    public string CodigoDepartamento { get; set; } = "";
    public string CodigoProvincia { get; set; } = "";
}
