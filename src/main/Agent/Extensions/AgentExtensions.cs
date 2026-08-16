using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Agent.Configurations;
using NttBank.QueryAgent.Agent.Memory;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentExtensions
{
    public static IServiceCollection AddQueryAgent(
        this IServiceCollection services)
    {
        services.AddSingleton<IQueryAgentProvider>(sp =>
        {
            var options = sp.GetRequiredService<
                IOptions<QueryAgentConfiguration>>().Value;

            var chatClient = sp.GetRequiredKeyedService<
                IChatClient>(options.Provider);

            var env = sp.GetRequiredService<IHostEnvironment>();

            var toolProvider = sp.GetRequiredService<IMcpToolService>();

            var agent = new QueryAgentProvider(
                chatClient, options, env, toolProvider);

            return agent;
        });

        services.AddSingleton<IQueryAgent, Agents.Query.QueryAgent>();

        services.AddSingleton<ISessionStore, CacheSessionStore>();

        return services;
    }
}
