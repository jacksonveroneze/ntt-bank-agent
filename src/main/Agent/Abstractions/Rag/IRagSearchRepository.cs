using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.Agent.Agent.Abstractions.Rag;

public interface IRagSearchRepository
{
    Task<IEnumerable<TextSearchResult>> SearchAsync(
        string query,
        CancellationToken cancellationToken);
}
