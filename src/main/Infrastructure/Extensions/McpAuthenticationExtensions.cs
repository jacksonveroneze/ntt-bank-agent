using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class McpAuthenticationExtensions
{
    public static IServiceCollection AddMcpAuthentication(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(appConfiguration);

        var tokenManagement = services.AddClientCredentialsTokenManagement();

        RegisterMcpServer(appConfiguration.McpQuery);
        RegisterMcpServer(appConfiguration.McpCards);

        return services;

        void RegisterMcpServer(McpServerConfiguration configuration)
        {
            var tokenClientName = $"{configuration.Name}-token-client";

            tokenManagement.AddClient(tokenClientName, client =>
            {
                client.TokenEndpoint = configuration.OAuth.TokenEndpoint;
                client.ClientId = ClientId.Parse(configuration.OAuth.ClientId);
                client.ClientSecret = ClientSecret.Parse(configuration.OAuth.ClientSecret);

                if (!string.IsNullOrEmpty(configuration.OAuth.Scope))
                {
                    client.Scope = Scope.Parse(configuration.OAuth.Scope);
                }

                if (!string.IsNullOrWhiteSpace(configuration.OAuth.Audience))
                {
                    client.Parameters = new Parameters
                    {
                        { "audience", configuration.OAuth.Audience },
                    };
                }

                client.ClientCredentialStyle = ClientCredentialStyle.PostBody;
            });

            services.AddClientCredentialsHttpClient(
                    configuration.Name,
                    ClientCredentialsClientName.Parse(tokenClientName))
                .AcceptSelfSignedInDevelopment();
        }
    }
}

internal static class DevCertBypassExtensions
{
    public static IHttpClientBuilder AcceptSelfSignedInDevelopment(
        this IHttpClientBuilder builder)
    {
        return builder.ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
        {
            SslOptions = new SslClientAuthenticationOptions
            {
                RemoteCertificateValidationCallback = (_, _, _, _) => true,
            },
        });
    }
}
