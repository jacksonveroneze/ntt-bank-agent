using System.Collections.Immutable;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Query.Models;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgent(
    AIAgent agent) : IQueryAgent
{
    public async Task<AgentOutput> RunAsync(
        AgentInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        AgentResponse response = await agent
            .RunAsync(input.Prompt,
                session: input.Session,
                options: new AgentRunOptions
                {
                    ResponseFormat = new ChatResponseFormatText(),
                },
                cancellationToken: cancellationToken);

        var result = string.IsNullOrWhiteSpace(response.Text)
            ? DefaultAgentOutputMessages.SafeRefusalMessage
            : response.Text;

        var messages = response.Messages
            .SelectMany(item => item.Contents
                .Select(content => content.ToString()))
            .ToImmutableArray();

        return new AgentOutput(
            Message: result,
            MessageCount: response.Messages.Count,
            Messages: messages,
            RawText: response.Text);
    }
}
