using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Triage;

public sealed class TriageAgentProvider(
    ILogger<TriageAgentProvider> logger,
    IOptionsMonitor<TriageAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    ILoggerFactory loggerFactory,
    IHostEnvironment env) 
    : AgentProviderBase<TriageAgentConfiguration>(
            logger, options, chatClientResolver, loggerFactory, env),
        ITriageAgentProvider
{
    public override string Name => "triage";

    protected override string Description => 
        TriageConstants.Description;
    
    protected override string Invariants =>
        TriageConstants.SystemPrompt;
    
    public override bool IsSpecialist => false;
}
