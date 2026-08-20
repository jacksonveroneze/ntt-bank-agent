using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Infra;

internal sealed class CacheSessionStore(
    HybridCache cache) : ISessionStore
{
    public async ValueTask<JsonElement?> LoadAsync(
        string key, 
        CancellationToken cancellationToken)
    {
        var json = await cache.GetOrCreateAsync(
            key,
            factory: static _ => 
                new ValueTask<JsonElement?>(result: null),
            cancellationToken: cancellationToken);

        return json;
    }

    public async ValueTask SaveAsync(
        string key, 
        JsonElement session, 
        CancellationToken cancellationToken)
    {
        await cache.SetAsync(key, session,
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                Flags = HybridCacheEntryFlags.DisableLocalCache,
            },
            cancellationToken: cancellationToken);
    }
}
