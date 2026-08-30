using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Agents.Cards;
using NttBank.QueryAgent.Infrastructure.Configurations.Mcp;

namespace NttBank.QueryAgent.Infrastructure.Mcp;

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
