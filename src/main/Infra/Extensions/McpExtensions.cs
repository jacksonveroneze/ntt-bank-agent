using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using NttBank.QueryAgent.Agent.Services;
using NttBank.QueryAgent.Infra.Configurations;

namespace NttBank.QueryAgent.Infra.Extensions;

[ExcludeFromCodeCoverage]
public static class McpExtensions
{
    public static IServiceCollection AddMcpToolProvider(
        this IServiceCollection services)
    {
        services.AddSingleton<IClientTransport>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<McpOptions>>().Value;
            
            var httpClient = sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(McpDefaults.HttpClientName);
            
            return new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = options.Endpoint,
                TransportMode = HttpTransportMode.AutoDetect,
            }, httpClient);
        });

        services.AddSingleton<IMcpQueryToolService, QueryMcpToolService>();

        return services;
    }
}
