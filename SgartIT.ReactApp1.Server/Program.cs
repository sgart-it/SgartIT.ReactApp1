using DocumentFormat.OpenXml.InkML;
using NLog;
using NLog.Web;
using SgartIT.ReactApp1.Server;
using SgartIT.ReactApp1.Server.DTO.Settings;
using SgartIT.ReactApp1.Server.Handlers;

Logger? logger = null;

try
{
    logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
    logger.Info($"{C.LOG_START}: v.{C.VERSION}, {Environment.CommandLine}");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    logger.Info($"Env: {builder.Environment.EnvironmentName}");

    // NLog: Setup NLog for Dependency injection
    // builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    AppSettings appSettings = builder.AddAppSettings(logger);

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // project services
    builder.AddAppRepository(logger);
    builder.AddAppServices(logger);

    WebApplication app = builder.Build();

    // sequenza di caricamento dei middleware https://developmentwithadot.blogspot.com/2025/01/aspnet-core-middleware.html
    app.UseExceptionHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        logger.Info("ENV IsDevelopment=true");
        //app.UseExceptionHandler("/Error");
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    //app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);
    //app.UseWhen(context => context.Request.Query.ContainsKey("branch"), appBuilder => HandleBranchAndRejoin(appBuilder));

    app.UseDefaultFiles(); // serve per i progetti SPA
    app.UseStaticFiles(); // serve per i progetti SPA

    // app.UseCookiePolicy();
    // app.UseRouting();
    // app.UseRateLimiter();
    // app.UseRequestLocalization();

    app.UseAppCors(logger, appSettings.Cors);

    app.UseAppMiddleware(logger);

    //app.UseAuthentication();
    app.UseAuthorization();

    // app.UseSession();
    // app.UseResponseCompression();
    // app.UseResponseCaching();

    app.MapControllers();

    app.MapFallbackToFile("/index.html");

    app.Run();
}
catch (Exception ex)
{
    // NLog: catch setup errors
    logger?.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    logger?.Info(C.LOG_STOP);
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
