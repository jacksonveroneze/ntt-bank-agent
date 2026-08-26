using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Rag;

namespace NttBank.QueryAgent.Agent.Factories;

public static class ChatClientAgentFactory
{
    public static AIAgent Create(
        string name,
        string description,
        string instructions,
        IChatClient chatClient,
        AgentConfiguration configuration,
        ILoggerFactory loggerFactory,
        bool enableSensitiveData,
        IList<AITool>? tools = null,
        ChatHistoryProvider? historyProvider = null,
        RagSearchAdapter? ragAdapter = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentException.ThrowIfNullOrEmpty(description);
        ArgumentException.ThrowIfNullOrEmpty(instructions);
        ArgumentNullException.ThrowIfNull(chatClient);
        ArgumentNullException.ThrowIfNull(configuration);

        var textSearchOptions = new TextSearchProviderOptions
        {
            SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
            RecentMessageMemoryLimit = 6, // Parametrizado
        };

        var chatOptions = new ChatClientAgentOptions
        {
            Id = $"agent-id-{name}",
            Name = name,
            Description = description,
            ChatOptions = new ChatOptions
            {
                ModelId = configuration.Model,
                Instructions = instructions,
                Temperature = configuration.Temperature,
                Tools = tools,
                ToolMode = ChatToolMode.Auto,
                AllowMultipleToolCalls =
                    configuration.AllowMultipleToolCalls,
            },
            ChatHistoryProvider = historyProvider,
        };

        if (ragAdapter != null)
        {
            chatOptions.AIContextProviders =
            [
                new TextSearchProvider(
                    ragAdapter.SearchAsync, textSearchOptions),
            ];
        }

        var chatAgent = new ChatClientAgent(
            chatClient, chatOptions, loggerFactory);

        return chatAgent
            .AsBuilder()
            .UseOpenTelemetry(name,
                config => config.EnableSensitiveData = enableSensitiveData)
            .Build();
    }
}
