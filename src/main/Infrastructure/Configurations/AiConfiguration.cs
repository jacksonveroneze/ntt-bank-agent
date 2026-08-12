using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiConfiguration
{
    public IDictionary<string, AiProviderConfiguration> Providers { get; init; } =
        new Dictionary<string, AiProviderConfiguration>(StringComparer.OrdinalIgnoreCase);

    public IDictionary<string, AiAgentConfiguration> Agents { get; init; } =
        new Dictionary<string, AiAgentConfiguration>(StringComparer.OrdinalIgnoreCase);
}
