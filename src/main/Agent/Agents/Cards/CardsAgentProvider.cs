using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Agents.Common;

namespace NttBank.Agent.Agent.Agents.Cards;

public sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IMcpCardsToolService mcpCardsToolService)
    : AgentProviderBase<CardsAgentConfiguration>(
            logger, options, agentBuilder),
        ICardsAgentProvider
{
    public override string Name => "cards";

    protected override string Description =>
        CardsConstants.Description;

    protected override string Invariants =>
        CardsConstants.SystemPrompt;

    protected override ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return mcpCardsToolService.GetToolsAsync(cancellationToken);
    }
}
