using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Agents.Query.Models;

namespace NttBank.QueryAgent.Agent.Agents.Query.Middleware;

internal static class AgentGuardrails
{
    private static readonly string[] ForbiddenPatterns =
    [
        "ignore"
    ];

    internal static Task<AgentResponse> ValidateAgentRunAsync(
        IEnumerable<ChatMessage> messages,
        AgentSession? session,
        AgentRunOptions? options,
        AIAgent innerAgent,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(messages);
        ArgumentNullException.ThrowIfNull(innerAgent);

        IEnumerable<ChatMessage> chatMessages = messages 
            as ChatMessage[] ?? messages.ToArray();
       
        var userMessage = chatMessages
            .LastOrDefault(message => message.Role == ChatRole.User)
            ?.Text ?? string.Empty;

        if (!HasForbiddenIntent(userMessage))
        {
            return innerAgent.RunAsync(
                chatMessages, session, options, cancellationToken);
        }

        AgentResponse response = new(
        [
            new ChatMessage(
                ChatRole.Assistant,
                DefaultAgentOutputMessages.SafeRefusalMessage),
        ]);

        return Task.FromResult(response);

    }

    private static bool HasForbiddenIntent(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return false;
        }

        return ForbiddenPatterns.Any(pattern =>
            userMessage.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
