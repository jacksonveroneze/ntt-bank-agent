using Microsoft.Agents.AI;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgent(
    IQueryAgentProvider provider,
    ISessionStore sessionStore) : IQueryAgent
{
    public async Task<AgentOutput> RunAsync(
        AgentInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        AIAgent agent = await provider
            .GetAsync(cancellationToken);

        var conversationId = GetConversationId(input);

        AgentSession agentSession = await GetSessionAsync(
            conversationId, agent, cancellationToken);

        var response = await agent.RunAsync(
            message: input.Prompt,
            session: agentSession,
            
            cancellationToken: cancellationToken);

        await StoreSessionAsync(conversationId, 
            agentSession, agent, cancellationToken);

        return AgentOutputMapper.ToOutput(
            response, conversationId);
    }

    private static string GetConversationId(AgentInput input)
    {
        var conversationId = !string.IsNullOrEmpty(input.ConversationId)
            ? input.ConversationId
            : Guid.NewGuid().ToString("N");

        return conversationId;
    }

    private async ValueTask<AgentSession> GetSessionAsync(
        string conversationId,
        AIAgent agent,
        CancellationToken cancellationToken)
    {
        var session = await sessionStore
            .LoadAsync(conversationId, cancellationToken);

        if (session is not null)
        {
            return await agent.DeserializeSessionAsync(
                session.Value, cancellationToken: cancellationToken);
        }

        return await agent
            .CreateSessionAsync(cancellationToken);
    }

    private async Task StoreSessionAsync(
        string conversationId,
        AgentSession agentSession,
        AIAgent agent,
        CancellationToken cancellationToken)
    {
        var json = await agent.SerializeSessionAsync(
            agentSession, cancellationToken: cancellationToken);

        await sessionStore.SaveAsync(
            conversationId, json, cancellationToken);
    }
}
