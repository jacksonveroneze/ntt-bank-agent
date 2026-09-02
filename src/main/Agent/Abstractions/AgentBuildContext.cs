using Microsoft.Extensions.AI;

namespace NttBank.Agent.Agent.Abstractions;

public sealed record AgentBuildContext(
    string Name,
    string Description,
    string Instructions,
    AgentConfiguration Configuration,
    IList<AITool>? Tools = null,
    IRagSearchRepository? RagAdapter = null);
