using NttBank.Agent.Infrastructure.Models;
using NttBank.Agent.Infrastructure.Results;
using Refit;

namespace NttBank.Agent.Infrastructure.HttpClients;

public interface INttBankRagApi
{
    [Post("/v1/rag/search")]
    Task<RagSearchResult> SearchAsync(
        [Body] RagSearchRequest body,
        CancellationToken cancellationToken);
}
