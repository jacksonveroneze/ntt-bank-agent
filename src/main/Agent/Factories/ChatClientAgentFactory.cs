using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Factories;

public static class ChatClientAgentFactory
{
    public static AIAgent Create(
        string name,
        string description,
        AgentConfiguration configuration,
        string instructions,
        IChatClient chatClient,
        bool enableSensitiveData,
        IList<AITool>? tools = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(instructions);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(chatClient);

        var chatAgent = new ChatClientAgent(chatClient,
            new ChatClientAgentOptions
            {
                Name = name,
                Description = description,
                ChatOptions = new ChatOptions
                {
                    ModelId = configuration.Model,
                    Instructions = instructions,
                    Temperature = configuration.Temperature,
                    Tools = tools,
                    ToolMode = ChatToolMode.Auto,
                    AllowMultipleToolCalls = configuration.AllowMultipleToolCalls,
                },
            });

        return chatAgent
            .AsBuilder()
            .UseOpenTelemetry(name,
                config => config.EnableSensitiveData = enableSensitiveData)
            .Build();
    }
}
