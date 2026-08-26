using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.QueryAgent.Agent.Rag;

public sealed class RagSearchAdapter
{
    public Task<IEnumerable<TextSearchResult>> SearchAsync(
        string query, CancellationToken cancellationToken)
    {
        try
        {
            var ragReults = new List<RagChunk>
            {
                new(
                    Content: "Exemplo de trecho recuperado do documento.",
                    DocumentName: "Documento Exemplo",
                    DocumentUrl: new Uri("https://exemplo.com/documento")
                ),
                new(
                    Content: "Exemplo de trecho recuperado do documento.",
                    DocumentName: "Documento Exemplo",
                    DocumentUrl: new Uri("https://exemplo.com/documento")
                ),
            };

            var payload = new RagSearchResponse(Results: ragReults);

            var result = payload.Results?.Select(r =>
                new TextSearchResult
                {
                    Text = r.Content,
                    SourceName = r.DocumentName,
                    SourceLink = r.DocumentUrl?.ToString(),
                }) ?? [];

            // log

            return Task.FromResult(result);
        }
        catch
        {
            // log
            return Task.FromResult<IEnumerable<TextSearchResult>>([]);
        }
    }
}

// contratos da SUA API RAG (ajuste aos campos reais)
public sealed record RagSearchRequest(string Query, int TopK);

public sealed record RagSearchResponse(IReadOnlyList<RagChunk>? Results);

public sealed record RagChunk(string Content, string? DocumentName, Uri? DocumentUrl);
