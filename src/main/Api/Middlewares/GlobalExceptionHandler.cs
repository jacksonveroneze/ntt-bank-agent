using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace NttBank.QueryAgent.Api.Middlewares;

internal sealed class CustomExceptionHandler(
    IHostEnvironment hostEnvironment,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        const int defaultStatus = StatusCodes.Status500InternalServerError;

        ProblemDetailsContext problemDetailsCtx = new()
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = "An error occurred",
                Detail = hostEnvironment.IsDevelopment()
                    ? exception.Message
                    : null,
                Type = exception.GetType().Name,
                Status = exception switch
                {
                    ValidationException => StatusCodes.Status400BadRequest,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    InvalidOperationException => StatusCodes.Status400BadRequest,
                    _ => defaultStatus,
                },
            },
        };

        if (exception is ValidationException validationException)
        {
            problemDetailsCtx.ProblemDetails.Extensions["errors"] =
                validationException.Errors
                    .GroupBy(error => error.PropertyName,
                        comparer: StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.ErrorMessage),
                        StringComparer.OrdinalIgnoreCase);
        }

        return problemDetailsService
            .TryWriteAsync(problemDetailsCtx);
    }
}
