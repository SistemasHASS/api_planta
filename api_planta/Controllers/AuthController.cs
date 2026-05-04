using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_planta.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api_planta.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SistemaPaletsDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, SistemaPaletsDbContext context, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                await _context.Database.OpenConnectionAsync();
                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SP_Auth_Login";
                command.CommandType = CommandType.StoredProcedure;

                var param = command.CreateParameter();
                param.ParameterName = "@Usuario";
                param.DbType = DbType.String;
                param.Value = request.Usuario;
                command.Parameters.Add(param);

                using var reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                {
                    await _context.Database.CloseConnectionAsync();
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
                }

                var storedPassword = reader["Password"].ToString();
                var userId = Convert.ToInt32(reader["Id"]);
                var usuario = reader["Usuario"].ToString();
                var nombreCompleto = reader["NombreCompleto"].ToString();
                var perfil = reader["Perfil"].ToString();
                var acopioId = reader["AcopioId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["AcopioId"]);
                var acopioCodigo = reader["AcopioCodigo"]?.ToString();
                var acopioNombre = reader["AcopioNombre"]?.ToString();

                await _context.Database.CloseConnectionAsync();

                // Validate password using BCrypt
                if (!BCrypt.Net.BCrypt.Verify(request.Password, storedPassword))
                {
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, userId.ToString()),
                    new(ClaimTypes.Name, usuario ?? ""),
                    new("NombreCompleto", nombreCompleto ?? ""),
                    new(ClaimTypes.Role, perfil ?? ""),
                };

                if (acopioId.HasValue)
                    claims.Add(new Claim("AcopioId", acopioId.Value.ToString()));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(8),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user = new
                    {
                        id = userId,
                        usuario,
                        nombreCompleto,
                        perfil,
                        acopioId,
                        acopioCodigo,
                        acopioNombre
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", error = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string Usuario { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
