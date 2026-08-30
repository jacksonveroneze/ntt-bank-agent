using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Triage;

public sealed class TriageAgentProvider(
    ILogger<TriageAgentProvider> logger,
    IOptionsMonitor<TriageAgentConfiguration> options,
    IAgentBuilder agentBuilder)
    : AgentProviderBase<TriageAgentConfiguration>(
            logger, options, agentBuilder),
        ITriageAgentProvider
{
    public override string Name => "triage";

    protected override string Description =>
        TriageConstants.Description;

    protected override string Invariants =>
        TriageConstants.SystemPrompt;

    public override bool IsSpecialist => false;

    protected override AIAgent PostBuild(AIAgent agent) =>
        agent.AsBuilder()
             .Use(
                 runFunc: InputGuardrailMiddleware.InvokeAsync,
                 runStreamingFunc: InputGuardrailMiddleware.InvokeStreamingAsync)
             .Build();
}
