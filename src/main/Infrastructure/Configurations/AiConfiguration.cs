using System.Diagnostics.CodeAnalysis;
using NttBank.Agent.Agent.Enums;

namespace NttBank.Agent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiConfiguration
{
    public required AgentMemoryConfiguration AgentMemory { get; init; }
    
    public IReadOnlyDictionary<Provider, AiProviderConfiguration> Providers { get; init; } =
        new Dictionary<Provider, AiProviderConfiguration>();
}
