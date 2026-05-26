using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Planta.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>());
        return services;
    }
}

internal sealed class ApplicationAssemblyMarker
{
}
