namespace NttBank.QueryAgent.Agent.Abstractions;

public sealed record AgentOutput(
    string Message,
    int MessageCount,
    IReadOnlyCollection<string> Messages,
    string? RawText);
