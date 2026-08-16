using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Api.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiConfiguration
{
    public IReadOnlyDictionary<Provider, AiProviderConfiguration> Providers { get; init; } =
        new Dictionary<Provider, AiProviderConfiguration>();
}
