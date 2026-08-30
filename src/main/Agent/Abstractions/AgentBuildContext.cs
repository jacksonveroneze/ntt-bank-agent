using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public sealed record AgentBuildContext(
    string Name,
    string Description,
    string Instructions,
    AgentConfiguration Configuration,
    IList<AITool>? Tools = null,
    IRagSearchRepository? RagAdapter = null);
