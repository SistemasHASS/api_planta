using api_planta.Infraestructure.Persistence;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_planta.Infraestructure.RepositoryImpl
{
    public abstract class BaseRepository
    {
        protected readonly SistemaPaletsDbContext _context;

        public BaseRepository(SistemaPaletsDbContext context)
        {
            _context = context;
        }

        protected async Task<List<T>> EjecutarStoredProcedureAsync<T>(
            string spName,
            string? json,
            Func<IDataReader, T> mapeo)
        {
            var lista = new List<T>();
            var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 120; // 2 minutos

            if (!string.IsNullOrEmpty(json))
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
    }
}
