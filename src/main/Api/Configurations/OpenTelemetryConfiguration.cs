using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Api.Configurations;

[ExcludeFromCodeCoverage]
public sealed record OpenTelemetryConfiguration
{
    public Uri? EndpointTracing { get; init; }
}
