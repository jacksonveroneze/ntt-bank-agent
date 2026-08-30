using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record OpenTelemetryConfiguration
{
    public Uri? EndpointTracing { get; init; }
}
