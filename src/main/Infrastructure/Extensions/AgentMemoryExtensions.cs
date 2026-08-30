using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Valkey;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Infrastructure.Configurations;
using NttBank.QueryAgent.Infrastructure.Conversation;
using Valkey.Glide;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentMemoryExtensions
{
    private const string InMemoryProvider = "InMemory";

    extension(IServiceCollection services)
    {
        public IServiceCollection AddAgentMemory(AppConfiguration appConfiguration)
        {
            ArgumentNullException.ThrowIfNull(appConfiguration);

            var memoryConfiguration = appConfiguration.Ai!.AgentMemory;

            services.AddSingleton<IConversationContext, HttpConversationContext>();

            if (!memoryConfiguration.Enabled)
            {
                return services;
            }

            var provider = memoryConfiguration.Provider;

            return provider.Equals(InMemoryProvider,
                StringComparison.OrdinalIgnoreCase)
                ? services.AddInMemoryChatHistory()
                : services.AddValkeyChatHistory(memoryConfiguration);
        }

        private IServiceCollection AddInMemoryChatHistory()
        {
            services.AddSingleton<ChatHistoryProvider>(static _ =>
                new InMemoryChatHistoryProvider());

            return services;
        }

        private IServiceCollection AddValkeyChatHistory(
            AgentMemoryConfiguration memoryConfiguration)
        {
            services.AddSingleton<ConversationStateInitializer>();

            services.AddSingleton<ChatHistoryProvider>(sp =>
            {
                var init = sp.GetRequiredService<ConversationStateInitializer>();

                return new ValkeyChatHistoryProvider(
                    sp.GetRequiredService<IConnectionMultiplexer>(),
                    stateInitializer: init.Initialize,
                    options: new ValkeyChatHistoryProviderOptions
                    {
                        KeyPrefix = memoryConfiguration.KeyPrefix,
                        MaxMessages = memoryConfiguration.MaxMessages,
                        MaxMessagesToRetrieve = memoryConfiguration.MaxMessagesToRetrieve,
                    },
                    loggerFactory: sp.GetRequiredService<ILoggerFactory>());
            });

            return services;
        }
    }
}
