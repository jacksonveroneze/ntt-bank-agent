using System.Diagnostics.CodeAnalysis;

namespace NttBank.Agent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AgentMemoryConfiguration
{
    public bool Enabled { get; init; }

    public required string KeyPrefix { get; init; }

    public required int TtlMs { get; init; }
}
