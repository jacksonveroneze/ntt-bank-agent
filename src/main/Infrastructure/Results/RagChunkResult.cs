namespace NttBank.QueryAgent.Infrastructure.Results;

public sealed record RagChunkResult(
    string Content,
    string? DocumentName,
    Uri? DocumentUrl);
