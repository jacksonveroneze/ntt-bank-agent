namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Models;

public record BaseAgentRequest
{
    public string? Prompt { get; init; }
}
