using FluentValidation;
using NttBank.QueryAgent.Api.Endpoints.Agents.Common.Validators;
using NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1.Models;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1.Validators;

public sealed class QueryAgentRequestValidator
    : AbstractValidator<QueryAgentRequest>
{
    public QueryAgentRequestValidator()
    {
        RuleFor(request => request.Prompt)
            .SetValidator(new PromptValidator()!);
    }
}
