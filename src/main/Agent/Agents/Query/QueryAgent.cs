using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgent(
    IQueryAgentProvider provider) : IQueryAgent
{
    public async Task<AgentOutput> RunAsync(
        AgentInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        var agent = await provider
            .GetAsync(cancellationToken);

        var response = await agent.RunAsync(
            message: input.Prompt,
            session: input.Session,
            options: new AgentRunOptions
            {
                ResponseFormat = new ChatResponseFormatText(),
            },
            cancellationToken: cancellationToken);

        return AgentOutputMapper.ToOutput(response);
    }
}
