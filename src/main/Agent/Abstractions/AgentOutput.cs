namespace NttBank.QueryAgent.Agent.Abstractions;

public record AgentOutput(
    string Message,
    int MessageCount,
    IReadOnlyCollection<string> Messages,
    string? RawText);
