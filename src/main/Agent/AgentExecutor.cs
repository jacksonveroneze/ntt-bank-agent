using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents;

namespace NttBank.QueryAgent.Agent;

public static class AgentExecutor
{
    public static async Task<AgentOutput> RunAsync(
        IAgentProvider provider,
        AgentInput input,
        CancellationToken cancellationToken)
    {
        var agent = await provider.GetAsync(cancellationToken);

        var response = await agent.RunAsync(
            input.Prompt, cancellationToken: cancellationToken);

        return AgentOutputMapper.ToOutput(response);
    }
}
