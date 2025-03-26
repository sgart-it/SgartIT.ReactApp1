﻿//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Mvc;

//namespace SgartIT.ReactApp1.Server.Handlers;

//internal sealed class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
//{

//    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
//    {
//        if (exception is not BadRequestException badRequestException)
//        {
//            return false;
//        }

//        logger.LogError(badRequestException, "Exception Bad Request: {Message}", badRequestException.Message);

//        var problemDetails = new ProblemDetails
//        {
//            Status = StatusCodes.Status400BadRequest,
//            Title = "Bad Request",
//            Detail = badRequestException.Message
//        };

//        httpContext.Response.StatusCode = problemDetails.Status.Value;

//        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

//        return true;
//    }
//}
