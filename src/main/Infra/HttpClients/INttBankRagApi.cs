using NttBank.QueryAgent.Infra.Results;
using Refit;

namespace NttBank.QueryAgent.Infra.HttpClients;

public interface INttBankRagApi
{
    [Get("/v1/rag-search")]
    Task<RagSearchResult> SearchAsync(
        [Query("query")] string query,
        [Query("topK")] int topK,
        CancellationToken cancellationToken);
}
