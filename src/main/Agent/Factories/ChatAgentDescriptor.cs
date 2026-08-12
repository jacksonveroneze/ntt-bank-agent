using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Factories;

public sealed record ChatAgentDescriptor
{
    public required IChatClient ChatClient { get; init; }
    
    public required string Name { get; init; }
    
    public required string Description { get; init; }
    
    public required string ModelId { get; init; }
    
    public required string Instructions { get; init; }
    
    public float Temperature { get; init; }
    
    public IList<AITool>? Tools { get; init; }
    
    public bool? AllowMultipleToolCalls { get; init; }
    
    public bool EnableSensitiveData { get; init; }
}
