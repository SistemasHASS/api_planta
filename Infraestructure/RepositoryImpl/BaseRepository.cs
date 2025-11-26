using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using api_planta.Infraestructure.Data;

namespace api_planta.Infraestructure.RepositoryImpl
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected async Task<List<T>> EjecutarStoredProcedureAsync<T>(
            string spName,
            string? json,
            Func<DbDataReader, T> mapeo,
            bool parametrosRequeridos = false)
        {
            var lista = new List<T>();
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;

            if (parametrosRequeridos && !string.IsNullOrEmpty(json))
            {
                var param = command.CreateParameter();
                param.ParameterName = "@json";
                param.DbType = DbType.String;
                param.Value = json;
                command.Parameters.Add(param);
            }

            try
            {
                await _context.Database.OpenConnectionAsync();
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
                await _context.Database.CloseConnectionAsync();
            }

            return lista;
        }

        protected async Task<int> EjecutarQueryAsync(string sql, Action<DbCommand> parametros)
        {
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            parametros(command);

            try
            {
                await _context.Database.OpenConnectionAsync();
                int affectedRows = await command.ExecuteNonQueryAsync();
                return affectedRows;
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

    }
}
