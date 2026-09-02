using NttBank.Agent.Infrastructure.Results;
using Refit;

namespace NttBank.Agent.Infrastructure.HttpClients;

public interface INttBankRagApi
{
    [Get("/rag/search")]
    Task<RagSearchResult> SearchAsync(
        [Query("query")] string query,
        [Query("topK")] int topK,
        CancellationToken cancellationToken);
}
