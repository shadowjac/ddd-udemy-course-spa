using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    //generate a middleware
    private readonly RequestDelegate _next;

    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            var details = HandleException(ex);

            var problemDetails = new ProblemDetails
            {
                Status = details.Status,
                Type = details.Type,
                Title = details.Title,
                Detail = details.Details
            };

            if (details.Errors is not null)
            {
                problemDetails.Extensions.Add("errors", details.Errors);
            }

            context.Response.StatusCode = details.Status;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static ExceptionDetails HandleException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation error",
                "Validation error in models",
                validationException.Errors
            ),

            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "Server error",
                "Server error",
                "Unexpected error",
                null)
        };
    }

    internal sealed record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Details,
        IEnumerable<object>? Errors);
}