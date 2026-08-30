namespace NttBank.QueryAgent.Infra.Results;

public sealed record RagSearchResult(
    IReadOnlyList<RagChunkResult>? Results);
