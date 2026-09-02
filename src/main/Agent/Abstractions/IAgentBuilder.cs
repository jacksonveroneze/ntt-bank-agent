using Microsoft.Agents.AI;

namespace NttBank.Agent.Agent.Abstractions;

public interface IAgentBuilder
{
    AIAgent Build(AgentBuildContext context);
}
