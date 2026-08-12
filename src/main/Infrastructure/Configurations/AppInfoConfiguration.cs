using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AppInfoConfiguration
{
    public string? Name { get; init; }

    public Version? Version { get; init; }
}
