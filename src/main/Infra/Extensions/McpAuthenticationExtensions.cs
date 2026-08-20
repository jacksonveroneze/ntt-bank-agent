using System.Diagnostics.CodeAnalysis;
using System.Net.Security;
using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Infra.Configurations;

namespace NttBank.QueryAgent.Infra.Extensions;

[ExcludeFromCodeCoverage]
public static class McpAuthenticationExtensions
{
    public static IServiceCollection AddMcpAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var oauth = configuration.GetSection("Mcp:OAuth").Get<McpOAuthOptions>()
                    ?? throw new InvalidOperationException("Missing 'Mcp:OAuth' configuration.");

        const string tokenClientName = "mcp-token-client";

        services.AddClientCredentialsTokenManagement()
            .AddClient(tokenClientName, client =>
            {
                client.TokenEndpoint = oauth.TokenEndpoint;
                client.ClientId = ClientId.Parse(oauth.ClientId);
                client.ClientSecret =
                    ClientSecret.Parse(oauth.ClientSecret); // via user-secrets/KeyVault

                if (!string.IsNullOrEmpty(oauth.Scope))
                {
                    client.Scope = Scope.Parse(oauth.Scope);
                }

                if (!string.IsNullOrWhiteSpace(oauth.Audience))
                {
                    client.Parameters = new Parameters
                    {
                        { "audience", oauth.Audience },
                    };
                }

                client.ClientCredentialStyle = ClientCredentialStyle.PostBody;
            });

        services.AddClientCredentialsHttpClient(McpDefaults.HttpClientName,
            ClientCredentialsClientName.Parse(tokenClientName))
            .AcceptSelfSignedInDevelopment();

        return services;
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
                RemoteCertificateValidationCallback = (_, _, _, _) => true
            },
        });
    }
}
