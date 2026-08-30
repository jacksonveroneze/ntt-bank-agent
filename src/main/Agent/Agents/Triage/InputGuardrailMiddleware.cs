using System.Runtime.CompilerServices;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Agents.Triage;

internal static class InputGuardrailMiddleware
{
    public static async Task<AgentResponse> InvokeAsync(
        IEnumerable<ChatMessage> messages,
        AgentSession? session,
        AgentRunOptions? options,
        AIAgent innerAgent,
        CancellationToken cancellationToken)
    {
        return await innerAgent.RunAsync(
            messages, session, options, cancellationToken);
    }

    public static async IAsyncEnumerable<AgentResponseUpdate> InvokeStreamingAsync(
        IEnumerable<ChatMessage> messages,
        AgentSession? session,
        AgentRunOptions? options,
        AIAgent innerAgent,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var update in innerAgent.RunStreamingAsync(
                           messages, session, options, cancellationToken))
        {
            yield return update;
        }
    }
}
