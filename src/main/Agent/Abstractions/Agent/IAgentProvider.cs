using Microsoft.Agents.AI;

namespace NttBank.Agent.Agent.Abstractions.Agent;

public interface IAgentProvider
{
    string Name { get; }

    ValueTask<AIAgent> CreateAsync(
        CancellationToken cancellationToken);
}
