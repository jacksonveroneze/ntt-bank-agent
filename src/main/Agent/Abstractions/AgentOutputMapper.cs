using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

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
