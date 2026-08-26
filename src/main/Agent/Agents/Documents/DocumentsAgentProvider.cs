using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Rag;

namespace NttBank.QueryAgent.Agent.Agents.Documents;

public sealed class DocumentsAgentProvider(
    ILogger<DocumentsAgentProvider> logger,
    IOptionsMonitor<DocumentsAgentConfiguration> options,
    IChatClientResolver chatClientResolver,
    ChatHistoryProvider historyProvider,
    ILoggerFactory loggerFactory,
    IHostEnvironment env,
    RagSearchAdapter ragSearchAdapter)
    : AgentProviderBase<DocumentsAgentConfiguration>(
            logger, options, chatClientResolver, 
            loggerFactory, env, historyProvider, ragSearchAdapter),
        IDocumentsAgentProvider
{
    public override string Name => "documents";

    protected override string Description =>
        DocumentsConstants.Description;

    protected override string Invariants =>
        DocumentsConstants.SystemPrompt;

    public override bool IsSpecialist => true;
}
