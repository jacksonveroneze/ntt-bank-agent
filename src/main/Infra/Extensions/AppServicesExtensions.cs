using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Cards;
using NttBank.QueryAgent.Agent.Agents.Query;

namespace NttBank.QueryAgent.Infra.Extensions;

[ExcludeFromCodeCoverage]
public static class AppServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IQueryAgentProvider, QueryAgentProvider>();
        services.AddSingleton<IQueryAgent, Agent.Agents.Query.QueryAgent>();
        
        services.AddSingleton<ICardsAgentProvider, CardsAgentProvider>();
        services.AddSingleton<ICardsAgent, CardsAgent>();
        
        services.AddSingleton<ISessionStore, CacheSessionStore>();
        services.AddSingleton<IChatClientResolver, ChatClientResolver>();
        
        return services;
    }
}
