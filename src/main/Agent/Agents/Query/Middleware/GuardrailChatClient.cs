using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Agents.Query.Middleware;

public sealed class GuardrailChatClient(AIAgent inner) : DelegatingAIAgent(inner)
{
    protected override Task<AgentResponse> RunCoreAsync(
        IEnumerable<ChatMessage> messages,
        AgentSession? session = null,
        AgentRunOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        foreach (var message in messages)
        {
            Console.WriteLine($"  - {message.Role}: {message.Text}");
        }

        return base.RunCoreAsync(messages, session, options, cancellationToken);
    }
}
