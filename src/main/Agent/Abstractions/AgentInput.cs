using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public record AgentInput(
    string Prompt,
    AgentSession? Session = null);
