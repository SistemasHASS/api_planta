using System.Data;
using System.Security.Cryptography;
using System.Text;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Repository;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>
/// Sesiones y refresh tokens en SQL Server. Los procedimientos estan en BD/sp/PLANTA_SesionesUsuario.sql.
/// El refresh token se guarda como hash SHA-256; el valor original nunca llega a la base de datos.
/// </summary>
public sealed class AuthRepositoryImpl(ApplicationDbContext context) : BaseRepository(context), IAuthRepository
{
    public async Task<Guid> CreateAsync(string refreshToken, UsuarioDto user, ClientSessionInfo client, DateTime refreshExpiresAtUtc, DateTime sessionExpiresAtUtc, CancellationToken cancellationToken = default)
    {
        var sessionId = Guid.NewGuid();

        await EjecutarAsync("PLANTA_SesionesUsuario_Crear", command =>
        {
            AgregarDatosSesion(command, sessionId, user, client, refreshExpiresAtUtc);
            Agregar(command, "@refreshTokenHash", Hash(refreshToken), DbType.Binary);
            Agregar(command, "@ipCreacion", client.IpAddress);
            Agregar(command, "@fechaExpiracionSesionUtc", sessionExpiresAtUtc, DbType.DateTime2);
        }, cancellationToken);

        return sessionId;
    }

    public async Task<UserSession?> FindActiveAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var sesiones = await EjecutarListaAsync(
            "PLANTA_SesionesUsuario_ObtenerActivaPorRefreshHash",
            command => Agregar(command, "@refreshTokenHash", Hash(refreshToken), DbType.Binary),
            record => new UserSession
            {
                SessionId = record.GetGuid(record.GetOrdinal("idSesion")),
                User = new UsuarioDto
                {
                    Id = LeerTexto(record, "idUsuario"),
                    IdEmpresa = LeerTexto(record, "idEmpresa"),
                    Ruc = LeerTexto(record, "ruc"),
                    Usuario = LeerTexto(record, "usuario"),
                    IdRol = LeerTexto(record, "idRol"),
                    Nombre = LeerTexto(record, "nombre"),
                    DocumentoIdentidad = LeerTexto(record, "documentoIdentidad"),
                    IdProyecto = LeerTexto(record, "idProyecto"),
                    CodigoAcopio = LeerTexto(record, "codigoAcopio"),
                    AcopioNombre = LeerTexto(record, "acopioNombre"),
                    SerieGuia = LeerTexto(record, "serieGuia")
                },
                Client = new ClientSessionInfo
                {
                    DeviceId = LeerTexto(record, "deviceId") ?? string.Empty,
                    ClientType = LeerTexto(record, "tipoCliente") ?? "WEB",
                    DeviceName = LeerTexto(record, "nombreDispositivo"),
                    IpAddress = LeerTexto(record, "ipUltimoUso"),
                    UserAgent = LeerTexto(record, "userAgent")
                },
                // El SP devuelve esta columna ya convertida a UTC; ADO.NET la entrega sin "Kind",
                // por eso se marca explicitamente como UTC.
                SessionExpiresAtUtc = DateTime.SpecifyKind(
                    record.GetDateTime(record.GetOrdinal("fechaExpiracionSesionUtc")),
                    DateTimeKind.Utc)
            },
            cancellationToken);

        return sesiones.FirstOrDefault();
    }

    public async Task<Guid?> RotateAsync(string refreshToken, string newRefreshToken, UsuarioDto user, ClientSessionInfo client, DateTime expiresAtUtc, CancellationToken cancellationToken = default)
    {
        var sessionId = Guid.NewGuid();

        var value = await EjecutarEscalarAsync("PLANTA_SesionesUsuario_Rotar", command =>
        {
            Agregar(command, "@refreshTokenHashAnterior", Hash(refreshToken), DbType.Binary);
            AgregarDatosSesion(command, sessionId, user, client, expiresAtUtc);
            Agregar(command, "@refreshTokenHashNuevo", Hash(newRefreshToken), DbType.Binary);
            Agregar(command, "@ipUltimoUso", client.IpAddress);
        }, cancellationToken);

        return value is Guid id ? id : null;
    }

    public Task RevokeAsync(string refreshToken, string reason, CancellationToken cancellationToken = default)
        => EjecutarAsync("PLANTA_SesionesUsuario_RevocarPorRefreshHash", command =>
        {
            Agregar(command, "@refreshTokenHash", Hash(refreshToken), DbType.Binary);
            Agregar(command, "@motivo", reason);
        }, cancellationToken);

    public async Task<IReadOnlyList<ActiveSession>> ListActiveAsync(string userId, string companyId, CancellationToken cancellationToken = default)
    {
        var sesiones = await EjecutarListaAsync(
            "PLANTA_SesionesUsuario_ListarActivas",
            command =>
            {
                Agregar(command, "@idUsuario", userId);
                Agregar(command, "@idEmpresa", companyId);
            },
            record => new ActiveSession
            {
                SessionId = record.GetGuid(record.GetOrdinal("idSesion")),
                Username = LeerTexto(record, "usuario"),
                FullName = LeerTexto(record, "nombre"),
                ClientType = LeerTexto(record, "tipoCliente") ?? "WEB",
                DeviceName = LeerTexto(record, "nombreDispositivo"),
                IpAddress = LeerTexto(record, "ipUltimoUso"),
                CreatedAt = record.GetDateTime(record.GetOrdinal("fechaCreacion")),
                LastUsedAt = record.GetDateTime(record.GetOrdinal("ultimoUso")),
                ExpiresAt = record.GetDateTime(record.GetOrdinal("fechaExpiracion")),
                SessionExpiresAt = record.GetDateTime(record.GetOrdinal("fechaExpiracionSesion"))
            },
            cancellationToken);

        return sesiones;
    }

    public async Task<bool> RevokeByIdAsync(Guid sessionId, string userId, string companyId, string reason, CancellationToken cancellationToken = default)
    {
        var value = await EjecutarEscalarAsync("PLANTA_SesionesUsuario_RevocarPorId", command =>
        {
            Agregar(command, "@idSesion", sessionId, DbType.Guid);
            Agregar(command, "@idUsuario", userId);
            Agregar(command, "@idEmpresa", companyId);
            Agregar(command, "@motivo", reason);
        }, cancellationToken);

        return value is bool revoked && revoked;
    }

    public Task RevokeAllAsync(string userId, string companyId, string reason, CancellationToken cancellationToken = default)
        => EjecutarAsync("PLANTA_SesionesUsuario_RevocarTodas", command =>
        {
            Agregar(command, "@idUsuario", userId);
            Agregar(command, "@idEmpresa", companyId);
            Agregar(command, "@motivo", reason);
        }, cancellationToken);

    // Parametros que comparten Crear y Rotar.
    private static void AgregarDatosSesion(IDbCommand command, Guid sessionId, UsuarioDto user, ClientSessionInfo client, DateTime refreshExpiresAtUtc)
    {
        var userId = user.Id ?? user.DocumentoIdentidad;
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(user.IdEmpresa))
        {
            throw new BadRequestException("La cuenta no tiene identidad o empresa para registrar la sesion.");
        }

        Agregar(command, "@idSesion", sessionId, DbType.Guid);
        Agregar(command, "@idUsuario", userId);
        Agregar(command, "@idEmpresa", user.IdEmpresa);
        Agregar(command, "@ruc", user.Ruc);
        Agregar(command, "@usuario", user.Usuario);
        Agregar(command, "@idRol", user.IdRol);
        Agregar(command, "@nombre", user.Nombre);
        Agregar(command, "@documentoIdentidad", user.DocumentoIdentidad);
        Agregar(command, "@idProyecto", user.IdProyecto);
        Agregar(command, "@codigoAcopio", user.CodigoAcopio);
        Agregar(command, "@acopioNombre", user.AcopioNombre);
        Agregar(command, "@serieGuia", user.SerieGuia);
        Agregar(command, "@deviceId", client.DeviceId);
        Agregar(command, "@tipoCliente", client.ClientType);
        Agregar(command, "@nombreDispositivo", client.DeviceName);
        Agregar(command, "@userAgent", client.UserAgent);
        Agregar(command, "@fechaExpiracionUtc", refreshExpiresAtUtc, DbType.DateTime2);
    }

    private static byte[] Hash(string refreshToken)
        => SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));
}
