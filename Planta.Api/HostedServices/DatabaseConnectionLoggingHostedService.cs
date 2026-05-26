using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Planta.Infrastructure.Persistence;

namespace Planta.Api.HostedServices;

public sealed class DatabaseConnectionLoggingHostedService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseConnectionLoggingHostedService> logger,
    IConfiguration configuration) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogError("No se encontro la cadena de conexion 'DefaultConnection'. No se puede validar conexion a BD.");
            return;
        }

        string databaseName;
        try
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            databaseName = builder.InitialCatalog;
        }
        catch
        {
            databaseName = "(desconocida)";
        }

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SistemaPaletsDbContext>();

            var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                logger.LogError("No se pudo conectar a la base de datos '{DatabaseName}'.", databaseName);
                return;
            }

            logger.LogInformation("Se conecto a la base de datos '{DatabaseName}'.", databaseName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al validar conexion a la base de datos '{DatabaseName}'.", databaseName);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
