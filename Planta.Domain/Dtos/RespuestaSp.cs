namespace Planta.Domain.Dtos;

/// <summary>Envelope estandar de los SP de PLANTA: error, data (JSON) y mensaje.</summary>
public sealed class RespuestaSp<T>
{
    public bool Error { get; set; }
    public T? Data { get; set; }
    public string Mensaje { get; set; } = "";
}
