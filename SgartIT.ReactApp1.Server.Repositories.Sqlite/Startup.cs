using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.Sqlite.DbContexts;

namespace SgartIT.ReactApp1.Server.Repositories.Sqlite;

public static class Startup
{
    const string APP_SETTINGS_KEY = "RepositoryDynamic:SqliteConnectionString";

    public static void Init(IHostApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetValue<string>(APP_SETTINGS_KEY) ?? throw new Exception($"Invalid {APP_SETTINGS_KEY}");

        var cnnBuilder = new SqliteConnectionStringBuilder(connectionString);

        // se il path è relativo ".\nomedb.db" lo trasformo in assoluto
        if (cnnBuilder.DataSource.StartsWith('.'))
        {
            cnnBuilder.DataSource = Path.Combine(AppContext.BaseDirectory, cnnBuilder.DataSource);
        }

        string path = Path.GetDirectoryName(cnnBuilder.DataSource) ?? throw new Exception($"Invalid path '{cnnBuilder.DataSource}'");

        if (Path.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite(cnnBuilder.ConnectionString);
        });

        //builder.Services.AddKeyedSingleton<string>("SqliteConnectionString", connectionString);
        builder.Services.AddScoped<ITodoRepository, SqliteTodoRepository>();
    }

}
