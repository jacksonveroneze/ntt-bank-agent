using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

internal interface IQueryAgentProvider
{
    ValueTask<AIAgent> GetAsync(CancellationToken cancellationToken);
}
