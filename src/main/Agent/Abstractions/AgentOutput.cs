using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public record AgentOutput(
    string Message,
    string? ConversationId,
    AgentResponse? AgentResponse = null);
