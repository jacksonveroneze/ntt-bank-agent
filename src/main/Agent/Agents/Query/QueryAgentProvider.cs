using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Agents.Query;

public sealed class QueryAgentProvider(
    ILogger<QueryAgentProvider> logger,
    IOptionsMonitor<QueryAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    IHostEnvironment env,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<QueryAgentConfiguration>(logger, options, chatClientResolver, env),
        IQueryAgentProvider
{
    protected override string Invariants => QueryInstructions.SystemPrompt;

    protected override ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return mcpQueryToolService.GetToolsAsync(cancellationToken);
    }
}
