using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Agents.Query.Instructions;
using NttBank.QueryAgent.Agent.Agents.Query.Middleware;
using NttBank.QueryAgent.Agent.Factories;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal sealed class QueryAgentFactory(
    IChatClient chatClient,
    string modelId,
    float temperature,
    IHostEnvironment hostEnvironment)
{
    private const string AgentName = "query-agent";
    private const string AgentDescription = "Agent de cálculos";

    internal AIAgent Build()
    {
        var chatClientAgent = ChatClientAgentFactory.Create(
            chatClient: chatClient,
            name: AgentName,
            description: AgentDescription,
            modelId: modelId,
            temperature: temperature,
            instructions: SystemPromptInstructions.SystemPrompt);

        var agent = chatClientAgent
            .AsBuilder()
            .UseOpenTelemetry(
                sourceName: AgentName,
                configure: cfg => { cfg.EnableSensitiveData = hostEnvironment.IsDevelopment(); })
            .Use(runFunc: AgentGuardrails.ValidateAgentRunAsync, runStreamingFunc: null)
            .Build();

        return agent;
    }
}
