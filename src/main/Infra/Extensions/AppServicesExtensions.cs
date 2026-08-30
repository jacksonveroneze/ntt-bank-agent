using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Cards;
using NttBank.QueryAgent.Agent.Agents.Documents;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Agent.Agents.Triage;
using NttBank.QueryAgent.Agent.Factories;
using NttBank.QueryAgent.Agent.Rag;

namespace NttBank.QueryAgent.Infra.Extensions;

[ExcludeFromCodeCoverage]
public static class AppServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        //services.AddSingleton<IQueryAgentProvider, QueryAgentProvider>();
        // services.AddSingleton<IQueryAgent, Agent.Agents.Query.QueryAgent>();
        
        //services.AddSingleton<ICardsAgentProvider, CardsAgentProvider>();
        // services.AddSingleton<ICardsAgent, Agent.Agents.Cards.CardsAgent>();
        
        services.AddSingleton<ISessionStore, CacheSessionStore>();
        services.AddSingleton<IChatClientResolver, ChatClientResolver>();
        services.AddSingleton<RagSearchAdapter>();
        services.AddSingleton<IAgentBuilder, AgentBuilder>();
        
        services.AddSingleton<IAgentProvider, TriageAgentProvider>();
        services.AddSingleton<IAgentProvider, QueryAgentProvider>();
        services.AddSingleton<IAgentProvider, CardsAgentProvider>();
        services.AddSingleton<IAgentProvider, DocumentsAgentProvider>();
        
        return services;
    }
}
