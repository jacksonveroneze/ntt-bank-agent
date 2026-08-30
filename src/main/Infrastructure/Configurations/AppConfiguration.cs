using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Infrastructure.Configurations.Mcp;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AppConfiguration
{
    public required AppInfoConfiguration Application { get; init; }

    public required CacheConfiguration Cache { get; init; }
    
    public required AuthTokenAuthenticationConfiguration AuthTokenAuthentication { get; init; }

    public required OpenTelemetryConfiguration OpenTelemetry { get; init; }

    public required AiConfiguration? Ai { get; init; }
    
    public required HttpClientConfiguration HttpClientRagNttBank { get; init; }

    public required McpQueryConfiguration McpQuery { get; init; }

    public required McpCardsConfiguration McpCards { get; init; }
}
