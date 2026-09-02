using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Abstractions.Mcp;
using NttBank.Agent.Agent.Agents.Common;

namespace NttBank.Agent.Agent.Agents.Accounts;

public sealed class AccountsAgentProvider(
    ILogger<AccountsAgentProvider> logger,
    IOptionsMonitor<AccountsAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<AccountsAgentConfiguration>(
            logger, options, agentBuilder),
        IAccountsAgentProvider
{
    public override string Name => "accounts";

    protected override string Description =>
        AccountsConstants.Description;

    protected override string Invariants =>
        AccountsConstants.SystemPrompt;

    protected override async ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        var tools = await mcpQueryToolService.GetToolsAsync(cancellationToken);

        return tools?
            .Where(tool => AccountsConstants.AllowedTools.Contains(tool.Name))
            .ToList();
    }
}
