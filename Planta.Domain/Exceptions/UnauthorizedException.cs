namespace Planta.Domain.Exceptions;

/// <summary>Credenciales o sesion invalidas. El middleware de errores responde 401.</summary>
public sealed class UnauthorizedException(string message) : Exception(message);
