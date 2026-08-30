using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Rag;

namespace NttBank.QueryAgent.Agent.Agents.Documents;

public sealed class DocumentsAgentProvider(
    ILogger<DocumentsAgentProvider> logger,
    IOptionsMonitor<DocumentsAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    RagSearchAdapter ragSearchAdapter)
    : AgentProviderBase<DocumentsAgentConfiguration>(
            logger, options, agentBuilder),
        IDocumentsAgentProvider
{
    public override string Name => "documents";

    protected override string Description =>
        DocumentsConstants.Description;

    protected override string Invariants =>
        DocumentsConstants.SystemPrompt;

    public override bool IsSpecialist => true;

    protected override RagSearchAdapter RagAdapter =>
        ragSearchAdapter;
}
