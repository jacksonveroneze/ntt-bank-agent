using Microsoft.Extensions.Logging;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

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

    [LoggerMessage(
        EventId = 1200,
        Level = LogLevel.Information,
        Message = "MCP server {Server} connected with {ToolCount} tools")]
    public static partial void McpConnected(
        this ILogger logger,
        string server,
        int toolCount);

    [LoggerMessage(
        EventId = 1201,
        Level = LogLevel.Error,
        Message = "MCP server {Server} connection failed")]
    public static partial void McpConnectionFailed(
        this ILogger logger,
        string server,
        Exception exception);
}
