using NttBank.QueryAgent.Agent.Agents.Query.Models;
using NttBank.QueryAgent.Api.Endpoints.Agents.Common.Models;
using NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1.Models;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1;

internal static class ResponseMapper
{
    internal static QueryAgentResponse ToHttpResponse(
        this AgentOutput agentOutput,
        IHostEnvironment hostEnvironment)
    {
        AgentDebugResponse? debugResponse = hostEnvironment.IsDevelopment()
            ? new AgentDebugResponse(
                MessageCount: agentOutput.MessageCount,
                Messages: agentOutput.Messages,
                RawText: agentOutput.RawText)
            : null;

        return new QueryAgentResponse
        {
            Message = agentOutput.Message,
            Debug = debugResponse,
        };
    }
}
