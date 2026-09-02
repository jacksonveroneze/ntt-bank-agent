using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Agents.Query;
using NttBank.Agent.Infrastructure.Configurations;

namespace NttBank.Agent.Infrastructure.Mcp;

public sealed class QueryMcpToolService(
    IOptions<McpQueryConfiguration> options,
    IHttpClientFactory httpClientFactory,
    ILoggerFactory loggerFactory,
    ILogger<QueryMcpToolService> logger)
    : McpToolService(
        options.Value,
        httpClientFactory,
        loggerFactory,
        logger),
      IMcpQueryToolService;
