using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Agents.Query.Instructions;
using NttBank.QueryAgent.Agent.Agents.Query.Middleware;
using NttBank.QueryAgent.Agent.Configurations;
using NttBank.QueryAgent.Agent.Factories;

namespace NttBank.QueryAgent.Agent.Agents.Query;

internal static class QueryAgentFactory
{
    private const string Name = "query-agent";
    private const string Description = "Banking query agent (read-only).";

    internal static AIAgent Build(
        IChatClient chatClient, 
        QueryAgentOptions options,
        IHostEnvironment env, 
        IList<AITool> tools)
    {
        var agent = ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClient,
                Name = Name,
                Description = Description,
                ModelId = options.Model,
                Instructions = SystemPromptInstructions.SystemPrompt,
                Temperature = options.Temperature,
                Tools = tools,
                EnableSensitiveData = env.IsDevelopment(),
            });

        return agent
            .AsBuilder()
            .Use(AgentGuardrails.ValidateAgentRunAsync, runStreamingFunc: null)
            .Build();
    }
}
