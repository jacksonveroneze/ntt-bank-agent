using Microsoft.Agents.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Factories;
using NttBank.QueryAgent.Agent.Services.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgentProvider : IQueryAgentProvider, IDisposable
{
    private readonly ILogger<QueryAgentProvider> _logger;
    private readonly IOptionsMonitor<QueryAgentConfiguration> _options;
    private readonly IChatClientResolver _chatClientResolver;
    private readonly IHostEnvironment _env;
    private readonly IMcpQueryToolService _mcpQueryToolService;

    private readonly SemaphoreSlim _buildLock = new(1, 1);
    private readonly IDisposable? _reloadRegistration;

    private volatile AIAgent? _agent;
    private int _generation;

    public QueryAgentProvider(
        ILogger<QueryAgentProvider> logger,
        IOptionsMonitor<QueryAgentConfiguration> options,
        IChatClientResolver chatClientResolver,
        IHostEnvironment env,
        IMcpQueryToolService mcpQueryToolService
    )
    {
        _chatClientResolver = chatClientResolver;
        _options = options;
        _env = env;
        _mcpQueryToolService = mcpQueryToolService;
        _logger = logger;

        _reloadRegistration = options.OnChange((_, name) =>
        {
            if (name is AgentNames.Query)
            {
                return;
            }

            Interlocked.Increment(ref _generation);
            _agent = null;

            _logger.LogInformation(
                "• Configuração do query-agent alterada — agente será reconstruído.");
        });
    }

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

            var generation = Volatile.Read(ref _generation);

            var built = await BuildAgentAsync(cancellationToken);

            if (Volatile.Read(ref _generation) == generation)
            {
                _agent = built;
            }

            return built;
        }
        finally
        {
            _buildLock.Release();
        }
    }

    private async Task<AIAgent> BuildAgentAsync(
        CancellationToken cancellationToken)
    {
        var configuration = _options.CurrentValue;

        var chatClient = _chatClientResolver
            .Resolve(configuration.Provider);

        var tools = await _mcpQueryToolService
            .GetToolsAsync(cancellationToken);

        _logger.LogInformation(
            "• Construindo query-agent — provider: {Provider}, tools: {ToolCount}.",
            configuration.Provider, tools.Count);

        return ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClient,
                Name = configuration.Name,
                Description = configuration.Description,
                ModelId = configuration.Model,
                Instructions = configuration.SystemPrompt,
                Temperature = configuration.Temperature,
                Tools = tools,
                EnableSensitiveData = _env.IsDevelopment(),
                AllowMultipleToolCalls = configuration.AllowMultipleToolCalls,
            });
    }

    public void Dispose()
    {
        _reloadRegistration?.Dispose();
        _buildLock.Dispose();
    }
}
