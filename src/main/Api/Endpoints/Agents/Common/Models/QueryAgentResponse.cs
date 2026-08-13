using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Common.Models;

public record BaseAgentResponse
{
    public string? Message { get; init; }

    public AgentResponse? Debug { get; init; }
}
