using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Extensions;

internal static partial class LogMessagesExtensions
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "Building agent {Agent} with provider {Provider} and {ToolCount} tools")]
    public static partial void AgentBuilding(
        this ILogger logger,
        string agent,
        Provider provider,
        int toolCount);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "Agent {Agent} built successfully")]
    public static partial void AgentBuilt(
        this ILogger logger,
        string agent);

    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Information,
        Message = "Invalidating cached agent {Agent} due to configuration change")]
    public static partial void AgentInvalidated(
        this ILogger logger,
        string agent);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Warning,
        Message = "Agent {Agent} build discarded because configuration changed during build")]
    public static partial void AgentBuildDiscarded(
        this ILogger logger,
        string agent);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Error,
        Message = "Failed to build agent {Agent}")]
    public static partial void AgentBuildFailed(
        this ILogger logger,
        Exception exception,
        string agent);

    [LoggerMessage(
        EventId = 1005,
        Level = LogLevel.Information,
        Message = "Applying RAG to agent {Agent}")]
    public static partial void AgentApplyingRag(
        this ILogger logger,
        string agent);

    [LoggerMessage(
        EventId = 1007,
        Level = LogLevel.Information,
        Message = "Building handoff workflow with triage {Triage} and {SpecialistCount} specialists")]
    public static partial void HandoffBuilding(
        this ILogger logger,
        string triage,
        int specialistCount);

    [LoggerMessage(
        EventId = 1008,
        Level = LogLevel.Information,
        Message = "Handoff workflow built successfully")]
    public static partial void HandoffBuilt(
        this ILogger logger);

    [LoggerMessage(
        EventId = 1010,
        Level = LogLevel.Information,
        Message = "Executing agent {Agent} for conversation {ConversationId}")]
    public static partial void AgentExecuting(
        this ILogger logger,
        string agent,
        string conversationId);

    [LoggerMessage(
        EventId = 1011,
        Level = LogLevel.Information,
        Message = "Agent {Agent} execution completed for conversation {ConversationId}")]
    public static partial void AgentExecuted(
        this ILogger logger,
        string agent,
        string conversationId);
}
