using NttBank.Agent.Agent.Enums;

namespace NttBank.Agent.Agent.Abstractions.Agent;

public abstract class AgentConfiguration
{
    public Provider Provider { get; init; } = Provider.None;

    public required string Model { get; init; }

    public required string Persona { get; init; }

    public float Temperature { get; init; }

    public bool AllowMultipleToolCalls { get; init; }
}
