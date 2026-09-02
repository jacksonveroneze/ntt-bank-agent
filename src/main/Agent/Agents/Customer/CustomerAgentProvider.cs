using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Abstractions.Mcp;
using NttBank.Agent.Agent.Agents.Common;

namespace NttBank.Agent.Agent.Agents.Customer;

public sealed class CustomerAgentProvider(
    ILogger<CustomerAgentProvider> logger,
    IOptionsMonitor<CustomerAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<CustomerAgentConfiguration>(
            logger, options, agentBuilder),
        ICustomerAgentProvider
{
    public override string Name => "customer";

    protected override string Description =>
        CustomerConstants.Description;

    protected override string Invariants =>
        CustomerConstants.SystemPrompt;

    protected override async ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        var tools = await mcpQueryToolService.GetToolsAsync(cancellationToken);

        return tools?
            .Where(tool => CustomerConstants.AllowedTools.Contains(tool.Name))
            .ToList();
    }
}
