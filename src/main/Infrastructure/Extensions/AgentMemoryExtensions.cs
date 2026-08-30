using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Valkey;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Infrastructure.Configurations;
using Valkey.Glide;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentMemoryExtensions
{
    public static IServiceCollection AddAgentMemory(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(appConfiguration.Cache.Endpoint!));

        services.AddSingleton<HistoryStateInitializer>();

        services.AddSingleton<ChatHistoryProvider>(sp =>
        {
            var init = sp.GetRequiredService<HistoryStateInitializer>();

            return new ValkeyChatHistoryProvider(
                sp.GetRequiredService<IConnectionMultiplexer>(),
                stateInitializer: init.Initialize,
                options: new ValkeyChatHistoryProviderOptions
                {
                    KeyPrefix = "chat_history",
                    MaxMessages = 100,
                    MaxMessagesToRetrieve = 50,

                    StoreInputResponseMessageFilter = msgs =>
                        msgs.Where(m => m.Role == ChatRole.User
                                        && !string.IsNullOrWhiteSpace(m.Text)),

                    ProvideOutputMessageFilter = msgs =>
                        msgs.Where(m => m.Role == ChatRole.User
                                        && !string.IsNullOrWhiteSpace(m.Text)),

                    StoreInputRequestMessageFilter = msgs =>
                        msgs.Where(m => m.Role == ChatRole.User),
                },
                loggerFactory: sp.GetRequiredService<ILoggerFactory>());
        });

        return services;
    }

    public sealed class HistoryStateInitializer(IHttpContextAccessor httpContextAccessor)
    {
        private const string ConversationHeader = "X-Conversation-Id";
        private const string CustomerHeader = "X-Customer-Id";

        public ValkeyChatHistoryProvider.State Initialize(AgentSession? _)
        {
            var http = httpContextAccessor.HttpContext
                       ?? throw new InvalidOperationException(
                           "Sem HttpContext ao inicializar a sessão de histórico.");

            var conversationId = http.Request.Headers[ConversationHeader].ToString();

            var customerId = http.Request.Headers[CustomerHeader].ToString();

            var scopedId = string.IsNullOrWhiteSpace(customerId)
                ? conversationId
                : $"{customerId}::{conversationId}";

            return new ValkeyChatHistoryProvider.State(scopedId);
        }
    }
}
