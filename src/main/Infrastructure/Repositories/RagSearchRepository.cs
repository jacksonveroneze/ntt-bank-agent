using MapsterMapper;
using Microsoft.Extensions.Logging;
using NttBank.Agent.Agent.Abstractions.Rag;
using NttBank.Agent.Infrastructure.Extensions;
using NttBank.Agent.Infrastructure.HttpClients;
using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.Agent.Infrastructure.Repositories;

public sealed class RagSearchRepository(
    IMapper mapper,
    INttBankRagApi api,
    ILogger<RagSearchRepository> logger) : IRagSearchRepository
{
    private const int TopK = 10;

    public async Task<IEnumerable<TextSearchResult>> SearchAsync(
        string query,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await api.SearchAsync(
                query, TopK, cancellationToken);

            var results = result.Results ?? [];

            logger.RagSearchCompleted(results.Count);

            var items = mapper.Map<IEnumerable<TextSearchResult>>(
                results);

            return items;
        }
        catch (Exception ex)
        {
            logger.RagSearchFailed(ex);

            return [];
        }
    }
}
