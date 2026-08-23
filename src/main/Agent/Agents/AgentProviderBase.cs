using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Factories;

namespace NttBank.QueryAgent.Agent.Agents;

public abstract class AgentProviderBase<TConfiguration> : IAgentProvider, IDisposable
    where TConfiguration : AgentConfiguration
{
    private readonly IChatClientResolver _chatClientResolver;
    private readonly IOptionsMonitor<TConfiguration> _options;
    private readonly IHostEnvironment _env;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

    private readonly SemaphoreSlim _buildLock = new(1, 1);
    private readonly IDisposable? _reloadRegistration;

    private volatile AIAgent? _agent;

    protected abstract string Invariants { get; }

    private int _generation;

    private bool _disposed;

    public abstract string Name { get; }

    protected abstract string Description { get; }

    public abstract bool IsSpecialist { get; }

    protected AgentProviderBase(
        ILogger logger,
        IOptionsMonitor<TConfiguration> options,
        IChatClientResolver chatClientResolver,
        ILoggerFactory loggerFactory,
        IHostEnvironment env)
    {
        _logger = logger;
        _options = options;
        _chatClientResolver = chatClientResolver;
        _env = env;
        _loggerFactory = loggerFactory;

        _reloadRegistration = options.OnChange(_ => Invalidate());
    }

    protected virtual ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return ValueTask.FromResult<IList<AITool>?>(null);
    }

    public async ValueTask<AIAgent> GetAsync(
        CancellationToken cancellationToken)
    {
        var cached = _agent;

        if (cached is not null)
        {
            return cached;
        }

        await _buildLock.WaitAsync(cancellationToken);

        try
        {
            if (_agent is not null)
            {
                return _agent;
            }

            var generation = Volatile.Read(ref _generation);
            var built = await BuildAsync(cancellationToken);

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

    protected virtual async Task<AIAgent> BuildAsync(
        CancellationToken cancellationToken)
    {
        var configuration = _options.CurrentValue;

        var chatClient = _chatClientResolver.Resolve(
            configuration.Provider);

        var tools = await ResolveToolsAsync(cancellationToken);

        var instructions = BuildInstructions(configuration);

        _logger.LogInformation(
            "• Construindo agente {Agent} — provider: {Provider}, tools: {ToolCount}.",
            Name, configuration.Provider, tools?.Count ?? 0);

        return ChatClientAgentFactory.Create(
            Name, Description, instructions, chatClient,
            configuration, _loggerFactory, _env.IsDevelopment(), tools);
    }

    private string BuildInstructions(
        TConfiguration configuration)
    {
        var persona = configuration.Persona;

        var instructions = $"""
                            # Persona
                            {persona}

                            {Invariants}
                            """;

        return instructions;
    }

    private void Invalidate()
    {
        Interlocked.Increment(ref _generation);
        _agent = null;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _reloadRegistration?.Dispose();
            _buildLock.Dispose();
        }

        _disposed = true;
    }
}
