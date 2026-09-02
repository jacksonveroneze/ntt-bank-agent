using System.Diagnostics.CodeAnalysis;

namespace NttBank.Agent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AgentMemoryConfiguration
{
    private const int DefaultMaxMessages = 100;

    private const int DefaultMaxMessagesToRetrieve = 50;

    public bool Enabled { get; init; } = true;

    public required string Provider { get; init; }

    public required string KeyPrefix { get; init; }

    public int MaxMessages { get; init; } = DefaultMaxMessages;

    public int MaxMessagesToRetrieve { get; init; } = DefaultMaxMessagesToRetrieve;
}
