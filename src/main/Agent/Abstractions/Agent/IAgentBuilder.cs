using Microsoft.Agents.AI;

namespace NttBank.Agent.Agent.Abstractions.Agent;

public interface IAgentBuilder
{
    AIAgent Build(AgentBuildContext context);
}
