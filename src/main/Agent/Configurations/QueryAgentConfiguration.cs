using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Configurations;

public sealed class QueryAgentConfiguration
{
    public const string SectionName = "Ai:Agents:Query";

    public Provider Provider { get; init; } = Provider.None;

    public required string Model { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public float Temperature { get; init; }
}
