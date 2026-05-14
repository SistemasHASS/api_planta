using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_planta.Domain.DTOs.Auth;
using api_planta.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace api_planta.Infrastructure.ServiceImpl;

public class JwtTokenServiceImpl : ITokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenServiceImpl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CrearToken(UsuarioAcopioDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.id.ToString() ?? ""),
            new(ClaimTypes.Name, user.usuario ?? ""),
            new("NombreCompleto", user.nombre ?? ""),
            new("IdEmpresa", user.idempresa ?? ""),
            new("Ruc", user.ruc ?? ""),
            new(ClaimTypes.Role, user.idRol ?? ""),
            new("AcopioId", user.acopioId.ToString() ?? ""),
            new("SerieGuia", user.serieGuia ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
