using Microsoft.Data.SqlClient;

namespace SgartIT.ReactApp1.Server.Repositories.MsSql.Extensions;

public static class DatabaseExtensions
{
    public static SqlCommand CreateCommandText(this SqlConnection cnn, string query, Dictionary<string,object>? values=null)
    {
        SqlCommand cmd = cnn.CreateCommand();
        cmd.CommandText = query;
        if(values != null)
        {
            foreach (var kvp in values)
            {
                cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
            }
        }
        return cmd;
    }
}
