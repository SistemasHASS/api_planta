namespace Planta.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var value)
                ? value?.ToString()
                : null;

            logger.LogError(ex, "Unhandled exception. CorrelationId={CorrelationId}", correlationId);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                error = "An unexpected error occurred.",
                correlationId
            });
        }
    }
}
