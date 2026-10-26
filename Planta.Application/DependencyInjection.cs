using Microsoft.Extensions.DependencyInjection;
using Planta.Application.UseCaseImpl;
using Planta.Domain.UseCase;

namespace Planta.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registra los casos de uso. Al crear un modulo nuevo, agregar aqui una linea:
    /// services.AddScoped&lt;IXxxUseCase, XxxUseCaseImpl&gt;();
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthUseCase, AuthUseCaseImpl>();
        services.AddScoped<IMaestrosUseCase, MaestrosUseCaseImpl>();
        services.AddScoped<ICatalogosLocalUseCase, CatalogosLocalUseCaseImpl>();
        services.AddScoped<IProcesosUseCase, ProcesosUseCaseImpl>();
        services.AddScoped<IReportesUseCase, ReportesUseCaseImpl>();
        services.AddScoped<IPaletsUseCase, PaletsUseCaseImpl>();
        services.AddScoped<IAdministracionUseCase, AdministracionUseCaseImpl>();
        services.AddScoped<IGuiasUseCase, GuiasUseCaseImpl>();
        services.AddScoped<IGuiasManualesUseCase, GuiasManualesUseCaseImpl>();

        return services;
    }
}
