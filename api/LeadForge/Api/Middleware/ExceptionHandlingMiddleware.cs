using System.Net;
using System.Text.Json;
using LeadForge.Domain.Exceptions;

namespace LeadForge.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception for request {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            InsufficientCreditsException => HttpStatusCode.BadRequest,
            InvalidCredentialsException => HttpStatusCode.Unauthorized,
            AlreadyExistsException => HttpStatusCode.Conflict,
            InvalidRefreshTokenException => HttpStatusCode.Unauthorized,

            _ => HttpStatusCode.InternalServerError
        };

        var response = new
        {
            message = exception.Message,
            statusCode = (int)statusCode
        };

        var json = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(json);
    }
}