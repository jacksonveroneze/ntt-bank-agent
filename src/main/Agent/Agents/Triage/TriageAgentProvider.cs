using Microsoft.Agents.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Triage;

public sealed class TriageAgentProvider(
    ILogger<TriageAgentProvider> logger,
    IOptionsMonitor<TriageAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    ChatHistoryProvider historyProvider,
    ILoggerFactory loggerFactory,
    IHostEnvironment env)
    : AgentProviderBase<TriageAgentConfiguration>(
            logger, options, chatClientResolver, loggerFactory, env, historyProvider),
        ITriageAgentProvider
{
    public override string Name => "triage";

    protected override string Description =>
        TriageConstants.Description;

    protected override string Invariants =>
        TriageConstants.SystemPrompt;

    public override bool IsSpecialist => false;

    protected override async Task<AIAgent> BuildAsync(
        CancellationToken cancellationToken)
    {
        var agent = await base.BuildAsync(cancellationToken);

        return agent
            .AsBuilder()
            .Use(
                runFunc: InputGuardrailMiddleware.InvokeAsync,
                runStreamingFunc: InputGuardrailMiddleware.InvokeStreamingAsync)
            .Build();
    }
}
