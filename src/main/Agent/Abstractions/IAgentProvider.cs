using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IAgentProvider
{
    string Name { get; }

    ValueTask<AIAgent> CreateAsync(
        CancellationToken cancellationToken);
}
