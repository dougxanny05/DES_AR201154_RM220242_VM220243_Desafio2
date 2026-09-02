using Eventos.Common;
using Eventos.DAL.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace Eventos.DAL
{
    public class DatabaseRepository(IOptions<AppSettings> appSettings, ILogger<DatabaseRepository> logger) : IDatabaseRepository
    {
        private readonly string connectionString = appSettings.Value.ConnectionString;
        private readonly ILogger<DatabaseRepository> _logger = logger;

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection.BeginTransaction();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    return await transaction.Connection!.QueryAsync<T>(sql, parameters, transaction);
                }

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return await connection.QueryAsync<T>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en QueryAsync. Sql: {Sql}", sql);
                throw;
            }
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    return await transaction.Connection!.QueryFirstOrDefaultAsync<T>(sql, parameters, transaction);
                }

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en QueryFirstOrDefaultAsync. Sql: {Sql}", sql);
                throw;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    return await transaction.Connection!.ExecuteAsync(sql, parameters, transaction);
                }

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return await connection.ExecuteAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ExecuteAsync. Sql: {Sql}", sql);
                throw;
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    return await transaction.Connection!.ExecuteScalarAsync<T>(sql, parameters, transaction);
                }

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return await connection.ExecuteScalarAsync<T>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ExecuteScalarAsync. Sql: {Sql}", sql);
                throw;
            }
        }
    }
}
