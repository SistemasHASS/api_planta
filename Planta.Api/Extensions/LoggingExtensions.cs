using Serilog;

namespace Planta.Api.Extensions;

public static class LoggingExtensions
{
    /// <summary>Serilog a consola y a archivo diario Logs/log-AAAAMMDD.txt (31 dias).</summary>
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 31));

        return builder;
    }
}
