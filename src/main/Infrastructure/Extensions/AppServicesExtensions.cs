using System.Diagnostics.CodeAnalysis;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using NttBank.Agent.Agent.Abstractions;
using NttBank.Agent.Agent.Agents.Cards;
using NttBank.Agent.Agent.Agents.Documents;
using NttBank.Agent.Agent.Agents.Query;
using NttBank.Agent.Agent.Agents.Triage;
using NttBank.Agent.Agent.Factories;
using NttBank.Agent.Infrastructure.Mappers;
using NttBank.Agent.Infrastructure.Mcp;
using NttBank.Agent.Infrastructure.Repositories;

namespace NttBank.Agent.Infrastructure.Extensions;

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
        services.AddSingleton<IRagSearchRepository, RagSearchRepository>();
        services.AddSingleton<IAgentBuilder, AgentBuilder>();
        
        services.AddSingleton<ITriageAgentProvider, TriageAgentProvider>();
        services.AddSingleton<ISpecialistAgentProvider, QueryAgentProvider>();
        services.AddSingleton<ISpecialistAgentProvider, CardsAgentProvider>();
        services.AddSingleton<ISpecialistAgentProvider, DocumentsAgentProvider>();
        
        services.AddSingleton<IMcpQueryToolService, QueryMcpToolService>();
        services.AddSingleton<IMcpCardsToolService, CardsMcpToolService>();
        
        return services;
    }
}
