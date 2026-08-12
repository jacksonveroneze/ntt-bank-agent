using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            var options = sp.GetRequiredService<QueryAgentOptions>();
            var chatClient = sp.GetRequiredKeyedService<IChatClient>(options.Provider);
            var env = sp.GetRequiredService<IHostEnvironment>();

            return new Agents.Query.QueryAgent(
                QueryAgentBuilder.Build(chatClient, options, env));
        });

        return services;
    }
}
