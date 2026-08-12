using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Agent.Enums;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Agent.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentExtensions
{
    public static IServiceCollection AddAgent(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        services.AddScoped<IQueryAgent>(sp =>
        {
            var config = appConfiguration!.Ai!.Agents["query"];

            var provider = ResolveProvider(config);
            var model = ResolveModel(config, appConfiguration, provider);
            
            IChatClient chatClient = sp
                .GetRequiredKeyedService<IChatClient>(provider);
            
            var factory = new QueryAgentFactory(
                chatClient,
                model,
                config.Temperature ?? 0,
                sp.GetRequiredService<IHostEnvironment>());
            
            var agent= factory.Build();

            return new Agents.Query.QueryAgent(agent);
        });

        return services;
    }   
    
    private static Provider ResolveProvider(
        AiAgentConfiguration agentConfiguration)
    {
        var providerName = agentConfiguration.Provider;

        if (!Enum.TryParse(providerName, ignoreCase: true, out Provider provider) ||
            provider is Provider.None)
        {
            throw new InvalidOperationException(
                $"AI provider '{providerName}' is invalid for agent.");
        }

        return provider;
    }
    
    private static string ResolveModel(
        AiAgentConfiguration agentConfiguration,
        AppConfiguration appConfiguration,
        Provider provider)
    {
        if (!string.IsNullOrWhiteSpace(agentConfiguration.Model))
        {
            return agentConfiguration.Model;
        }

        var providerKey = provider.ToString();

        if (appConfiguration.Ai?.Providers
                .TryGetValue(providerKey, out var providerConfiguration) is true &&
            !string.IsNullOrWhiteSpace(providerConfiguration.Model))
        {
            return providerConfiguration.Model;
        }

        throw new InvalidOperationException(
            $"AI model was not configured for agent or provider '{providerKey}'.");
    }
}
