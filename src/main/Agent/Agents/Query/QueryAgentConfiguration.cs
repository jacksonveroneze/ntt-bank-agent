using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Agents.Query;

public sealed class QueryAgentConfiguration
{
    public const string SectionName = "Ai:Agents:Query";

    public Provider Provider { get; init; } = Provider.None;

    public required string Model { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public string? SystemPrompt { get; init; }

    public float Temperature { get; init; }

    public bool AllowMultipleToolCalls { get; init; }
}
