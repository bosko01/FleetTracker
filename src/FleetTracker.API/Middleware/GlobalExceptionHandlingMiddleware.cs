using System.Net;
using System.Text.Json;
using FleetTracker.Application.Common.Exceptions;
using FleetTracker.Application.Common.Models;
using FleetTracker.Domain.Exceptions;
using FluentValidation;

namespace FleetTracker.API.Middleware;

public sealed class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
    {
        logger.LogError(
            exception,
            "Unhandled exception on {RequestPath}. Message: {ExceptionMessage}",
            context.Request.Path,
            exception.Message);

        var (statusCode, response) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, new ErrorResponse(exception.Message)),
            ConflictException => (HttpStatusCode.Conflict, new ErrorResponse(exception.Message)),
            DomainRuleViolationException => (HttpStatusCode.BadRequest, new ErrorResponse(exception.Message)),
            ValidationException validationException =>
                (HttpStatusCode.BadRequest, new ErrorResponse("Validation failed.", validationException.Errors.Select(e => e.ErrorMessage).ToList())),
            _ => (HttpStatusCode.InternalServerError, new ErrorResponse("An unexpected error occurred."))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
