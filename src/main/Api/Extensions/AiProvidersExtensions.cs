using System.Diagnostics.CodeAnalysis;
using Anthropic;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Enums;
using NttBank.QueryAgent.Api.Configurations;
using OllamaSharp;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
internal static class AiProvidersExtensions
{
    public static IServiceCollection AddAiProviders(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        var providers = appConfiguration.Ai?.Providers;

        if (providers is null)
        {
            return services;
        }

        foreach ((Provider provider, AiProviderConfiguration cfg) in providers)
        {
            if (provider is Provider.None || !cfg.Enabled)
            {
                continue;
            }

            Register(services, provider, cfg);
        }

        return services;
    }

    private static void Register(
        IServiceCollection services,
        Provider provider,
        AiProviderConfiguration cfg)
    {
        services.AddKeyedSingleton<IChatClient>(provider, (_, _) =>
        {
            IChatClient inner = provider switch
            {
                Provider.Ollama => new OllamaApiClient(
                    new Uri(cfg.Endpoint!), cfg.Model!),

                Provider.Claude => new AnthropicClient
                {
                    ApiKey = cfg.ApiKey,
                }.AsIChatClient(cfg.Model!),

                _ => throw new InvalidOperationException(
                    $"Provider {provider} not supported."),
            };

            return inner
                .AsBuilder()
                .UseOpenTelemetry(
                    sourceName: $"ai-{provider.ToString().ToLowerInvariant()}",
                    configure: c => c.EnableSensitiveData = cfg.EnableSensitiveData)
                .Build();
        });
    }
}
