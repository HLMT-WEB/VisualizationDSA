using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VisualizationDSA.Domain.Exceptions;

namespace VisualizationDSA.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, problemDetails) = exception switch
        {
            NotFoundException ex => (StatusCodes.Status404NotFound, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7807",
                Extensions = { ["errorType"] = ex.ErrorType }
            }),

            DomainValidationException ex => (StatusCodes.Status400BadRequest, new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Error",
                Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7807",
                Extensions =
                {
                    ["errorType"] = ex.ErrorType,
                    ["errors"] = ex.Errors
                }
            }),

            AuthenticationException ex => (StatusCodes.Status401Unauthorized, new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Authentication Failed",
                Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7807",
                Extensions = { ["errorType"] = ex.ErrorType }
            }),

            ConflictException ex => (StatusCodes.Status409Conflict, new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc7807",
                Extensions = { ["errorType"] = ex.ErrorType }
            }),

            _ => (StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Detail = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.",
                Type = "https://tools.ietf.org/html/rfc7807",
                Extensions = { ["errorType"] = "INTERNAL_ERROR" }
            })
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("Domain exception {ErrorType}: {Message}", (exception as DomainException)?.ErrorType, exception.Message);
        }

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options));
    }
}
