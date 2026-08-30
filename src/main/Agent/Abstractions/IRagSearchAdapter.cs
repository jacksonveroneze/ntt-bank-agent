using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IRagSearchAdapter
{
    Task<IEnumerable<TextSearchResult>> SearchAsync(
        string query,
        CancellationToken cancellationToken);
}
