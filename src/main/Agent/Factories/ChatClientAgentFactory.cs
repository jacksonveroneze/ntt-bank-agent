using Microsoft.Agents.AI;
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

public static class ChatClientAgentFactory
{
    public static AIAgent Create(ChatAgentDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        var agent = new ChatClientAgent(descriptor.ChatClient, new ChatClientAgentOptions
        {
            Name = descriptor.Name,
            Description = descriptor.Description,
            ChatOptions = new ChatOptions
            {
                ModelId = descriptor.ModelId,
                Instructions = descriptor.Instructions,
                Temperature = descriptor.Temperature,
                Tools = descriptor.Tools,
                ToolMode = ChatToolMode.Auto,
                AllowMultipleToolCalls = descriptor.AllowMultipleToolCalls,
            },
        });

        return agent
            .AsBuilder()
            .UseOpenTelemetry(descriptor.Name, c => c.EnableSensitiveData = descriptor.EnableSensitiveData)
            .Build();
    }
}
