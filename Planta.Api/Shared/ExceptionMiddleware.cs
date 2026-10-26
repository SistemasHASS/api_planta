using Planta.Domain.Exceptions;

namespace Planta.Api.Shared;

/// <summary>
/// Unico lugar donde una excepcion se convierte en respuesta HTTP:
/// UnauthorizedException -> 401, BadRequestException -> 400, NotFoundException -> 404, otras -> 500.
/// El cuerpo siempre es { "message": "..." }.
/// </summary>
public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // El cliente cancelo la peticion: no es un error.
        }
        catch (Exception ex)
        {
            var (status, message) = ex switch
            {
                UnauthorizedException => (StatusCodes.Status401Unauthorized, ex.Message),
                BadRequestException => (StatusCodes.Status400BadRequest, ex.Message),
                NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor.")
            };

            if (status == StatusCodes.Status500InternalServerError)
            {
                logger.LogError(ex, "Error no controlado en {Method} {Path}", context.Request.Method, context.Request.Path);
            }

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new { message });
        }
    }
}
