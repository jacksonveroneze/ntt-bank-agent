namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Models;

public sealed record AgentDebugResponse(
    int MessageCount,
    IReadOnlyCollection<string> Messages,
    string? RawText);
