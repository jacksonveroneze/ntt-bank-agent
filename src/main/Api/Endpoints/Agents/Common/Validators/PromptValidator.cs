using FluentValidation;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Validators;

public sealed class PromptValidator
    : AbstractValidator<string>
{
    private const int PromptMinLength = 25;
    private const int PromptMaxLength = 10_000;

    public PromptValidator()
    {
        RuleFor(request => request)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Length(PromptMinLength, PromptMaxLength);
    }
}
