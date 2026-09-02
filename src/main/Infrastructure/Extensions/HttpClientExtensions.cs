using System.Diagnostics.CodeAnalysis;
using CorrelationId.HttpClient;
using Microsoft.Extensions.DependencyInjection;
using NttBank.Agent.Infrastructure.Configurations;
using NttBank.Agent.Infrastructure.HttpClients;
using Refit;

namespace NttBank.Agent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpClientExtensions
{
    public static IServiceCollection AddHttpClient(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.AddRefitClient<INttBankRagApi>()
            .ConfigureHttpClient(config =>
            {
                config.BaseAddress = appConfiguration.HttpClientRagNttBank.Address;
            })
            .AddCorrelationIdForwarding()
            .AddStandardResilienceHandler();

        return services;
    }
}
