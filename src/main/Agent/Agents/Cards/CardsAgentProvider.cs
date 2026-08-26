using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Rag;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

public sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    ChatHistoryProvider historyProvider,
    ILoggerFactory loggerFactory,
    IHostEnvironment env,
    IMcpQueryToolService mcpQueryToolService)
    : AgentProviderBase<CardsAgentConfiguration>(
            logger, options, chatClientResolver, loggerFactory, env, historyProvider),
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
