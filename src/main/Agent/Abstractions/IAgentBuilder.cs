using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IAgentBuilder
{
    AIAgent Build(AgentBuildContext context);
}
