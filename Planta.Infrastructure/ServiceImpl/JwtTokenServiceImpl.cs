using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Planta.Application.Auth.Abstractions;
using Planta.Application.Auth.Models;

namespace Planta.Infrastructure.ServiceImpl;

public sealed class JwtTokenServiceImpl(IConfiguration configuration) : ITokenService
{
    public string CrearToken(UsuarioAcopioDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.id ?? ""),
            new(ClaimTypes.Name, user.usuario ?? ""),
            new("NombreCompleto", user.nombre ?? ""),
            new("IdEmpresa", user.idempresa ?? ""),
            new("Ruc", user.ruc ?? ""),
            new(ClaimTypes.Role, user.idRol ?? ""),
            new("codigoAcopio", user.codigoAcopio.ToString()),
            new("SerieGuia", user.serieGuia ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
