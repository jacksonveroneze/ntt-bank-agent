namespace NttBank.QueryAgent.Infrastructure.Results;

public sealed record RagSearchResult(
    IReadOnlyList<RagChunkResult>? Results);
