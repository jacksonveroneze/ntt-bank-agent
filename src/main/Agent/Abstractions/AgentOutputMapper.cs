using Microsoft.Agents.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

internal static class AgentOutputMapper
{
    public static AgentOutput ToOutput(
        AgentResponse response)
    {
        return new AgentOutput(
            Message: response.Text,
            response);
    }
}
