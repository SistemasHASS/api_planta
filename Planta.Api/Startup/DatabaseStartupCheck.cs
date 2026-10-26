using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Planta.Infrastructure.Persistence;

namespace Planta.Api.Startup;

/// <summary>
/// Al arrancar, prueba la conexion a la BD y lo deja claro en el log:
///   OK    -> "Conexion a BD correcta | Servidor | Base de datos | ms"
///   ERROR -> una sola linea con el motivo (sin stack trace) y la API se detiene.
/// Nunca se escribe la contrasena en el log.
/// </summary>
public sealed class DatabaseStartupCheck(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    IHostApplicationLifetime lifetime,
    ILogger<DatabaseStartupCheck> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("BD NO configurada: falta ConnectionStrings:DefaultConnection. La API levanta sin base de datos.");
            return;
        }

        var servidor = LeerValor(connectionString, "Data Source", "Server") ?? "?";
        var baseDatos = LeerValor(connectionString, "Initial Catalog", "Database") ?? "?";

        if (connectionString.Contains("CAMBIAR-", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError(
                "La cadena de conexion todavia tiene valores de ejemplo (CAMBIAR-...). " +
                "Configurala con 'dotnet user-secrets set \"ConnectionStrings:DefaultConnection\" \"...\"' o en appsettings.");
            lifetime.StopApplication();
            return;
        }

        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var connection = dbContext.Database.GetDbConnection();
        var reloj = Stopwatch.StartNew();

        try
        {
            await connection.OpenAsync(cancellationToken);
            logger.LogInformation(
                "Conexion a BD correcta | Base de datos: {BaseDatos} | {Ms} ms",
                 baseDatos, reloj.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            logger.LogError(
                "No se pudo conectar a la BD | Servidor: {Servidor} | Base de datos: {BaseDatos} | Motivo: {Motivo}",
                servidor, baseDatos, ex.GetBaseException().Message);
            lifetime.StopApplication();
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static string? LeerValor(string connectionString, params string[] keys)
    {
        try
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            foreach (var key in keys)
            {
                if (builder.TryGetValue(key, out var value) && value is not null)
                {
                    return value.ToString();
                }
            }
        }
        catch (ArgumentException)
        {
            // Cadena mal formada: se informa "?" y el error real saldra al intentar conectar.
        }

        return null;
    }
}
