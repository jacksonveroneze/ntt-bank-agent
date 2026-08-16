using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Memory;

internal sealed class HybridCacheSessionStore(
    HybridCache cache) : ISessionStore
{
    public async ValueTask<JsonElement?> LoadAsync(string key, CancellationToken ct)
    {
        var json = await cache.GetOrCreateAsync(
            key,
            factory: static _ => new ValueTask<JsonElement?>(result: null),
            cancellationToken: ct);

        return json;
    }

    public async ValueTask SaveAsync(string key, JsonElement session, CancellationToken ct)
    {
        await cache.SetAsync(key, session,
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(30),
                Flags = HybridCacheEntryFlags.DisableLocalCache,
            },
            cancellationToken: ct);
    }
}
