using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Planta.Domain.Repository;
using Planta.Domain.Service;
using Planta.Infrastructure.Persistence;
using Planta.Infrastructure.RepositoryImpl;
using Planta.Infrastructure.ServiceImpl;

namespace Planta.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registra la base de datos, las integraciones externas, los repositorios y los servicios.
    /// Al crear un modulo nuevo, agregar aqui una linea:
    /// services.AddScoped&lt;IXxxRepository, XxxRepositoryImpl&gt;();
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Base de datos: si no hay cadena de conexion la API levanta igual (ver DatabaseStartupCheck).
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));
        }

        // apimaestra: la URL base vive solo en appsettings (ExternalApis:Maestros:BaseUrl)
        var maestrosUrl = configuration["ExternalApis:Maestros:BaseUrl"];
        if (string.IsNullOrWhiteSpace(maestrosUrl))
        {
            throw new InvalidOperationException("Falta configurar ExternalApis:Maestros:BaseUrl en appsettings.");
        }

        services.AddHttpClient(MaestrosServiceImpl.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(maestrosUrl);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        });

        // ApiDocumentosElectronicos (emision de guias ante SUNAT): opcional. Sin URL, emitir devuelve error controlado.
        // Igual que el sistema anterior, se aceptan certificados no validos (ExternalApis:DocumentosElectronicos:IgnorarErroresSsl, por defecto true).
        var documentosUrl = configuration["ExternalApis:DocumentosElectronicos:BaseUrl"];
        if (!string.IsNullOrWhiteSpace(documentosUrl))
        {
            var ignorarSsl = !bool.TryParse(configuration["ExternalApis:DocumentosElectronicos:IgnorarErroresSsl"], out var valor) || valor;
            var cliente = services.AddHttpClient(DocumentosElectronicosServiceImpl.HttpClientName, client =>
            {
                client.BaseAddress = new Uri(documentosUrl);
                client.Timeout = TimeSpan.FromSeconds(120);
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            });
            if (ignorarSsl)
            {
                cliente.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                });
            }
        }

        // Servicios
        services.AddScoped<IMaestrosService, MaestrosServiceImpl>();
        services.AddScoped<ITokenService, JwtTokenServiceImpl>();
        services.AddScoped<ICatalogosLocalService, CatalogosLocalServiceImpl>();
        services.AddScoped<IProcesosService, ProcesosServiceImpl>();
        services.AddScoped<IJornadasAnotadorService, JornadasAnotadorServiceImpl>();
        services.AddScoped<IReportesService, ReportesServiceImpl>();
        services.AddScoped<IPaletsService, PaletsServiceImpl>();
        services.AddScoped<IAdministracionService, AdministracionServiceImpl>();
        services.AddScoped<IGuiasService, GuiasServiceImpl>();
        services.AddScoped<IGuiasManualesService, GuiasManualesServiceImpl>();
        services.AddScoped<IDocumentosElectronicosService, DocumentosElectronicosServiceImpl>();
        services.AddSingleton<IPdfService, PdfServiceImpl>();
        services.AddSingleton<IExcelService, ExcelServiceImpl>();

        // Repositorios
        services.AddScoped<IAuthRepository, AuthRepositoryImpl>();
        services.AddScoped<IUsuarioAcopioRepository, UsuarioAcopioRepositoryImpl>();
        services.AddScoped<ICatalogosLocalRepository, CatalogosLocalRepositoryImpl>();
        services.AddScoped<IProcesosRepository, ProcesosRepositoryImpl>();
        services.AddScoped<IJornadasAnotadorRepository, JornadasAnotadorRepositoryImpl>();
        services.AddScoped<IReportesRepository, ReportesRepositoryImpl>();
        services.AddScoped<IPaletsRepository, PaletsRepositoryImpl>();
        services.AddScoped<IAdministracionRepository, AdministracionRepositoryImpl>();
        services.AddScoped<IGuiasRepository, GuiasRepositoryImpl>();
        services.AddScoped<IGuiasManualesRepository, GuiasManualesRepositoryImpl>();

        return services;
    }
}
