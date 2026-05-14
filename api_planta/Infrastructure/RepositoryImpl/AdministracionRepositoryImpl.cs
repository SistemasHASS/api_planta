using api_planta.Domain.Repository;
using api_planta.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_planta.Infrastructure.RepositoryImpl
{
    public class AdministracionRepositoryImpl : BaseRepository, IAdministracionRepository
    {
        public AdministracionRepositoryImpl(SistemaPaletsDbContext context) : base(context) { }

        public async Task<List<JsonElement>> ListarMatricesCompatibilidadAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("Listar_MatrizCompatibilidad", json, result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                JsonElement data;
                if (result.IsDBNull(1))
                {
                    data = JsonSerializer.Deserialize<JsonElement>("null");
                }
                else
                {
                    var dataStr = Convert.ToString(result.GetValue(1));
                    data = string.IsNullOrWhiteSpace(dataStr)
                        ? JsonSerializer.Deserialize<JsonElement>("null")
                        : JsonSerializer.Deserialize<JsonElement>(dataStr);
                }

                var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));

                var payload = new { error, data, mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });
        }

        public async Task<List<JsonElement>> SincronizarMatrizCompatibilidadAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_SincronizarMatrizCompatibilidad", json, result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                JsonElement data;
                if (result.IsDBNull(1))
                {
                    data = JsonSerializer.Deserialize<JsonElement>("null");
                }
                else
                {
                    var dataStr = Convert.ToString(result.GetValue(1));
                    data = string.IsNullOrWhiteSpace(dataStr)
                        ? JsonSerializer.Deserialize<JsonElement>("null")
                        : JsonSerializer.Deserialize<JsonElement>(dataStr);
                }

                var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));

                var payload = new { error, data, mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });
        }

        public async Task<List<JsonElement>> ListarUsuariosAsync(string json)
        {
            return await EjecutarStoredProcedureAsync("SP_ListarUsuarios", 
            json
            , result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                JsonElement data;
                if (result.IsDBNull(1))
                {
                    data = JsonSerializer.Deserialize<JsonElement>("null");
                }
                else
                {
                    var dataStr = Convert.ToString(result.GetValue(1));
                    data = string.IsNullOrWhiteSpace(dataStr)
                        ? JsonSerializer.Deserialize<JsonElement>("null")
                        : JsonSerializer.Deserialize<JsonElement>(dataStr);
                }

                var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));

                var payload = new { error, data, mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });
        }

        public async Task<List<JsonElement>> SincronizarUsuariosAsync(string userId,string json)
        {
            return await EjecutarStoredProcedureAsync("SP_SincronizarUsuarios", 
            new Dictionary<string, object?>
            {
                { "@usuarioCre", int.Parse(userId) },
                { "@json", json }
            }
            , result =>
            {
                var error = !result.IsDBNull(0) && Convert.ToBoolean(result.GetValue(0));

                JsonElement data;
                if (result.IsDBNull(1))
                {
                    data = JsonSerializer.Deserialize<JsonElement>("null");
                }
                else
                {
                    var dataStr = Convert.ToString(result.GetValue(1));
                    data = string.IsNullOrWhiteSpace(dataStr)
                        ? JsonSerializer.Deserialize<JsonElement>("null")
                        : JsonSerializer.Deserialize<JsonElement>(dataStr);
                }

                var mensaje = result.IsDBNull(2) ? null : Convert.ToString(result.GetValue(2));

                var payload = new { error, data, mensaje };
                var payloadJson = JsonSerializer.Serialize(payload);
                return JsonSerializer.Deserialize<JsonElement>(payloadJson);
            });
        }

        public async Task<List<JsonElement>> ResetearPasswordUsuarioAsync(int id, string usuario, string passwordHasheado)
        {
            var connection = _context.Database.GetDbConnection();

            try
            {
                await _context.Database.OpenConnectionAsync();

                // 1) Validar existencia
                using (var selectCmd = connection.CreateCommand())
                {
                    selectCmd.CommandText = "select count(1) from Usuarios where Id = @id and Usuario = @usuario";
                    selectCmd.CommandType = CommandType.Text;

                    var pId = selectCmd.CreateParameter();
                    pId.ParameterName = "@id";
                    pId.Value = id;
                    selectCmd.Parameters.Add(pId);

                    var pUsuario = selectCmd.CreateParameter();
                    pUsuario.ParameterName = "@usuario";
                    pUsuario.Value = usuario;
                    selectCmd.Parameters.Add(pUsuario);

                    var existe = Convert.ToInt32(await selectCmd.ExecuteScalarAsync()) > 0;
                    if (!existe)
                    {
                        return EmpaquetarRespuesta(true, null, "Usuario no encontrado.");
                    }
                }

                // 2) Update password
                using (var updateCmd = connection.CreateCommand())
                {
                    updateCmd.CommandText = "update Usuarios set Password = @password where Id = @id and Usuario = @usuario";
                    updateCmd.CommandType = CommandType.Text;

                    var pPassword = updateCmd.CreateParameter();
                    pPassword.ParameterName = "@password";
                    pPassword.Value = passwordHasheado;
                    updateCmd.Parameters.Add(pPassword);

                    var pId = updateCmd.CreateParameter();
                    pId.ParameterName = "@id";
                    pId.Value = id;
                    updateCmd.Parameters.Add(pId);

                    var pUsuario = updateCmd.CreateParameter();
                    pUsuario.ParameterName = "@usuario";
                    pUsuario.Value = usuario;
                    updateCmd.Parameters.Add(pUsuario);

                    var rows = await updateCmd.ExecuteNonQueryAsync();
                    if (rows <= 0)
                        return EmpaquetarRespuesta(true, null, "No se pudo actualizar la contraseña.");
                }

                var data = new { id, usuario };
                return EmpaquetarRespuesta(false, data, "Contraseña actualizada.");
            }
            catch (Exception ex)
            {
                return EmpaquetarRespuesta(true, null, ex.Message);
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        private static List<JsonElement> EmpaquetarRespuesta(bool error, object? data, string? mensaje)
        {
            var payload = new { error, data, mensaje };
            var payloadJson = JsonSerializer.Serialize(payload);
            return new List<JsonElement> { JsonSerializer.Deserialize<JsonElement>(payloadJson) };
        }
    }
}
