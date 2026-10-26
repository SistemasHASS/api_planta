namespace Planta.Domain.Exceptions;

/// <summary>El recurso solicitado no existe. El middleware de errores responde 404.</summary>
public sealed class NotFoundException(string message) : Exception(message);
