using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public record AgentOutput(
    string Message,
    AgentResponse? AgentResponse = null);
