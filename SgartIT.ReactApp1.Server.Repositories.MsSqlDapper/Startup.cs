using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SgartIT.ReactApp1.Server.DTO.Repositories;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlDapper;

public static class Startup
{
    const string APP_SETTINGS_KEY = "RepositoryDynamic:MsSqlConnectionString";

    public static void Init(IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetValue<string>(APP_SETTINGS_KEY) ?? throw new Exception($"Invalid {APP_SETTINGS_KEY}");
        
        builder.Services.AddKeyedSingleton<string>("MsSqlConnectionString", connectionString);
        builder.Services.AddScoped<ITodoRepository, MsSqlDapperTodoRepository>();
    }
}
