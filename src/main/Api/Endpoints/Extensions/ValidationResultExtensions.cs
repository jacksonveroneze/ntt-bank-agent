using FluentValidation.Results;

namespace NttBank.QueryAgent.Api.Endpoints.Extensions;

internal static class ValidationResultExtensions
{
    internal static IDictionary<string, string[]> ToValidationProblemDictionary(
        this ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(
                error => error.PropertyName,
                comparer: StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .ToArray(),
                StringComparer.OrdinalIgnoreCase);
    }
    
    internal static IResult ToValidationProblem(
        this ValidationResult validationResult)
    {
        return Results.ValidationProblem(
            validationResult.ToValidationProblemDictionary());
    }
}
