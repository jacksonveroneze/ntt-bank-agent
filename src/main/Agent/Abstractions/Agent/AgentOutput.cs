using Microsoft.Agents.AI;

namespace NttBank.Agent.Agent.Abstractions.Agent;

public record AgentOutput(
    string Message,
    string? ConversationId,
    AgentResponse? AgentResponse = null);
