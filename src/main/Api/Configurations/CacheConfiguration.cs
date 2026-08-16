using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Api.Configurations;

[ExcludeFromCodeCoverage]
public sealed record CacheConfiguration
{
    public bool UseDistributed { get; init; }

    public string? Endpoint { get; init; }
}
