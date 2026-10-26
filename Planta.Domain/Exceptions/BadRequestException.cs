namespace Planta.Domain.Exceptions;

/// <summary>Datos o situacion que no permiten completar la operacion. El middleware de errores responde 400.</summary>
public sealed class BadRequestException(string message) : Exception(message);
