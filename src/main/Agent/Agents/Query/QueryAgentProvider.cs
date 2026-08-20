using Microsoft.Agents.AI;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Query.Instructions;
using NttBank.QueryAgent.Agent.Configurations;
using NttBank.QueryAgent.Agent.Factories;
using NttBank.QueryAgent.Agent.Services.Mcp;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgentProvider(
    IChatClientResolver chatClientResolver,
    QueryAgentConfiguration configuration,
    IHostEnvironment env,
    IMcpQueryToolService mcpQueryToolService) : IQueryAgentProvider, IDisposable
{
    private readonly SemaphoreSlim _buildLock = new(1, 1);
    private AIAgent? _agent;

    public async ValueTask<AIAgent> GetAsync(
        CancellationToken cancellationToken)
    {
        if (_agent is not null)
        {
            return _agent;
        }

        await _buildLock.WaitAsync(cancellationToken);

        try
        {
            if (_agent is not null)
            {
                return _agent;
            }

            _agent = await BuildAgentAsync(cancellationToken);

            return _agent;
        }
        finally
        {
            _buildLock.Release();
        }
    }

    private async Task<AIAgent> BuildAgentAsync(
        CancellationToken cancellationToken)
    {
        var chatClientAgent = chatClientResolver
            .Resolve(configuration.Provider);

        var mcpTools = await mcpQueryToolService
            .GetToolsAsync(cancellationToken);

        var agent = ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClientAgent,
                Name = configuration.Name,
                Description = configuration.Description,
                ModelId = configuration.Model,
                Instructions = configuration.SystemPrompt
                               ?? SystemPromptInstructions.SystemPrompt,
                Temperature = configuration.Temperature,
                Tools = mcpTools,
                EnableSensitiveData = env.IsDevelopment(),
                AllowMultipleToolCalls = configuration.AllowMultipleToolCalls,
            });

        return agent
            .AsBuilder()
            .Build();
    }

    public void Dispose()
    {
        _buildLock.Dispose();
    }
}
