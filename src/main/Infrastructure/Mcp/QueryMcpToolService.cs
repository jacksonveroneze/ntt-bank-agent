using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Infrastructure.Mcp;

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
