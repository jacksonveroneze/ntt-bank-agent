using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

public sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    ILoggerFactory loggerFactory,
    IHostEnvironment env)
    : AgentProviderBase<CardsAgentConfiguration>(
            logger, options, chatClientResolver, loggerFactory, env),
        ICardsAgentProvider
{
    public override string Name => "cards";

    protected override string Description => 
        CardsConstants.Description;
    
    protected override string Invariants =>
        CardsConstants.SystemPrompt;
    
    public override bool IsSpecialist => true;
}
