using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infra.Configurations;

[ExcludeFromCodeCoverage]
public sealed record HttpClientConfiguration
{
    public required string Name { get; init; }
    
    public required Uri Address { get; init; }
}
