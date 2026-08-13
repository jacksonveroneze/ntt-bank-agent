using System.ComponentModel.DataAnnotations;

namespace NttBank.QueryAgent.Agent.Configurations;

public sealed class McpOptions
{
    public const string SectionName = "Mcp";
    
    [Required] 
    public required Uri Endpoint { get; init; }
    
    public required McpOAuthOptions OAuth { get; init; }
}
