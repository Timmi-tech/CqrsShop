using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Middlewares
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/problem+json";

                    IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null)
                        return;

                    Exception  exception = contextFeature.Error;
                    int statusCode = MapToStatusCode(exception);

                    context.Response.StatusCode = statusCode;

                    ProblemDetails problemDetails = new()
                    {
                        Type = $"https://httpstatuses.com/{statusCode}",
                        Status = statusCode,
                        Title = GetTitleForStatusCode(statusCode),
                        Detail = exception.Message,
                        Instance = context.Request.Path
                    };

                    // Add trace ID for correlation in logs
                    problemDetails.Extensions["traceId"] = context.TraceIdentifier;

                    // Conditional logging
                    if (statusCode == StatusCodes.Status500InternalServerError)
                        logger.LogError($"Unhandled Exception: {exception}");
                    else
                        logger.LogError($"{exception.GetType().Name}: {exception.Message}");

                    await context.Response.WriteAsJsonAsync(problemDetails);
                });
            });
        }

        private static int MapToStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static string GetTitleForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                _ => "Unexpected Error"
            };
        }
    }
}