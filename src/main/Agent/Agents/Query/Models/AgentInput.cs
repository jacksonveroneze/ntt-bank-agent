using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Agents.Query.Models;

public sealed record AgentInput(
    string Prompt,
    AgentSession? Session = null);
