using MapsterMapper;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Infrastructure.Extensions;
using NttBank.QueryAgent.Infrastructure.HttpClients;
using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.QueryAgent.Infrastructure.Rag;

public sealed class RagSearchAdapter(
    IMapper mapper,
    INttBankRagApi api,
    ILogger<RagSearchAdapter> logger) : IRagSearchAdapter
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
