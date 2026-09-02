using System.Data;

namespace Eventos.DAL.Interfaces
{
    public interface IDatabaseRepository
    {
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null);
        public Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null);
        public Task<int> ExecuteAsync(string sql, object? parameters = null, IDbTransaction? transaction = null);
        public Task<T?> ExecuteScalarAsync<T>(string sql, object? parameters = null, IDbTransaction? transaction = null);
        public Task<IDbTransaction> BeginTransactionAsync();
    }
}
