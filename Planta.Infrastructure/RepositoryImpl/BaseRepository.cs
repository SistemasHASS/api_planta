using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

/// <summary>
/// Base de todos los repositorios. Ejecuta stored procedures con parametros nombrados y tipados.
/// Cada repositorio solo describe QUE procedimiento llama y con que parametros.
/// </summary>
public abstract class BaseRepository
{
    protected readonly ApplicationDbContext Context;

    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    /// <summary>Ejecuta un SP que devuelve filas y las convierte con <paramref name="mapeo"/>.</summary>
    protected async Task<List<T>> EjecutarListaAsync<T>(string procedimiento,Action<IDbCommand> parametros,Func<IDataRecord, T> mapeo,CancellationToken cancellationToken = default)
    {
        var lista = new List<T>();

        await using var command = Context.Database.GetDbConnection().CreateCommand();
        Preparar(command, procedimiento, parametros);

        await Context.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                lista.Add(mapeo(reader));
            }
        }
        finally
        {
            await Context.Database.CloseConnectionAsync();
        }

        return lista;
    }

    /// <summary>Ejecuta un SP que no devuelve datos (insert, update, delete).</summary>
    protected async Task EjecutarAsync(string procedimiento,Action<IDbCommand> parametros,CancellationToken cancellationToken = default)
    {
        await using var command = Context.Database.GetDbConnection().CreateCommand();
        Preparar(command, procedimiento, parametros);

        await Context.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
        finally
        {
            await Context.Database.CloseConnectionAsync();
        }
    }

    /// <summary>Ejecuta un SP y devuelve el primer valor de la primera fila (o null).</summary>
    protected async Task<object?> EjecutarEscalarAsync(string procedimiento,Action<IDbCommand> parametros,CancellationToken cancellationToken = default)
    {
        await using var command = Context.Database.GetDbConnection().CreateCommand();
        Preparar(command, procedimiento, parametros);

        await Context.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            return await command.ExecuteScalarAsync(cancellationToken);
        }
        finally
        {
            await Context.Database.CloseConnectionAsync();
        }
    }

    /// <summary>
    /// Ejecuta un SP que sigue el estandar de respuesta (docs/ESTANDAR-SP.md): una fila con { error, data, mensaje }
    /// donde data es un texto JSON. Devuelve ese mismo sobre como JSON, con data ya como arreglo (sin DTO ni validaciones).
    /// Las columnas se leen por posicion (0 = error, 1 = data, 2 = mensaje), como lo hacia la API anterior;
    /// con <c>mensajeAntesDeData</c> el orden es (error, mensaje, data).
    /// </summary>
    protected async Task<JsonElement> EjecutarSobreAsync(string procedimiento, Action<IDbCommand> parametros, CancellationToken cancellationToken = default, bool mensajeAntesDeData = false)
    {
        // Casi todos los SP devuelven (error, data, mensaje); unos pocos devuelven (error, mensaje, data).
        var posicionData = mensajeAntesDeData ? 2 : 1;
        var posicionMensaje = mensajeAntesDeData ? 1 : 2;
        var filas = await EjecutarListaAsync(procedimiento, parametros, record => (
            Error: !record.IsDBNull(0) && Convert.ToBoolean(record.GetValue(0)),
            Data: record.IsDBNull(posicionData) ? null : Convert.ToString(record.GetValue(posicionData)),
            Mensaje: record.IsDBNull(posicionMensaje) ? null : Convert.ToString(record.GetValue(posicionMensaje))), cancellationToken);

        var fila = filas.FirstOrDefault();
        using var data = JsonDocument.Parse(string.IsNullOrWhiteSpace(fila.Data) ? "[]" : fila.Data);

        return JsonSerializer.SerializeToElement(new { error = fila.Error, data = data.RootElement, mensaje = fila.Mensaje });
    }

    /// <summary>Agrega un parametro nombrado. Un valor null se envia como NULL de SQL.</summary>
    protected static void Agregar(IDbCommand command, string nombre, object? valor, DbType? tipo = null)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = nombre;
        parameter.Value = valor ?? DBNull.Value;
        if (tipo.HasValue)
        {
            parameter.DbType = tipo.Value;
        }

        command.Parameters.Add(parameter);
    }

    /// <summary>Lee una columna de texto que puede venir NULL.</summary>
    protected static string? LeerTexto(IDataRecord record, string columna)
    {
        var ordinal = record.GetOrdinal(columna);
        return record.IsDBNull(ordinal) ? null : record.GetString(ordinal);
    }

    private static void Preparar(IDbCommand command, string procedimiento, Action<IDbCommand> parametros)
    {
        command.CommandText = procedimiento;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 120;
        parametros(command);
    }
}
