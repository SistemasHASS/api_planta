using System.Text.Json;
using Planta.Application.Shared;
using Planta.Domain.Dtos;
using Planta.Domain.Exceptions;
using Planta.Domain.Service;
using Planta.Domain.UseCase;

namespace Planta.Application.UseCaseImpl;

/// <summary>Toma los datos del usuario del token, arma el JSON del SP (el acopio sale del token, no del cuerpo) y delega al servicio.</summary>
public sealed class JornadasAnotadorUseCaseImpl(IJornadasAnotadorService jornadas, ICurrentUser currentUser) : IJornadasAnotadorUseCase
{
    public Task<JsonElement> ListarAsync(ListarJornadasAnotadorRequest request, CancellationToken cancellationToken = default)
        => jornadas.ListarAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request, cancellationToken);

    public Task<JsonElement> SincronizarAsync(SincronizarJornadaAnotadorRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true);
        var modo = (request.Modo ?? "").Trim().ToLowerInvariant();

        // El nombre de quien abre la jornada sale del token (no del cuerpo).
        var nombreUsuario = currentUser.NombreCompleto?.Trim();

        if (modo == "nuevo")
        {
            Requerir(request.IdProyecto, "idProyecto");
            Requerir(request.IdProceso, "idProceso");
            if (string.IsNullOrEmpty(nombreUsuario)) throw new BadRequestException("No se encontro el nombre del usuario en el token.");
        }
        else if (modo == "editado")
        {
            if (request.Id <= 0) throw new BadRequestException("El campo id es obligatorio para cerrar la jornada.");
        }
        else
        {
            throw new BadRequestException("Modo invalido. Use nuevo o editado.");
        }

        var json = JsonSerializer.Serialize(new[]
        {
            new
            {
                id = request.Id,
                modo,
                idProyecto = (request.IdProyecto ?? "").Trim(),
                codigoAcopio = contexto.CodigoAcopio,
                idProceso = (request.IdProceso ?? "").Trim(),
                nombreUsuario = nombreUsuario ?? string.Empty,
            },
        });
        return jornadas.SincronizarAsync(contexto, json, cancellationToken);
    }

    public Task<JsonElement> ListarLineasAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarLineasAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> SincronizarLineasAsync(SincronizarJornadaAnotadorLineaRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true);
        var modo = (request.Modo ?? "").Trim().ToLowerInvariant();

        if (modo == "nuevo")
        {
            if (request.IdLinea <= 0) throw new BadRequestException("El campo idLinea es obligatorio para tomar una línea.");
        }
        else if (modo == "editado")
        {
            if (request.Id <= 0) throw new BadRequestException("El campo id es obligatorio para soltar una línea.");
        }
        else
        {
            throw new BadRequestException("Modo invalido. Use nuevo o editado.");
        }

        // El nombre que se muestra del anotador sale del token (no del cuerpo).
        var nombre = currentUser.NombreCompleto?.Trim();
        if (modo == "nuevo" && string.IsNullOrEmpty(nombre))
        {
            throw new BadRequestException("No se encontro el nombre del usuario en el token.");
        }

        var json = JsonSerializer.Serialize(new[]
        {
            new
            {
                id = request.Id,
                modo,
                idLinea = request.IdLinea,
                codigoAcopio = contexto.CodigoAcopio,
                nombreAnotador = nombre ?? string.Empty,
            },
        });
        return jornadas.SincronizarLineasAsync(contexto, json, cancellationToken);
    }

    public Task<JsonElement> BuscarTrabajadorAsync(BuscarTrabajadorRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true);
        Requerir(request.NumeroDocumento, "numeroDocumento");
        return jornadas.BuscarTrabajadorAsync(contexto, request.NumeroDocumento.Trim(), cancellationToken);
    }

    public Task<JsonElement> ListarPuestosAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarPuestosAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> SincronizarPuestosAsync(SincronizarJornadaAnotadorPuestoRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true);
        var modo = (request.Modo ?? "").Trim().ToLowerInvariant();

        if (modo == "nuevo")
        {
            if (request.IdLinea <= 0) throw new BadRequestException("El campo idLinea es obligatorio para asignar un empacador.");
            if (request.Puesto <= 0) throw new BadRequestException("El campo puesto es obligatorio para asignar un empacador.");
            Requerir(request.NumeroDocumento, "numeroDocumento");
            Requerir(request.NombreCompleto, "nombreCompleto");
        }
        else if (modo == "mover")
        {
            if (request.Id <= 0) throw new BadRequestException("El campo id es obligatorio para mover un empacador.");
            if (request.IdLinea <= 0) throw new BadRequestException("El campo idLinea es obligatorio para mover un empacador.");
            if (request.Puesto <= 0) throw new BadRequestException("El campo puesto es obligatorio para mover un empacador.");
        }
        else if (modo == "editado")
        {
            if (request.Id <= 0) throw new BadRequestException("El campo id es obligatorio para quitar un empacador.");
        }
        else if (modo == "vaciar")
        {
            if (request.IdLinea <= 0) throw new BadRequestException("El campo idLinea es obligatorio para vaciar una línea.");
        }
        else
        {
            throw new BadRequestException("Modo invalido. Use nuevo, mover, editado o vaciar.");
        }

        var json = JsonSerializer.Serialize(new[]
        {
            new
            {
                id = request.Id,
                modo,
                idLinea = request.IdLinea,
                puesto = request.Puesto,
                numeroDocumento = (request.NumeroDocumento ?? "").Trim(),
                nombreCompleto = (request.NombreCompleto ?? "").Trim(),
                apellidoPaterno = (request.ApellidoPaterno ?? "").Trim(),
                apellidoMaterno = (request.ApellidoMaterno ?? "").Trim(),
                codigoAcopio = contexto.CodigoAcopio,
            },
        });
        return jornadas.SincronizarPuestosAsync(contexto, json, cancellationToken);
    }

    public Task<JsonElement> ListarTiposEmpaquePorMatrizAsync(ListarTiposEmpaquePorMatrizRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true);
        Requerir(request.CodigoCultivo, "codigoCultivo");
        if (request.FormatoId <= 0) throw new BadRequestException("El campo formatoId es obligatorio.");
        return jornadas.ListarTiposEmpaquePorMatrizAsync(contexto, request.CodigoCultivo.Trim(), request.FormatoId, cancellationToken);
    }

    public Task<JsonElement> SincronizarRecoleccionesCajasAsync(SincronizarRecoleccionCajaRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true);
        var modo = (request.Modo ?? "").Trim().ToLowerInvariant();
        if (modo != "nuevo" && modo != "anular") throw new BadRequestException("Modo invalido. Use nuevo o anular.");
        if (request.IdLinea <= 0) throw new BadRequestException("El campo idLinea es obligatorio para registrar cajas.");
        if (request.Puesto <= 0) throw new BadRequestException("El campo puesto es obligatorio para registrar cajas.");
        if (modo == "nuevo")
        {
            if (request.FormatoId <= 0) throw new BadRequestException("El campo formatoId es obligatorio para registrar cajas.");
            if (request.TipoEmpaqueId <= 0) throw new BadRequestException("El campo tipoEmpaqueId es obligatorio para registrar cajas.");
            if (request.CantidadCajas < 1 || request.CantidadCajas > 99999) throw new BadRequestException("La cantidad de cajas debe ser un entero entre 1 y 99999.");
        }

        var json = JsonSerializer.Serialize(new[]
        {
            new
            {
                id = request.Id,
                modo,
                codigoAcopio = contexto.CodigoAcopio,
                idLinea = request.IdLinea,
                puesto = request.Puesto,
                formatoId = request.FormatoId,
                tipoEmpaqueId = request.TipoEmpaqueId,
                cantidadCajas = request.CantidadCajas,
                calibreNombre = (request.CalibreNombre ?? "").Trim(),
            },
        });
        return jornadas.SincronizarRecoleccionesCajasAsync(contexto, json, cancellationToken);
    }

    public Task<JsonElement> ListarRecoleccionesAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarRecoleccionesAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> ListarRecoleccionesCajasHistorialAsync(ListarRecoleccionesCajasHistorialRequest request, CancellationToken cancellationToken = default)
    {
        if (request.IdRecoleccion < 0) throw new BadRequestException("El campo idRecoleccion no es válido.");
        return jornadas.ListarRecoleccionesCajasHistorialAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), request.IdRecoleccion, cancellationToken);
    }

    public Task<JsonElement> ListarRecoleccionesCajasResumenAsync(CancellationToken cancellationToken = default)
        => jornadas.ListarRecoleccionesCajasResumenAsync(ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true), cancellationToken);

    public Task<JsonElement> SincronizarRecoleccionesAsync(SincronizarJornadaAnotadorRecoleccionRequest request, CancellationToken cancellationToken = default)
    {
        var contexto = ContextoDeUsuario.Crear(currentUser, exigirUsuarioYRol: true, exigirAcopio: true);
        var modo = (request.Modo ?? "").Trim().ToLowerInvariant();
        var nombreUsuario = currentUser.NombreCompleto?.Trim();

        if (modo == "nuevo")
        {
            if (string.IsNullOrEmpty(nombreUsuario)) throw new BadRequestException("No se encontro el nombre del usuario en el token.");
        }
        else if (modo == "editado")
        {
            if (request.Id <= 0) throw new BadRequestException("El campo id es obligatorio para cerrar la recoleccion.");
        }
        else
        {
            throw new BadRequestException("Modo invalido. Use nuevo o editado.");
        }

        var json = JsonSerializer.Serialize(new[]
        {
            new
            {
                id = request.Id,
                modo,
                codigoAcopio = contexto.CodigoAcopio,
                nombreUsuario = nombreUsuario ?? string.Empty,
            },
        });
        return jornadas.SincronizarRecoleccionesAsync(contexto, json, cancellationToken);
    }

    private static void Requerir(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new BadRequestException($"El campo {campo} es obligatorio.");
        }
    }
}
