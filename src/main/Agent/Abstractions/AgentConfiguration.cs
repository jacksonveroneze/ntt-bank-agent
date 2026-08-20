using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Abstractions;

public abstract class AgentConfiguration
{
    public Provider Provider { get; init; } = Provider.None;

    public required string Model { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public required string SystemPrompt { get; init; }

    public float Temperature { get; init; }

    public bool AllowMultipleToolCalls { get; init; }
}
