using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Extensions;

namespace NttBank.QueryAgent.Agent;

public static class AgentExecutor
{
    public static async Task<AgentOutput> RunAsync(
        AIAgent agent,
        AgentInput input,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(nameof(AgentExecutor));
        var agentName = agent.Name ?? "unknown";
        var conversationId = input.ConversationId ?? "unknown";

        logger.AgentExecuting(agentName, conversationId);

        var response = await agent.RunAsync(
            input.Prompt, cancellationToken: cancellationToken);

        logger.AgentExecuted(agentName, conversationId);

        return new AgentOutput(
            response.Text,
            conversationId,
            response);
    }
}
