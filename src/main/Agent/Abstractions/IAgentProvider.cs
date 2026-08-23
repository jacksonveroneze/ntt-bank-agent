using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IAgentProvider
{
    bool IsSpecialist { get; }
    
    string Name { get; }

    ValueTask<AIAgent> GetAsync(
        CancellationToken cancellationToken);
}
