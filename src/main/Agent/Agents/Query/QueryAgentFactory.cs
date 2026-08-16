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
    internal static AIAgent Build(
        IChatClient chatClient, 
        QueryAgentConfiguration configuration,
        IHostEnvironment env, 
        IList<AITool> tools)
    {
        var agent = ChatClientAgentFactory.Create(
            new ChatAgentDescriptor
            {
                ChatClient = chatClient,
                Name = configuration.Name,
                Description = configuration.Description,
                ModelId = configuration.Model,
                Instructions = SystemPromptInstructions.SystemPrompt,
                Temperature = configuration.Temperature,
                Tools = tools,
                EnableSensitiveData = env.IsDevelopment(),
                AllowMultipleToolCalls = true,
            });

        return agent
            .AsBuilder()
            .Use(inner => new GuardrailChatClient(inner))
            .Build();
    }
}
