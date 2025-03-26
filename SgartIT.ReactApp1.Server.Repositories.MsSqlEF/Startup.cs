using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.MsSqlEf.DbContexts;
using SgartIT.ReactApp1.Server.Repositories.MsSqlEf.Interceptors;

namespace SgartIT.ReactApp1.Server.Repositories.MsSqlEf;

public static class Startup
{
    const string APP_SETTINGS_KEY = "RepositoryDynamic:MsSqlConnectionString";

    public static void Init(IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetValue<string>(APP_SETTINGS_KEY) ?? throw new Exception($"Invalid {APP_SETTINGS_KEY}");

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(new DatabaseUpdateInterceptor());
        });
        builder.Services.AddScoped<ITodoRepository, MsSqlEfTodoRepository>();
    }
}
