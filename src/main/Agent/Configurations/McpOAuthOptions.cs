using System.ComponentModel.DataAnnotations;

namespace NttBank.QueryAgent.Agent.Configurations;

public sealed class McpOAuthOptions
{
    [Required] 
    public required Uri TokenEndpoint { get; init; }
    
    [Required] 
    public required string ClientId { get; init; }
    
    [Required] 
    public required string ClientSecret { get; init; }
    
    public required string Scope { get; init; }
    
    public required string Audience { get; init; }
}
