using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Rag;

public sealed class RagSearchAdapter
{
    public Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAsync(
        string query, CancellationToken cancellationToken)
    {
        try
        {
            // chama SUA API RAG — ela faz embedding (ollama) + busca vetorial (pgvector)
            // var response = await httpClient.PostAsJsonAsync(
            //     "search", new RagSearchRequest(query, TopK: 5), cancellationToken);
            //
            // if (!response.IsSuccessStatusCode)
            // {
            //     logger.LogWarning("API RAG retornou {Status} para a busca", response.StatusCode);
            //     return [];   // falha de RAG → sem contexto, não derruba o agente
            // }
            //
            // var payload = await response.Content
            //     .ReadFromJsonAsync<RagSearchResponse>(cancellationToken: cancellationToken);

            var ragReults = new List<RagChunk>
            {
                new(
                    Content: "Exemplo de trecho recuperado do documento.",
                    DocumentName: "Documento Exemplo",
                    DocumentUrl: new Uri("https://exemplo.com/documento")
                ),
                new RagChunk(
                    Content: "Exemplo de trecho recuperado do documento.",
                    DocumentName: "Documento Exemplo",
                    DocumentUrl: new Uri("https://exemplo.com/documento")
                ),
            };

            var payload = new RagSearchResponse(Results: ragReults);

            var result = payload.Results.Select(r =>
                new TextSearchProvider.TextSearchResult
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
            return Task.FromResult<IEnumerable<TextSearchProvider.TextSearchResult>>([]);
        }
    }
}

// contratos da SUA API RAG (ajuste aos campos reais)
public sealed record RagSearchRequest(string Query, int TopK);

public sealed record RagSearchResponse(IReadOnlyList<RagChunk> Results);

public sealed record RagChunk(string Content, string? DocumentName, Uri? DocumentUrl);
