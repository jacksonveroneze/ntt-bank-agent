using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IAgentProvider
{
    ValueTask<AIAgent> GetAsync(
        CancellationToken cancellationToken);
}
