using Microsoft.Data.SqlClient;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper.Helpers
{
    internal static class Helper
    {
        public static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);

        }
    }
}
