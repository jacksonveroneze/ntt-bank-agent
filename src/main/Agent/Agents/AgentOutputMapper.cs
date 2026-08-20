using Microsoft.Agents.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents;

internal static class AgentOutputMapper
{
    public static AgentOutput ToOutput(
        AgentResponse response,
        string? conversationId = null)
    {
        return new AgentOutput(
            Message: response.Text,
            ConversationId: conversationId,
            response);
    }
}
