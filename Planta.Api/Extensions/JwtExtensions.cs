using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Planta.Domain.Constants;

namespace Planta.Api.Extensions;

public static class JwtExtensions
{
    /// <summary>
    /// Autenticacion JWT Bearer. Por defecto TODO endpoint exige token (FallbackPolicy);
    /// solo los marcados con [AllowAnonymous] quedan abiertos (login, refresh, logout, /health).
    /// </summary>
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var jwtKey = configuration["Jwt:Key"];
        if (!environment.IsDevelopment())
        {
            ValidarClave(jwtKey);
        }
        else if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("Falta Jwt:Key (appsettings.Development.json).");
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
                    ClockSkew = TimeSpan.Zero
                };

                // Ademas de la expiracion normal del token, se valida el tope absoluto de la sesion.
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var claim = context.Principal?.FindFirst(ClaimNames.SesionExpiraUtc)?.Value;
                        if (!DateTime.TryParse(claim, CultureInfo.InvariantCulture,
                                DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var sessionExpiry)
                            || sessionExpiry <= DateTime.UtcNow)
                        {
                            context.Fail("La sesion expiro.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return services;
    }

    private static void ValidarClave(string? key)
    {
        if (string.IsNullOrWhiteSpace(key)
            || key.StartsWith("CAMBIAR-", StringComparison.OrdinalIgnoreCase)
            || Encoding.UTF8.GetByteCount(key) < 32)
        {
            throw new InvalidOperationException(
                "Jwt:Key invalida. Configurar una clave real de al menos 32 bytes (variable de entorno Jwt__Key).");
        }
    }
}
