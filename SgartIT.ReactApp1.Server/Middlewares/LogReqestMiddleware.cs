using Microsoft.Extensions.Logging;

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
            logger.LogTrace(C.LOG_REQUEST_BEGIN);

            logger.LogDebug("CALL: {method} {scheme}://{host}{path}?{query} {protocol}, controller: {ctrl}, user: {user}"
                , context.Request.Method
                , context.Request.Scheme
                , context.Request.Host
                , context.Request.Path
                , context.Request.QueryString
                , context.Request.Protocol
                , context.GetEndpoint()?.DisplayName
                , context.User.Identity?.Name);

            await next(context);     // dovo ricordarmi di passare il controllo al prossimo middleware

            if (context.Response.StatusCode == 401)
            {
                logger.LogWarning("HTTP 401 Unauthorized");
            }
            else if (context.Response.StatusCode == 403)
            {
                logger.LogWarning("HTTP 403 Forbidden");
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