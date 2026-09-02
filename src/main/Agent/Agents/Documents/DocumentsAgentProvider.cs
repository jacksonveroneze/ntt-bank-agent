using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Abstractions.Rag;
using NttBank.Agent.Agent.Agents.Common;

namespace NttBank.Agent.Agent.Agents.Documents;

public sealed class DocumentsAgentProvider(
    ILogger<DocumentsAgentProvider> logger,
    IOptionsMonitor<DocumentsAgentConfiguration> options,
    IAgentBuilder agentBuilder,
    IRagSearchRepository ragSearchRepository)
    : AgentProviderBase<DocumentsAgentConfiguration>(
            logger, options, agentBuilder),
        IDocumentsAgentProvider
{
    public override string Name => "documents";

    protected override string Description =>
        DocumentsConstants.Description;

    protected override string Invariants =>
        DocumentsConstants.SystemPrompt;

    protected override IRagSearchRepository RagRepository =>
        ragSearchRepository;
}
