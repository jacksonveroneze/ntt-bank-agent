using Microsoft.Extensions.AI;
using NttBank.Agent.Agent.Abstractions.Rag;

namespace NttBank.Agent.Agent.Abstractions.Agent;

public sealed record AgentBuildContext(
    string Name,
    string Description,
    string Instructions,
    AgentConfiguration Configuration,
    IList<AITool>? Tools = null,
    IRagSearchRepository? RagAdapter = null);
