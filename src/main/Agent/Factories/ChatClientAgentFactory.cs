using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Factories;

public static class ChatClientAgentFactory
{
    public static AIAgent Create(
        ChatAgentDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        var chatAgent = new ChatClientAgent(
            descriptor.ChatClient,
            new ChatClientAgentOptions
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

        return chatAgent
            .AsBuilder()
            .UseOpenTelemetry(
                descriptor.Name,
                config => { config.EnableSensitiveData = descriptor.EnableSensitiveData; })
            .Build();
    }
}
