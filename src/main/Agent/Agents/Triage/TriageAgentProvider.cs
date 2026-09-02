using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Agents.Common;

namespace NttBank.Agent.Agent.Agents.Triage;

public sealed class TriageAgentProvider(
    ILogger<TriageAgentProvider> logger,
    IOptionsMonitor<TriageAgentConfiguration> options,
    IAgentBuilder agentBuilder)
    : AgentProviderBase<TriageAgentConfiguration>(
        logger, options, agentBuilder), ITriageAgentProvider
{
    public override string Name => "triage";

    protected override string Description =>
        TriageConstants.Description;

    protected override string Invariants =>
        TriageConstants.SystemPrompt;

    protected override AIAgent PostBuild(AIAgent agent)
    {
        return agent.AsBuilder()
            .Use(
                runFunc: InputGuardrailMiddleware.InvokeAsync,
                runStreamingFunc: InputGuardrailMiddleware.InvokeStreamingAsync)
            .Build();
    }
}
