using FluentValidation;
using NttBank.Agent.Agent.Abstractions;

namespace NttBank.Agent.Api.Endpoints.Agents.Common.Validators;

public sealed class AgentInputValidator
    : AbstractValidator<AgentInput>
{
    public AgentInputValidator()
    {
        RuleFor(request => request.Prompt)
            .SetValidator(new PromptValidator()!);
    }
}
