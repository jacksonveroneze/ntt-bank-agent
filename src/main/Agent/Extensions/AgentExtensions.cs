using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Agent.Configurations;

namespace NttBank.QueryAgent.Agent.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentExtensions
{
    public static IServiceCollection AddQueryAgent(
        this IServiceCollection services)
    {
        services.AddSingleton<IQueryAgent>(sp =>
        {
            var options = sp.GetRequiredService<
                IOptions<QueryAgentOptions>>().Value;

            var chatClient = sp.GetRequiredKeyedService<
                IChatClient>(options.Provider);
            
            var env = sp.GetRequiredService<IHostEnvironment>();

            var agent = QueryAgentFactory.Build(
                chatClient, options, env);
            
            return new Agents.Query.QueryAgent(agent);
        });

        return services;
    }
}
