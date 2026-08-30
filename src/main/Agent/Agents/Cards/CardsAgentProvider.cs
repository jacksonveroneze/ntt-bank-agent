using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

public sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<CardsAgentConfiguration>(
            logger, options, agentBuilder),
        ICardsAgentProvider
{
    public override string Name => "cards";

    protected override string Description =>
        CardsConstants.Description;

    protected override string Invariants =>
        CardsConstants.SystemPrompt;

    public override bool IsSpecialist => true;

    protected override ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return mcpQueryToolService.GetToolsAsync(cancellationToken);
    }
}
