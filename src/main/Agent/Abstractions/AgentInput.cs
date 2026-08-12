using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public sealed record AgentInput(
    string Prompt,
    AgentSession? Session = null);
