using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

internal sealed class CardsAgentProvider(
    ILogger<CardsAgentProvider> logger,
    IOptionsMonitor<CardsAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    IHostEnvironment env)
    : AgentProviderBase<CardsAgentConfiguration>(logger, options, chatClientResolver, env),
        ICardsAgentProvider;
