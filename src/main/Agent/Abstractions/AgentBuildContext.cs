using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Rag;

namespace NttBank.QueryAgent.Agent.Abstractions;

public sealed record AgentBuildContext(
    string Name,
    string Description,
    string Instructions,
    AgentConfiguration Configuration,
    IList<AITool>? Tools = null,
    RagSearchAdapter? RagAdapter = null);
