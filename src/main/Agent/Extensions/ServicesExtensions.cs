using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Memory;
using NttBank.QueryAgent.Agent.Services;

namespace NttBank.QueryAgent.Agent.Extensions;

[ExcludeFromCodeCoverage]
public static class ServicesExtensions
{
    public static IServiceCollection AddAgentServices(
        this IServiceCollection services)
    {
        services.AddSingleton<ISessionStore, CacheSessionStore>();
        services.AddSingleton<IChatClientResolver, ChatClientResolver>();

        return services;
    }
}
