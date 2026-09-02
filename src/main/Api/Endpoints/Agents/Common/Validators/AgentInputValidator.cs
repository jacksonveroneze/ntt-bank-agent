using FluentValidation;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Agent;

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
