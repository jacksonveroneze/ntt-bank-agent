using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Agents.Query;

namespace NttBank.QueryAgent.Agent.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentExtensions
{
    public static IServiceCollection AddQueryAgent(
        this IServiceCollection services)
    {
        services.AddSingleton<IQueryAgentProvider, QueryAgentProvider>();
        services.AddSingleton<IQueryAgent, Agents.Query.QueryAgent>();

        return services;
    }
}
