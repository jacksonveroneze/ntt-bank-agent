using System.Text.Json;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface ISessionStore
{
    ValueTask<JsonElement?> LoadAsync(
        string key, 
        CancellationToken cancellationToken);
    
    ValueTask SaveAsync(
        string key, 
        JsonElement session, 
        CancellationToken cancellationToken);
}
