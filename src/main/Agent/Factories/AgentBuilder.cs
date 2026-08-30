using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Extensions;

namespace NttBank.QueryAgent.Agent.Factories;

public sealed class AgentBuilder(
    IChatClientResolver chatClientResolver,
    ILoggerFactory loggerFactory,
    IHostEnvironment hostEnvironment,
    ChatHistoryProvider? historyProvider = null)
    : IAgentBuilder
{
    private const int RecentMessageMemoryLimit = 6;

    private const TextSearchProviderOptions.TextSearchBehavior
        DefaultSearchBehavior =
            TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke;

    private readonly ILogger _logger =
        loggerFactory.CreateLogger<AgentBuilder>();

    public AIAgent Build(AgentBuildContext context)
    {
        var chatClient = chatClientResolver.Resolve(
            context.Configuration.Provider);

        var chatOptions = BuildChatOptions(context);

        if (context.RagAdapter is not null)
        {
            _logger.AgentApplyingRag(context.Name);
            ApplyRag(chatOptions, context.RagAdapter);
        }

        var chatAgent = new ChatClientAgent(
            chatClient, chatOptions, loggerFactory);

        return ApplyOpenTelemetry(chatAgent, context.Name);
    }

    private ChatClientAgentOptions BuildChatOptions(
        AgentBuildContext context)
    {
        var chatOptions = new ChatClientAgentOptions
        {
            Id = $"agent-id-{context.Name}",
            Name = context.Name,
            Description = context.Description,
            ChatOptions = new ChatOptions
            {
                ModelId = context.Configuration.Model,
                Instructions = context.Instructions,
                Temperature = context.Configuration.Temperature,
                Tools = context.Tools,
                ToolMode = ChatToolMode.Auto,
                AllowMultipleToolCalls =
                    context.Configuration.AllowMultipleToolCalls,
            },
            ChatHistoryProvider = historyProvider,
        };

        return chatOptions;
    }

    private static void ApplyRag(
        ChatClientAgentOptions chatOptions,
        IRagSearchAdapter ragAdapter)
    {
        var textSearchOptions = new TextSearchProviderOptions
        {
            SearchTime = DefaultSearchBehavior,
            RecentMessageMemoryLimit = RecentMessageMemoryLimit,
        };

        chatOptions.AIContextProviders =
        [
            new TextSearchProvider(
                ragAdapter.SearchAsync, textSearchOptions),
        ];
    }

    private AIAgent ApplyOpenTelemetry(
        AIAgent chatAgent,
        string name)
    {
        _logger.AgentApplyingOpenTelemetry(name);

        return chatAgent
            .AsBuilder()
            .UseOpenTelemetry(
                name,
                config =>
                    config.EnableSensitiveData =
                        hostEnvironment.IsDevelopment())
            .Build();
    }
}
