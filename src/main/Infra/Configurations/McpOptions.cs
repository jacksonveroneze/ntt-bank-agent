using System.ComponentModel.DataAnnotations;

namespace NttBank.QueryAgent.Infra.Configurations;

public sealed class McpOptions
{
    public const string SectionName = "Mcp";
    
    [Required] 
    public required Uri Endpoint { get; init; }
    
    public required McpOAuthOptions OAuth { get; init; }
}
