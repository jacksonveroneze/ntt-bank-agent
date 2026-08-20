using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Agents.Query.Instructions;
using NttBank.QueryAgent.Agent.Configurations;
using NttBank.QueryAgent.Agent.Factories;
using NttBank.QueryAgent.Agent.Services.Mcp;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgentProvider(
    IChatClient chatClient,
    QueryAgentConfiguration configuration,
    IHostEnvironment env,
    IMcpToolService mcpToolService) : IQueryAgentProvider, IDisposable
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

            _agent = await BuildAgentAsync(
                chatClient, configuration, env, cancellationToken);

            return _agent;
        }
        finally
        {
            _buildLock.Release();
        }
    }

    private async Task<AIAgent> BuildAgentAsync(
        IChatClient chatClientAgent,
        QueryAgentConfiguration configurationAgent,
        IHostEnvironment envAgent,
        CancellationToken cancellationToken)
    {
        var mcpTools = await mcpToolService
            .GetToolsAsync(cancellationToken);
        
        var agent = ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClientAgent,
                Name = configurationAgent.Name,
                Description = configurationAgent.Description,
                ModelId = configurationAgent.Model,
                Instructions = configurationAgent.SystemPrompt
                               ?? SystemPromptInstructions.SystemPrompt,
                Temperature = configurationAgent.Temperature,
                Tools = mcpTools,
                EnableSensitiveData = envAgent.IsDevelopment(),
                AllowMultipleToolCalls = configurationAgent.AllowMultipleToolCalls,
            });

        return agent
            .AsBuilder()
            .Build();
    }

    public void Dispose()
    {
        chatClient.Dispose();
        _buildLock.Dispose();
    }
}
