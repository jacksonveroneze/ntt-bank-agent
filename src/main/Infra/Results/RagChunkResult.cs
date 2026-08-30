namespace NttBank.QueryAgent.Infra.Results;

public sealed record RagChunkResult(
    string Content,
    string? DocumentName,
    Uri? DocumentUrl);
