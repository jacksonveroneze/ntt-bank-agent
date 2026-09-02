using System.Diagnostics.CodeAnalysis;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NttBank.Agent.Agent;
using NttBank.Agent.Infrastructure.Configurations;
using NttBank.Agent.Infrastructure.Conversation;

namespace NttBank.Agent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AgentMemoryExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAgentMemory(
            AppConfiguration appConfiguration)
        {
            ArgumentNullException.ThrowIfNull(appConfiguration);

            var memoryConfiguration = appConfiguration.Ai!.AgentMemory;

            if (!memoryConfiguration.Enabled)
            {
                return services;
            }

            services.AddKeyedSingleton<AgentSessionStore, ValkeyAgentSessionStore>(
                HandoffWorkflowFactory.AgentName);

            return services;
        }
    }
}
