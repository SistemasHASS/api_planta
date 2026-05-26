using System.Diagnostics;

namespace Planta.Api.Middlewares;

public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var value)
            ? value?.ToString()
            : null;

        logger.LogInformation(
            "HTTP {Method} {Path} started. CorrelationId={CorrelationId}",
            context.Request.Method,
            context.Request.Path.Value,
            correlationId);

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            logger.LogInformation(
                "HTTP {Method} {Path} finished with {StatusCode} in {ElapsedMs}ms. CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }
    }
}
