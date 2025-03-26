using Microsoft.Data.SqlClient;
using SgartIT.ReactApp1.Server.Repositories.MsSql.Extensions;

namespace SgartIT.ReactApp1.Server.Repositories.MsSql.DBContexts
{
    public class DatabaseContext(string connectionString) : IDisposable
    {
        private readonly string connectionString = connectionString;

        private SqlConnection? cnn;
        private SqlCommand? cmd;
        private SqlDataReader? reader;

        public async Task<SqlDataReader> ExecuteReaderAsync(string query, Dictionary<string, object>? values = null)
        {
            cnn = await GetConnectionAsync();
            cmd = cnn.CreateCommandText(query, values);
            reader = await cmd.ExecuteReaderAsync();
            return reader;
        }

        public async Task<object?> ExecuteScalarAsync(string query, Dictionary<string, object>? values = null)
        {
            cnn = await GetConnectionAsync();
            cmd = cnn.CreateCommandText(query, values);
            return await cmd.ExecuteScalarAsync();
        }
        public async Task ExecuteNonQuery(string query, Dictionary<string, object>? values = null)
        {
            cnn = await GetConnectionAsync();
            cmd = cnn.CreateCommandText(query, values);
            await cmd.ExecuteNonQueryAsync();
        }


        private async Task<SqlConnection> GetConnectionAsync()
        {
            SqlConnection cnn = new(connectionString);
            await cnn.OpenAsync();
            return cnn;
        }

        public void Dispose()
        {
            cmd?.Dispose();
            cnn?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
