using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using api_planta.Infraestructure.Persistence;
using api_planta.Domain.Repository;
using api_planta.Domain.Services;
using api_planta.Domain.UseCase;
using api_planta.Infraestructure.RepositoryImpl;
using api_planta.Infraestructure.ServiceImpl;
using api_planta.Application.Usecase;
using api_planta.Infraestructure.Shared.Exceptions;
using Serilog;

// ── Serilog ──
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File(
        @"C:\LogPaletsAPI\log-.txt",
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
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    // Controladores
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        });

    // DbContext con retry
    builder.Services.AddDbContextPool<SistemaPaletsDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
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
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });

    // Inyección de dependencias - Clean Architecture
    builder.Services.AddScoped<IPaletRepository, PaletRepositoryImpl>();
    builder.Services.AddScoped<IPaletService, PaletServiceImpl>();
    builder.Services.AddScoped<IPaletUseCase, PaletUseCaseImpl>();

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
    app.UseCors("AllowAll");

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
