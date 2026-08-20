using Microsoft.Agents.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Factories;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

internal sealed class CardsAgentProvider : ICardsAgentProvider, IDisposable
{
    private readonly ILogger<CardsAgentProvider> _logger;
    private readonly IOptionsMonitor<CardsAgentConfiguration> _options;
    private readonly IChatClientResolver _chatClientResolver;
    private readonly IHostEnvironment _env;

    private readonly SemaphoreSlim _buildLock = new(1, 1);
    private readonly IDisposable? _reloadRegistration;

    private volatile AIAgent? _agent;
    private int _generation;

    public CardsAgentProvider(
        ILogger<CardsAgentProvider> logger,
        IOptionsMonitor<CardsAgentConfiguration> options,
        IChatClientResolver chatClientResolver,
        IHostEnvironment env
    )
    {
        _chatClientResolver = chatClientResolver;
        _options = options;
        _env = env;
        _logger = logger;

        _reloadRegistration = options.OnChange((_, name) =>
        {
            if (name is AgentNames.Cards)
            {
                return;
            }

            Interlocked.Increment(ref _generation);
            _agent = null;

            _logger.LogInformation(
                "• Configuração do Cards-agent alterada — agente será reconstruído.");
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

            var built = BuildAgent();

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

    private AIAgent BuildAgent()
    {
        var configuration = _options.CurrentValue;

        var chatClient = _chatClientResolver
            .Resolve(configuration.Provider);


        _logger.LogInformation(
            "• Construindo Cards-agent — provider: {Provider}",
            configuration.Provider);

        return ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClient,
                Name = configuration.Name,
                Description = configuration.Description,
                ModelId = configuration.Model,
                Instructions = configuration.SystemPrompt,
                Temperature = configuration.Temperature,
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
