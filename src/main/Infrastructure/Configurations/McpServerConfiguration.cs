using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations;

[ExcludeFromCodeCoverage]
public record McpServerConfiguration
{
    [Required]
    public required string Name { get; init; }

    [Required]
    public required Uri Address { get; init; }

    [Required]
    public required McpOAuthOptions OAuth { get; init; }
}
