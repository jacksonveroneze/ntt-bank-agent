using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Query;

public sealed class QueryAgentProvider(
    ILogger<QueryAgentProvider> logger,
    IOptionsMonitor<QueryAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<QueryAgentConfiguration>(
            logger, options, agentBuilder),
        IQueryAgentProvider
{
    public override string Name => "query";

    protected override string Description =>
        QueryConstants.Description;

    protected override string Invariants =>
        QueryConstants.SystemPrompt;

    protected override ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return mcpQueryToolService.GetToolsAsync(cancellationToken);
    }
}
