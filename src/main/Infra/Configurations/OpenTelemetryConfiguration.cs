using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infra.Configurations;

[ExcludeFromCodeCoverage]
public sealed record OpenTelemetryConfiguration
{
    public Uri? EndpointTracing { get; init; }
}
