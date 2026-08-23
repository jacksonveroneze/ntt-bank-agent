using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

public sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    IHostEnvironment env)
    : AgentProviderBase<CardsAgentConfiguration>(logger, options, chatClientResolver, env),
        ICardsAgentProvider
{
    public override string Name => "cards";
    
    public override string Description =>
        "Consultas bancárias somente-leitura: cartões.";
    
    protected override string Invariants => QueryInstructions.SystemPrompt;
}
