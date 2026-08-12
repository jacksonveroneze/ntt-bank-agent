using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Api.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AppConfiguration
{
    public AppInfoConfiguration? Application { get; init; }

    public AiConfiguration? Ai { get; init; }
}
