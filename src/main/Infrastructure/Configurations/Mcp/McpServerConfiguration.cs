using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations.Mcp;

[ExcludeFromCodeCoverage]
public abstract record McpServerConfiguration
{
    [Required]
    public required string Name { get; init; }

    [Required]
    public required Uri Address { get; init; }

    [Required]
    public required McpOAuthOptions OAuth { get; init; }
}
