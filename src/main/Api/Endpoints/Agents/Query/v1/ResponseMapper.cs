using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1.Models;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1;

internal static class ResponseMapper
{
    internal static QueryAgentResponse ToHttpResponse(
        this AgentOutput agentOutput,
        IHostEnvironment hostEnvironment)
    {
        return new QueryAgentResponse
        {
            Message = agentOutput.Message,
            Debug = hostEnvironment.IsDevelopment()
                ? agentOutput.AgentResponse
                : null,
        };
    }
}
