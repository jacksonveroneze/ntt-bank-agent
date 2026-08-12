namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Models;

public record BaseAgentResponse
{
    public string? Message { get; init; }

    public AgentDebugResponse? Debug { get; init; }
}
