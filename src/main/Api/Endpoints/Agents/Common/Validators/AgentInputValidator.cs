using FluentValidation;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Validators;

public sealed class AgentInputValidator
    : AbstractValidator<AgentInput>
{
    public AgentInputValidator()
    {
        RuleFor(request => request.Prompt)
            .SetValidator(new PromptValidator()!);
    }
}
