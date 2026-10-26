using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Planta.Domain.Constants;
using Planta.Domain.Dtos;
using Planta.Domain.Service;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// Crea el access token (JWT de vida corta) y el refresh token (64 bytes aleatorios).
/// Duraciones y clave se leen de la seccion "Jwt" de appsettings.
/// </summary>
public sealed class JwtTokenServiceImpl(IConfiguration configuration) : ITokenService
{
    public (string Token, DateTime ExpiresAtUtc) CreateAccessToken(UsuarioDto user, DateTime sessionExpiresAtUtc)
    {
        // La fecha de sesion viene de SQL sin "Kind": se asegura que se trate como UTC.
        var normalizedSessionExpiryUtc = sessionExpiresAtUtc.Kind == DateTimeKind.Utc
            ? sessionExpiresAtUtc
            : DateTime.SpecifyKind(sessionExpiresAtUtc, DateTimeKind.Utc);

        // El access token nunca vive mas que la sesion.
        var configuredExpiryUtc = DateTime.UtcNow.AddMinutes(
            GetInt("Jwt:AccessTokenMinutes", GetInt("Jwt:AccessTokenHours", 4) * 60));
        var expiresAtUtc = configuredExpiryUtc <= normalizedSessionExpiryUtc ? configuredExpiryUtc : normalizedSessionExpiryUtc;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id ?? user.DocumentoIdentidad ?? ""),
            new(ClaimTypes.Name, user.Usuario ?? ""),
            new(ClaimTypes.Role, user.IdRol ?? ""),
            new(ClaimNames.NombreCompleto, user.Nombre ?? ""),
            new(ClaimNames.DocumentoIdentidad, user.DocumentoIdentidad ?? ""),
            new(ClaimNames.IdEmpresa, user.IdEmpresa ?? ""),
            new(ClaimNames.Ruc, user.Ruc ?? ""),
            new(ClaimNames.IdProyecto, user.IdProyecto ?? ""),
            new(ClaimNames.SesionExpiraUtc, normalizedSessionExpiryUtc.ToString("O", System.Globalization.CultureInfo.InvariantCulture)),
            new(ClaimNames.Aplicacion, configuration["App:Name"] ?? ""),
            new(ClaimNames.CodigoAcopio, user.CodigoAcopio ?? ""),
            new(ClaimNames.AcopioNombre, user.AcopioNombre ?? ""),
            new(ClaimNames.SerieGuia, user.SerieGuia ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }

    public (string Token, DateTime ExpiresAtUtc) CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return (Convert.ToBase64String(bytes), DateTime.UtcNow.AddMinutes(
            GetInt("Jwt:RefreshTokenMinutes", GetInt("Jwt:RefreshTokenHours", 8) * 60)));
    }

    public DateTime CreateSessionExpiresAtUtc()
        => DateTime.UtcNow.AddMinutes(
            GetInt("Jwt:SessionMaxMinutes", GetInt("Jwt:SessionMaxHours", 24) * 60));

    private int GetInt(string key, int fallback)
        => int.TryParse(configuration[key], out var value) ? value : fallback;
}
