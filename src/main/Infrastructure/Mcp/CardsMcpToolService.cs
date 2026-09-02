using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Agents.Cards;
using NttBank.Agent.Infrastructure.Configurations;

namespace NttBank.Agent.Infrastructure.Mcp;

public sealed class CardsMcpToolService(
    IOptions<McpCardsConfiguration> options,
    IHttpClientFactory httpClientFactory,
    ILoggerFactory loggerFactory,
    ILogger<CardsMcpToolService> logger)
    : McpToolService(
        options.Value,
        httpClientFactory,
        loggerFactory,
        logger),
      IMcpCardsToolService;
