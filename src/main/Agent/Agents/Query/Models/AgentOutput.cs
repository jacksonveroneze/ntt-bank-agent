namespace NttBank.QueryAgent.Agent.Agents.Query.Models;

public sealed record AgentOutput(
    string Message,
    int MessageCount,
    IReadOnlyCollection<string> Messages,
    string? RawText);
