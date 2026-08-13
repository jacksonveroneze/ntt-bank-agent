using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Configurations;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgentProvider(
    IChatClient chatClient,
    QueryAgentOptions options,
    IHostEnvironment env,
    IMcpToolService toolService) : IQueryAgentProvider, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private AIAgent? _agent;

    public async ValueTask<AIAgent> GetAsync(
        CancellationToken cancellationToken)
    {
        if (_agent is not null)
        {
            return _agent;
        }

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            if (_agent is not null)
            {
                return _agent;
            }

            var tools = await toolService
                .GetToolsAsync(cancellationToken);

            _agent = QueryAgentFactory.Build(
                chatClient, options, env, tools);

            return _agent;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
        chatClient.Dispose();
    }
}
