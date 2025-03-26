namespace SgartIT.ReactApp1.Server.Middlewares;

/// <summary>
/// Middlewar di esempio che logga l'inizio e la fine di una richiesta
/// </summary>
/// <param name="logger"></param>
public class LogReqestMiddleware(ILogger<LogReqestMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            if (System.Diagnostics.Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                // sovrascrivo solo se non era già impostato
                System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            logger.LogTrace(C.LOG_REQUEST_BEGIN + " {method} {path}{query}"
                , context.Request.Method, context.Request.Path, context.Request.QueryString);

            await next(context);     // dovo ricordarmi di passare il controllo al prossimo middleware

            if (context.Response.StatusCode == 401)
            {
                logger.LogWarning("401 Unauthorized");
            }
        }
        catch (Exception ex)
        {
            var r = context.Request;
            logger.LogError(ex, "IsAuthenticated:{isAuthenticated}|{method} {protocol} {scheme}://{host}{path}{query}",
                context.User?.Identity?.IsAuthenticated,
                r.Method, r.Protocol, r.Scheme, r.Host, r.Path, r.QueryString);
            throw;
        }
        finally
        {
            logger.LogTrace(C.LOG_REQUEST_END);
        }
    }
}