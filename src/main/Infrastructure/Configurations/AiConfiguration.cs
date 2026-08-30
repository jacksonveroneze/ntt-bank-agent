using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiConfiguration
{
    public required AgentMemoryConfiguration AgentMemory { get; init; }
    
    public IReadOnlyDictionary<Provider, AiProviderConfiguration> Providers { get; init; } =
        new Dictionary<Provider, AiProviderConfiguration>();
}
