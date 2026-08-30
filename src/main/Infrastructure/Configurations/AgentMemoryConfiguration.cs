using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AgentMemoryConfiguration
{
    public const string DefaultProvider = "Valkey";

    public const string DefaultKeyPrefix = "chat_history";

    public const int DefaultMaxMessages = 100;

    public const int DefaultMaxMessagesToRetrieve = 50;

    public bool Enabled { get; init; } = true;

    public string Provider { get; init; } = DefaultProvider;

    public string KeyPrefix { get; init; } = DefaultKeyPrefix;

    public int MaxMessages { get; init; } = DefaultMaxMessages;

    public int MaxMessagesToRetrieve { get; init; } = DefaultMaxMessagesToRetrieve;
}
