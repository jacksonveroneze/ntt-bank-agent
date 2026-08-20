using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace NttBank.QueryAgent.Agent.Services.Mcp;

internal sealed class QueryMcpToolService(
    ILogger<QueryMcpToolService> logger,
    ILoggerFactory loggerFactory,
    IClientTransport transport) : IMcpQueryToolService, IAsyncDisposable
{
    private static readonly TimeSpan DiscoverProbeTimeout
        = TimeSpan.FromSeconds(5);

    private readonly SemaphoreSlim _connectLock = new(1, 1);

    private McpClient? _client;

    private IList<AITool>? _tools;

    public async ValueTask<IList<AITool>> GetToolsAsync(
        CancellationToken cancellationToken)
    {
        if (_tools is not null)
        {
            return _tools;
        }

        await _connectLock.WaitAsync(cancellationToken);

        try
        {
            await CreateMcpClientAsync(cancellationToken);

            var tools = await _client!.ListToolsAsync(
                cancellationToken: cancellationToken);

            _tools = [.. tools];

            logger.LogInformation("Connected to MCP Server - {ToolCount} tools available",
                _tools.Count);

            return _tools;
        }
        finally
        {
            _connectLock.Release();
        }
    }

    private async Task CreateMcpClientAsync(
        CancellationToken cancellationToken)
    {
        if (_client is not null)
        {
            return;
        }

        _client = await McpClient.CreateAsync(
            transport,
            new McpClientOptions
            {
                DiscoverProbeTimeout = DiscoverProbeTimeout,
            },
            loggerFactory: loggerFactory,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _connectLock.WaitAsync();

        try
        {
            if (_client is not null)
            {
                await _client.DisposeAsync();
                _client = null;
            }

            _tools = [];
        }
        finally
        {
            _connectLock.Dispose();
        }
    }
}
