using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiAgentConfiguration
{
    public string? Provider { get; init; }

    public string? Model { get; init; }

    public float? Temperature { get; init; }
}
