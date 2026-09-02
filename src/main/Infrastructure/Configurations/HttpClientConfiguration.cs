using System.Diagnostics.CodeAnalysis;

namespace NttBank.Agent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record HttpClientConfiguration
{
    public required string Name { get; init; }
    
    public required Uri Address { get; init; }
}
