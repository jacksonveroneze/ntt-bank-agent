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
    ILoggerFactory loggerFactory,
    IHostEnvironment env,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<QueryAgentConfiguration>(
            logger, options, chatClientResolver, loggerFactory, env),
        IQueryAgentProvider
{
    public override string Name => "query";

    protected override string Description => 
        QueryConstants.Description;
    
    protected override string Invariants =>
        QueryConstants.SystemPrompt;
    
    public override bool IsSpecialist => false;

    protected override ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return mcpQueryToolService.GetToolsAsync(cancellationToken);
    }
}
