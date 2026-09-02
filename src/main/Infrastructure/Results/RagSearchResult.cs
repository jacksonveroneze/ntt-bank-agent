namespace NttBank.Agent.Infrastructure.Results;

public sealed record RagSearchResult(
    IReadOnlyList<RagChunkResult>? Results);
