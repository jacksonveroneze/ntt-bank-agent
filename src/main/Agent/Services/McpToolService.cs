using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

namespace NttBank.QueryAgent.Agent.Services;

internal sealed class McpToolService(
    IClientTransport transport) : IMcpToolService, IAsyncDisposable
{
    private readonly SemaphoreSlim _gate = new(1, 1);

    private McpClient? _client;

    private IList<AITool>? _tools;

    public async ValueTask<IList<AITool>> GetToolsAsync(
        CancellationToken cancellationToken)
    {
        if (_tools is not null)
        {
            return _tools;
        }

        await _gate.WaitAsync(cancellationToken);

        try
        {
            if (_tools is not null)
            {
                return _tools;
            }

            _client = await McpClient.CreateAsync(
                transport, cancellationToken: cancellationToken);

            var tools = await _client.ListToolsAsync(
                cancellationToken: cancellationToken);

            _tools = [.. tools];

            return _tools;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_client is not null)
        {
            await _client.DisposeAsync();
        }

        _gate.Dispose();
    }
}
