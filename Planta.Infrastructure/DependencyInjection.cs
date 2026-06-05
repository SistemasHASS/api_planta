using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth;
using Planta.Application.Administracion;
using Planta.Application.Administracion.Abstractions;
using Planta.Application.Catalogos.Abstractions;
using Planta.Application.Catalogos;
using Planta.Application.Maestros.Abstractions;
using Planta.Infrastructure.Persistence;
using Planta.Infrastructure.RepositoryImpl;
using Planta.Infrastructure.ServiceImpl;
using Planta.Application.Proceso.Abstractions;
using Planta.Application.Proceso;
using Planta.Application.GuiaRemision.Abstractions;
using Planta.Application.GuiaRemision;

namespace Planta.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(defaultConnectionString))
        {
            throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection' en la configuracion.");
        }

        services.AddDbContextPool<SistemaPaletsDbContext>(options =>
            options.UseSqlServer(
                defaultConnectionString,
                sql => sql.EnableRetryOnFailure()));

        services.AddHttpClient();

        services.AddScoped<IProcesosService, ProcesosServiceImpl>();
        services.AddScoped<IProcesosUseCase, ProcesosUseCase>();
        services.AddScoped<IProcesosRepository, ProcesosRespository>();

        services.AddScoped<IGuiasRemisionService, GuiasRemisionServiceImpl>();
        services.AddScoped<IGuiasRemisionUseCase, GuiasRemisionUseCase>();
        services.AddScoped<IGuiasRemisionRepository, GuiasRemisionRepository>();

        services.AddScoped<IMaestrosService, MaestrosServiceImpl>();
        services.AddScoped<ICatalogosService,CatalogosServiceImpl>();
        services.AddScoped<ICatalogosUseCase, CatalogosUseCase>();
        services.AddScoped<ICatalogosRepository, CatalogosRepository>();

        services.AddScoped<IAdministracionUseCase, AdministracionUseCase>();
        services.AddScoped<IAdministracionService, AdministracionServiceImpl>();
        services.AddScoped<IAdministracionRepository, AdministracionRepository>();

        services.AddScoped<IMaestrosAuthService, MaestrosAuthServiceImpl>();
        services.AddScoped<ITokenService, JwtTokenServiceImpl>();
        services.AddScoped<IAuthUseCase, AuthUseCase>();
        services.AddScoped<IAuthService, AuthServiceImpl>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        return services;
    }
}
