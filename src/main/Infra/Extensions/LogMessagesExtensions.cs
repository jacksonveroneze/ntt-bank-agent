using Microsoft.Extensions.Logging;

namespace NttBank.QueryAgent.Infra.Extensions;

internal static partial class LogMessagesExtensions
{
    [LoggerMessage(
        EventId = 1100,
        Level = LogLevel.Information,
        Message = "RAG search completed with {ResultCount} results")]
    public static partial void RagSearchCompleted(
        this ILogger logger,
        int resultCount);

    [LoggerMessage(
        EventId = 1101,
        Level = LogLevel.Error,
        Message = "RAG search failed")]
    public static partial void RagSearchFailed(
        this ILogger logger,
        Exception exception);
}
