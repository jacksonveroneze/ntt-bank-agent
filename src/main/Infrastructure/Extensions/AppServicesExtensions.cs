using System.Diagnostics.CodeAnalysis;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Cards;
using NttBank.QueryAgent.Agent.Agents.Documents;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Agent.Agents.Triage;
using NttBank.QueryAgent.Agent.Factories;
using NttBank.QueryAgent.Infrastructure.Mappers;
using NttBank.QueryAgent.Infrastructure.Mcp;
using NttBank.QueryAgent.Infrastructure.Rag;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AppServicesExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddSingleton<TypeAdapterConfig>(_ =>
        {
            var config = new TypeAdapterConfig();
            new RagMapper().Register(config);
            return config;
        });

        services.AddSingleton<IMapper, ServiceMapper>();

        services.AddSingleton<IChatClientResolver, ChatClientResolver>();
        services.AddSingleton<IRagSearchAdapter, RagSearchAdapter>();
        services.AddSingleton<IAgentBuilder, AgentBuilder>();
        
        services.AddSingleton<IAgentProvider, TriageAgentProvider>();
        services.AddSingleton<IAgentProvider, QueryAgentProvider>();
        services.AddSingleton<IAgentProvider, CardsAgentProvider>();
        services.AddSingleton<IAgentProvider, DocumentsAgentProvider>();
        
        services.AddSingleton<IMcpQueryToolService, QueryMcpToolService>();
        services.AddSingleton<IMcpCardsToolService, CardsMcpToolService>();
        
        return services;
    }
}
