using System.Diagnostics.CodeAnalysis;

namespace NttBank.Agent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record OpenTelemetryConfiguration
{
    public Uri? EndpointTracing { get; init; }
}
