using System.Data;
using Microsoft.EntityFrameworkCore;
using Planta.Infrastructure.Persistence;

namespace Planta.Infrastructure.RepositoryImpl;

public abstract class BaseRepository
{
    protected readonly SistemaPaletsDbContext Context;

    protected BaseRepository(SistemaPaletsDbContext context)
    {
        Context = context;
    }

    protected async Task<List<T>> EjecutarStoredProcedureAsync<T>(
        string spName,
        Dictionary<string, object?> parametros,
        Func<IDataReader, T> mapeo)
    {
        var lista = new List<T>();
        var command = Context.Database.GetDbConnection().CreateCommand();
        command.CommandText = spName;
        command.CommandType = CommandType.StoredProcedure;
        command.CommandTimeout = 120;

        foreach (var kvp in parametros)
        {
            var param = command.CreateParameter();
            param.ParameterName = kvp.Key;
            param.Value = kvp.Value ?? DBNull.Value;
            command.Parameters.Add(param);
        }

        try
        {
            await Context.Database.OpenConnectionAsync();
            using (var result = await command.ExecuteReaderAsync())
            {
                while (await result.ReadAsync())
                {
                    lista.Add(mapeo(result));
                }
            }
        }
        finally
        {
            await Context.Database.CloseConnectionAsync();
        }

        return lista;
    }
}
