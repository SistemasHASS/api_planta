using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text;
using api_planta.Infrastructure.Persistence;
using api_planta.Domain.Repository;
using api_planta.Domain.Services;
using api_planta.Domain.UseCase;
using api_planta.Infrastructure.RepositoryImpl;
using api_planta.Infrastructure.ServiceImpl;
using api_planta.Application.Usecase;
using api_planta.Api.Security;
using api_planta.Infrastructure.Shared.Exceptions;
using Serilog;

// ── Serilog ──
var logDir = Path.Combine(AppContext.BaseDirectory, "LogPaletsApi");
Directory.CreateDirectory(logDir);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(
        Path.Combine(logDir, "log-.txt"),
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {RequestPath} {Message:lj}{NewLine}{Exception}")
    .WriteTo.Console()
    .MinimumLevel.Debug()
    .CreateLogger();

AppDomain.CurrentDomain.UnhandledException += (_, e) =>
{
    if (e.ExceptionObject is Exception ex)
    {
        Log.Fatal(ex, "AppDomain.CurrentDomain.UnhandledException");
        return;
    }
    Log.Fatal("AppDomain.CurrentDomain.UnhandledException no administrada");
};

TaskScheduler.UnobservedTaskException += (_, e) =>
{
    Log.Error(e.Exception, "TaskScheduler.UnobservedTaskException");
    e.SetObserved();
};

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AngularPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
        });
    });

    // Controladores
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        });

    var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(defaultConnectionString))
    {
        throw new InvalidOperationException("No se encontro la cadena de conexion 'DefaultConnection' en la configuracion.");
    }

    // DbContext con retry
    builder.Services.AddDbContextPool<SistemaPaletsDbContext>(options =>
        options.UseSqlServer(
            defaultConnectionString,
            sql => sql.EnableRetryOnFailure()));

    // JWT Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (string.IsNullOrEmpty(context.Token)
                        && context.Request.Cookies.TryGetValue("access_token", out var cookieToken)
                        && !string.IsNullOrWhiteSpace(cookieToken))
                    {
                        context.Token = cookieToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();

    // Inyección de dependencias - Clean Architecture
    builder.Services.AddScoped<IPaletRepository, PaletRepositoryImpl>();
    builder.Services.AddScoped<ICatalogosRepository, CatalogosRepositoryImpl>();
    builder.Services.AddScoped<IAdministracionRepository, AdministracionRepositoryImpl>();
    builder.Services.AddScoped<IPaletService, PaletServiceImpl>();
    builder.Services.AddScoped<ICatalogosService, CatalogosServiceImpl>();
    builder.Services.AddScoped<IAdministracionService, AdministracionServiceImpl>();
    builder.Services.AddScoped<IPaletUseCase, PaletUseCaseImpl>();
    builder.Services.AddScoped<ICatalogosUsaCase, CatalogosUseCaseImpl>();
    builder.Services.AddScoped<IAdministracionUseCase, AdministracionUseCaseImpl>();

    builder.Services.AddScoped<IMaestrosAuthService, MaestrosServiceImpl>();
    builder.Services.AddScoped<ITokenService, JwtTokenServiceImpl>();
    builder.Services.AddScoped<IAuthUseCase, AuthUseCaseImpl>();
    builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
    builder.Services.AddScoped<IAuthRepository, AuthRepository>();


    // Swagger con soporte JWT Bearer
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Sistema Gestión Logística API",
            Version = "v1",
            Description = "Documentación de la API / Sistema de Gestión Logística."
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Introduce el token en el formato: Bearer {token}"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    // CORS antes que autenticación
    app.UseCors("AngularPolicy");

    // Middleware de excepciones globales
    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    // Swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Gestión Logística API v1");
        c.DocumentTitle = "Sistema Gestión Logística API Docs";
    });

    app.MapControllers();

    await ProbarConexionBaseDatosAsync(app.Services, defaultConnectionString);

    // Endpoint minimal API para personal disponible
    app.MapGet("/api/procesos/personal-disponible", async (
        string fecha, 
        string turno, 
        ILogger<Program> logger) =>
    {
        logger.LogInformation("[MinimalAPI/personal-disponible] Fecha: {Fecha}, Turno: {Turno}", fecha, turno);
        
        try
        {
            if (string.IsNullOrEmpty(fecha) || string.IsNullOrEmpty(turno))
            {
                return Results.BadRequest(new { success = false, message = "Fecha y turno son requeridos" });
            }

            // Llamar al SP directamente usando ADO.NET
            var connectionString = app.Services.GetRequiredService<IConfiguration>()
                .GetConnectionString("DefaultConnection");
            
            using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            await connection.OpenAsync();
            
            using var command = new Microsoft.Data.SqlClient.SqlCommand("SP_Proceso_ObtenerPersonalDisponible", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            
            var jsonParam = new Microsoft.Data.SqlClient.SqlParameter("@json", System.Data.SqlDbType.NVarChar, -1);
            jsonParam.Value = System.Text.Json.JsonSerializer.Serialize(new { fecha, turno });
            command.Parameters.Add(jsonParam);
            
            var result = await command.ExecuteScalarAsync();
            var jsonResult = result?.ToString();
            
            if (!string.IsNullOrEmpty(jsonResult))
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(jsonResult);
                var supervisores = data.GetProperty("supervisores");
                var logisticos = data.GetProperty("logisticos");
                return Results.Ok(new { success = true, supervisores, logisticos });
            }

            return Results.Ok(new { success = true, supervisores = new object[] {}, logisticos = new object[] {} });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[MinimalAPI/personal-disponible] Error");
            return Results.StatusCode(500);
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static async Task ProbarConexionBaseDatosAsync(IServiceProvider services, string connectionString)
{
    var (server, database) = ObtenerDatosConexion(connectionString);
    using var scope = services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseConnection");

    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SistemaPaletsDbContext>();
        await dbContext.Database.OpenConnectionAsync();
        await dbContext.Database.CloseConnectionAsync();

        var mensajeOk = $"Se conecto a la BD {database} en {server}.";
        Console.WriteLine(mensajeOk);
        logger.LogInformation(mensajeOk);
    }
    catch (Exception ex)
    {
        var mensajeError = $"Error al conectar a la BD {database} en {server}. Error: {ex.Message}";
        Console.WriteLine(mensajeError);
        logger.LogError(ex, "Error al conectar a la BD {Database} en {Server}", database, server);
    }
}

static (string Server, string Database) ObtenerDatosConexion(string connectionString)
{
    try
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var server = string.IsNullOrWhiteSpace(builder.DataSource) ? "servidor no especificado" : builder.DataSource;
        var database = string.IsNullOrWhiteSpace(builder.InitialCatalog) ? "base de datos no especificada" : builder.InitialCatalog;
        return (server, database);
    }
    catch
    {
        return ("servidor no especificado", "base de datos no especificada");
    }
}
