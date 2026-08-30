using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Extensions;

namespace NttBank.QueryAgent.Agent.Agents;

public abstract class AgentProviderBase<TConfiguration>
    : IAgentProvider, IDisposable
    where TConfiguration : AgentConfiguration
{
    private readonly IOptionsMonitor<TConfiguration> _options;
    private readonly ILogger _logger;
    private readonly IAgentBuilder _agentBuilder;
    private readonly SemaphoreSlim _buildLock = new(1, 1);
    private readonly IDisposable? _reloadRegistration;

    private volatile AIAgent? _agent;

    private int _generation;

    private bool _disposed;

    protected abstract string Invariants { get; }

    public abstract string Name { get; }

    protected abstract string Description { get; }

    protected virtual IRagSearchRepository? RagRepository => null;

    protected AgentProviderBase(
        ILogger logger,
        IOptionsMonitor<TConfiguration> options,
        IAgentBuilder agentBuilder)
    {
        _logger = logger;
        _options = options;
        _agentBuilder = agentBuilder;

        _reloadRegistration = options.OnChange(_ => Invalidate());
    }

    protected virtual ValueTask<IList<AITool>?> ResolveToolsAsync(
        CancellationToken cancellationToken)
    {
        return ValueTask.FromResult<IList<AITool>?>(null);
    }

    protected virtual AIAgent PostBuild(AIAgent agent) => agent;

    public async ValueTask<AIAgent> CreateAsync(
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

            try
            {
                var built = await BuildAsync(cancellationToken);

                if (Volatile.Read(ref _generation) == generation)
                {
                    _agent = built;
                }
                else
                {
                    _logger.AgentBuildDiscarded(Name);
                }

                return built;
            }
            catch (Exception ex)
            {
                _logger.AgentBuildFailed(ex, Name);
                throw;
            }
        }
        finally
        {
            _buildLock.Release();
        }
    }

    private async Task<AIAgent> BuildAsync(
        CancellationToken cancellationToken)
    {
        var configuration = _options.CurrentValue;

        var tools = await ResolveToolsAsync(cancellationToken);

        _logger.AgentBuilding(
            Name,
            configuration.Provider,
            tools?.Count ?? 0);

        var context = new AgentBuildContext(
            Name,
            Description,
            BuildInstructions(configuration),
            configuration,
            tools,
            RagRepository);

        var agent = _agentBuilder.Build(context);
        var built = PostBuild(agent);

        _logger.AgentBuilt(Name);

        return built;
    }

    private string BuildInstructions(
        TConfiguration configuration) =>
        $"""
        # Persona
        {configuration.Persona}
        # Invariants
        {Invariants}
        """;

    private void Invalidate()
    {
        _logger.AgentInvalidated(Name);
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
