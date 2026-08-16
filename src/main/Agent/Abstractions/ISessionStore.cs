using System.Text.Json;

namespace NttBank.QueryAgent.Agent.Abstractions;

internal interface ISessionStore
{
    ValueTask<JsonElement?> LoadAsync(string key, CancellationToken ct);
    
    ValueTask SaveAsync(string key, JsonElement session, CancellationToken ct);
}
