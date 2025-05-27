using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper.DBContexts
{
    public class DatabaseContext(ILogger<DatabaseContext> logger, [FromKeyedServices("MsSqlConnectionString")] string connectionString)
    {
        public IDbConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            logger.LogDebug("Executing query: {query} with parameters: {@parameters}", query, parameters);

            using IDbConnection cnn = GetConnection();
            return await cnn.QueryAsync<T>(query, parameters, transaction);
        }
        public async Task<IEnumerable<dynamic>> QueryAsync(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            logger.LogDebug("Executing query dynamic: {query} with parameters: {@parameters}", query, parameters);

            using IDbConnection cnn = GetConnection();
            return await cnn.QueryAsync<dynamic>(query, parameters, transaction);
        }

        public async Task<T> QueryFirstAsync<T>(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            logger.LogDebug("Executing query first: {query} with parameters: {@parameters}", query, parameters);

            using IDbConnection cnn = GetConnection();
            return await cnn.QueryFirstAsync<T>(query, parameters, transaction);
        }

        public async Task<IEnumerable<T>> StoreAsync<T>(string query, object? parameters = null, IDbTransaction? transaction = null)
        {
            logger.LogDebug("Executing query: {query} with parameters: {@parameters}", query, parameters);

            using IDbConnection cnn = GetConnection();
            return await cnn.QueryAsync<T>(query, parameters, transaction, null, CommandType.StoredProcedure);
        }
    }
}
