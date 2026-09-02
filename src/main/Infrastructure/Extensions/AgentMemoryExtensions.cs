using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Valkey;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Memory;
using NttBank.Agent.Infrastructure.Configurations;
using NttBank.Agent.Infrastructure.Conversation;
using Valkey.Glide;

namespace NttBank.Agent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentMemoryExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAgentMemory(
            AppConfiguration appConfiguration)
        {
            ArgumentNullException.ThrowIfNull(appConfiguration);

            var memoryConfiguration = appConfiguration.Ai!.AgentMemory;

            services.AddSingleton<IConversationContext, HttpConversationContext>();

            if (!memoryConfiguration.Enabled)
            {
                return services;
            }

            services.AddValkeyChatHistory(memoryConfiguration);
            
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
