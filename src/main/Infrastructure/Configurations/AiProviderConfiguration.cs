using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public sealed record AiProviderConfiguration
{
    public bool Enabled { get; init; } = true;

    public string? Endpoint { get; init; }

    public string? ApiKey { get; init; }

    public string? Model { get; init; }
    
    public bool EnableSensitiveData { get; init; }
}
