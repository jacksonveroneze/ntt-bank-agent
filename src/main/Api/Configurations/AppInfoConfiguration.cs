using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Api.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AppInfoConfiguration
{
    public required string Name { get; init; }

    public required Version Version { get; init; }
}
