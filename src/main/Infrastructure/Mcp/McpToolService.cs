using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Infrastructure.Configurations;
using NttBank.QueryAgent.Infrastructure.Extensions;

namespace NttBank.QueryAgent.Infrastructure.Mcp;

public abstract class McpToolService(
    McpServerConfiguration configuration,
    IHttpClientFactory httpClientFactory,
    ILoggerFactory loggerFactory,
    ILogger logger)
    : IMcpToolService, IAsyncDisposable
{
    private static readonly TimeSpan DiscoverProbeTimeout
        = TimeSpan.FromSeconds(5);

    private readonly SemaphoreSlim _connectLock = new(1, 1);

    private McpClient? _client;
    private IClientTransport? _transport;
    private IList<AITool>? _tools;
    private bool _disposed;

    public async ValueTask<IList<AITool>?> GetToolsAsync(
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

            logger.McpConnected(configuration.Name, _tools.Count);

            return _tools;
        }
        catch (Exception ex)
        {
            logger.McpConnectionFailed(configuration.Name, ex);
            throw;
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

        var httpClient = httpClientFactory.CreateClient(
            configuration.Name);

        _transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = configuration.Address,
                TransportMode = HttpTransportMode.AutoDetect,
            },
            httpClient);

        _client = await McpClient.CreateAsync(
            _transport,
            new McpClientOptions
            {
                DiscoverProbeTimeout = DiscoverProbeTimeout,
            },
            loggerFactory,
            cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        GC.SuppressFinalize(this);

        await _connectLock.WaitAsync();

        try
        {
            if (_client is not null)
            {
                await _client.DisposeAsync();
                _client = null;
            }

            (_transport as IDisposable)?.Dispose();
            _transport = null;

            _tools = [];
        }
        finally
        {
            _connectLock.Dispose();
        }
    }
}
