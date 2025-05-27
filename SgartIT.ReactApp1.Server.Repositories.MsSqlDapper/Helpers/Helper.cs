using Microsoft.Data.SqlClient;
using System.Data;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper.Helpers
{
    internal static class Helper
    {
        public static IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
