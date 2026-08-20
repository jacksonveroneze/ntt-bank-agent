using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Factories;

public static class ChatClientAgentFactory
{
    public static AIAgent Create(
        AgentConfiguration configuration,
        IChatClient chatClient,
        bool enableSensitiveData,
        IList<AITool>? tools = null)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(chatClient);

        var chatAgent = new ChatClientAgent(chatClient,
            new ChatClientAgentOptions
            {
                Name = configuration.Name,
                Description = configuration.Description,
                ChatOptions = new ChatOptions
                {
                    ModelId = configuration.Model,
                    Instructions = configuration.SystemPrompt,
                    Temperature = configuration.Temperature,
                    Tools = tools,
                    ToolMode = ChatToolMode.Auto,
                    AllowMultipleToolCalls = configuration.AllowMultipleToolCalls,
                },
            });

        return chatAgent
            .AsBuilder()
            .UseOpenTelemetry(configuration.Name,
                config => config.EnableSensitiveData = enableSensitiveData)
            .Build();
    }
}
