using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Valkey;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Memory;

namespace NttBank.Agent.Infrastructure.Conversation;

[ExcludeFromCodeCoverage]
public sealed class ConversationStateInitializer(
    IConversationContext conversationContext)
{
    public ValkeyChatHistoryProvider.State Initialize(AgentSession? _)
    {
        var conversationId = conversationContext.GetConversationId();
        var customerId = conversationContext.GetCustomerId();

        var scopedId = string.IsNullOrWhiteSpace(customerId)
            ? conversationId
            : $"{customerId}::{conversationId}";

        return new ValkeyChatHistoryProvider.State(scopedId);
    }
}
