namespace NttBank.Agent.Infrastructure.Models;

public sealed record RagSearchRequest(
    string Query,
    int TopK);
