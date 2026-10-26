namespace Planta.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "Frontend";

    /// <summary>Origenes permitidos desde Cors:AllowedOrigins. En Development tambien cualquier localhost.</summary>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy.WithOrigins(origins).AllowAnyHeader().WithMethods("GET", "POST");

                if (environment.IsDevelopment())
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback);
                }
            });
        });

        return services;
    }
}
