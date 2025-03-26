using NLog;
using SgartIT.ReactApp1.Server.DTO.Settings;
using SgartIT.ReactApp1.Server.Exports;
using SgartIT.ReactApp1.Server.Middlewares;
using SgartIT.ReactApp1.Server.Services;
using System.Reflection;

namespace SgartIT.ReactApp1.Server;

public static class ProgramExtensions
{
    /// <summary>
    /// aggiungo appsettings.json
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="logger"></param>
    /// <returns>ritorna un oggetto AppSettings da usare in altre impostazioni</returns>
    public static AppSettings AddAppSettings(this IHostApplicationBuilder builder, Logger logger)
    {
        logger.Trace(C.LOG_BEGIN);

        var section = builder.Configuration.GetSection(AppSettings.KEY_NAME);

        builder.Services.AddOptions<AppSettings>()
            .Bind(section)
            .ValidateDataAnnotations() // attenzione funziona solo sul primo livello non con le calssi innestatehttps://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-6.0#validate-options-configuration-data
            .ValidateOnStart();

        // settings dell'applicazione
        AppSettings appSettings = section.Get<AppSettings>() ?? throw new Exception("Invalid AppSettings");

        //Create singleton from instance
        //builder.Services.AddSingleton(appSettings);

        return appSettings;
    }

    public static void AddAppServices(this IHostApplicationBuilder builder, Logger logger)
    {
        logger.Trace(C.LOG_BEGIN);

        builder.Services.AddScoped<MainService>();
        builder.Services.AddTransient<LogReqestMiddleware>();
    }

    /// <summary>
    /// carica una dll "Repository" dinamicamente tramite il nome dell'assembly
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="logger"></param>
    /// <exception cref="Exception"></exception>
    public static void AddAppRepository(this IHostApplicationBuilder builder, Logger logger)
    {
        const string APP_SETTINGS_KEY = "RepositoryDynamic:AssemblyName";
        const string CLASS_NAME = "Startup";
        const string METHOD_NAME = "Init";

        logger.Trace(C.LOG_BEGIN);

        string repositoryName = builder.Configuration.GetValue<string>(APP_SETTINGS_KEY) ?? throw new Exception($"Appsettings name {APP_SETTINGS_KEY} not found");

        try
        {
            logger.Info($"Repository: {repositoryName}");

            string fullRepositoryName = repositoryName.StartsWith('.')
                ? Path.Combine(AppContext.BaseDirectory, repositoryName)
                : repositoryName;

            logger.Info($"Loading repository: {fullRepositoryName}");

            Assembly? assembly = Assembly.LoadFile(fullRepositoryName) ?? throw new Exception($"Assembly '{repositoryName}' not found.");

            dynamic? myClass = assembly.ExportedTypes.FirstOrDefault(x => x.Name == CLASS_NAME) ?? throw new Exception($"Class '{CLASS_NAME}' not found in the assembly '{repositoryName}'.");

            logger.Info($"RepositoryClass: {myClass.FullName}");

            MethodInfo staticMethodInfo = myClass.GetMethod(METHOD_NAME) ?? throw new Exception($"Method {CLASS_NAME}.{METHOD_NAME} not found in the assembly {repositoryName}.");

            logger.Info($"Invoke: {METHOD_NAME}");

            staticMethodInfo.Invoke(null, [builder]);

            logger.Trace(C.LOG_END);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Repository: {repo}", repositoryName);
            throw;
        }
        finally
        {
            logger.Trace(C.LOG_END);
        }

        //string assemblyName = assembly.GetName().Name;

        //string? className = repositoryName + "." + CLASS_NAME;
        //dynamic? myClass = assembly.GetType(className) ?? throw new Exception($"Repository {className} not found in the assembly {repositoryName}.");
        //logger.Info($"RepositoryClass: {CLASS_NAME}");

        //MethodInfo staticMethodInfo = myClass.GetMethod(METHOD_NAME) ?? throw new Exception($"Method {className}.{METHOD_NAME} not found in the assembly {repositoryName}.");
        //logger.Info($"RepositoryMethod: {METHOD_NAME}");

        //staticMethodInfo.Invoke(null, [builder]);

        //logger.Trace(C.LOG_END);
    }


    public static void UseAppCors(this IApplicationBuilder app, Logger logger, string[] urls)
    {
        //string[]? urls = settingsSection.Get<AppSettings>()?.Cors;
        if (urls?.Length > 0)
        {
            logger.Info("APPLY CORS");

            app.UseCors(policy =>
            {
                //string[]? urls = builder.Configuration.GetSection("Cors").Get<string[]>() ?? throw new Exception("appsettings Cors is null");
                //string[] urls = settingsSection.Get<AppSettings>()?.Cors ?? throw new Exception("appsettings Cors is null");

                //policy.WithOrigins("https://sgart.sharepoint.com/", "https://sgart.sharepoint.com")
                policy.WithOrigins(urls)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        }
    }


    /// <summary>
    /// https://developmentwithadot.blogspot.com/2025/01/aspnet-core-middleware.html
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    public static void UseAppMiddleware(this IApplicationBuilder app, Logger logger)
    {
        logger.Trace(C.LOG_BEGIN);

        app.UseMiddleware<LogReqestMiddleware>();
    }


    /*
    public static void AddRepositoryService(this IHostApplicationBuilder builder, RepositorySettings repoSettings)
    {
        Logger logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        logger.Trace(C.LOG_BEGIN);

        // project services
        RepositoryTypeEnum? repositoryType = repoSettings.Type;
        logger.Info($"RepositoryType: {repositoryType}");

        switch (repositoryType)
        {
            case RepositoryTypeEnum.InMemory:
                Repositories.InMemory.Startup.Init(builder);
                break;

            case RepositoryTypeEnum.MsSql:
                Repositories.MsSql.Startup.Init(builder);
                break;

            case RepositoryTypeEnum.MsSqlEf:
                Repositories.MsSqlEf.Startup.Init(builder);
                break;

            case RepositoryTypeEnum.SPOnline:
                Repositories.SPOnline.Startup.Init(builder);
                break;

            default:
                throw new Exception($"Repository '{repositoryType}' not implemented");
        }

    }
    */
}
