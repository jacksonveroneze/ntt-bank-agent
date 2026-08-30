using Microsoft.Agents.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent;

public static class AgentExecutor
{
    public static async Task<AgentOutput> RunAsync(
        AIAgent agent,
        AgentInput input,
        CancellationToken cancellationToken)
    {
        var response = await agent.RunAsync(
            input.Prompt, cancellationToken: cancellationToken);

        return new AgentOutput(
            Message: response.Text,
            ConversationId: input.ConversationId,
            response);
    }
}
