using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Hosting;
using NttBank.Agent.Infrastructure.Configurations;
using Valkey.Glide;

namespace NttBank.Agent.Infrastructure.Conversation;

public sealed class ValkeyAgentSessionStore(
    IConnectionMultiplexer connection,
    AppConfiguration appConfiguration)
    : AgentSessionStore
{
    private IDatabase Db => connection.GetDatabase();

    public override async ValueTask SaveSessionAsync(
        AIAgent agent,
        string sessionStoreId,
        AgentSession session,
        CancellationToken cancellationToken = default)
    {
        JsonElement snapshot = await agent
            .SerializeSessionAsync(session, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        await Db.StringSetAsync(
            Key(agent, sessionStoreId),
            snapshot.GetRawText(),
            expiry: TimeSpan.FromMilliseconds(appConfiguration.Ai!.AgentMemory.TtlMs));
    }

    public override async ValueTask<AgentSession> GetSessionAsync(
        AIAgent agent,
        string sessionStoreId,
        CancellationToken cancellationToken = default)
    {
        var raw = await Db.StringGetAsync(Key(agent, sessionStoreId));

        if (raw.IsNullOrEmpty)
        {
            return await agent.CreateSessionAsync(cancellationToken);
        }

        using var doc = JsonDocument.Parse(raw!.ToString());

        return await agent
            .DeserializeSessionAsync(doc.RootElement, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public override ValueTask DeleteSessionAsync(
        AIAgent agent,
        string sessionStoreId,
        CancellationToken cancellationToken = default)
    {
        return new ValueTask(Db.KeyDeleteAsync(Key(agent, sessionStoreId)));
    }

    private string Key(AIAgent agent, string sessionStoreId)
    {
        return $"{appConfiguration.Ai!.AgentMemory.KeyPrefix}:{agent.Name}:{sessionStoreId}";
    }
}
