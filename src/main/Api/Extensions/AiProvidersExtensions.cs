using System.Diagnostics.CodeAnalysis;
using Anthropic;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Agents;
using NttBank.QueryAgent.Agent.Enums;
using NttBank.QueryAgent.Infrastructure.Configurations;
using OllamaSharp;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
internal static class AiProvidersExtensions
{
    public static IServiceCollection AddAiProviders(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.AddProvider(Provider.Ollama, appConfiguration)
            .AddProvider(Provider.Claude, appConfiguration);

        return services;
    }

    private static IServiceCollection AddProvider(
        this IServiceCollection services,
        Provider provider,
        AppConfiguration appConfiguration)
    {
        if (!TryGetEnabledProvider(
                appConfiguration.Ai!, provider,
                out var providerConfiguration))
        {
            return services;
        }

        services.AddKeyedSingleton<IChatClient>(provider, (_, _) =>
        {
            var chatClient = CreateChatClient(provider, providerConfiguration);

            return BuildPipeline(chatClient, provider, providerConfiguration);
        });

        return services;
    }

    private static IChatClient CreateChatClient(
        Provider provider,
        AiProviderConfiguration providerConfiguration)
    {
        return provider switch
        {
            Provider.Ollama => CreateOllamaChatClient(providerConfiguration),
            Provider.Claude => CreateClaudeChatClient(providerConfiguration),
            _ => throw new InvalidOperationException($"AI provider {provider} is not supported.")
        };
    }

    private static OllamaApiClient CreateOllamaChatClient(
        AiProviderConfiguration providerConfiguration)
    {
        var endpoint = new Uri(providerConfiguration.Endpoint!);

        var chatClient = new OllamaApiClient(
            endpoint,
            providerConfiguration.Model!);

        return chatClient;
    }

    private static IChatClient CreateClaudeChatClient(
        AiProviderConfiguration providerConfiguration)
    {
        using var anthropicClient = new AnthropicClient
        {
            ApiKey = providerConfiguration.ApiKey,
        };

        return anthropicClient.AsIChatClient(
            providerConfiguration.Model!);
    }

    private static bool TryGetEnabledProvider(
        AiConfiguration aiConfiguration,
        Provider provider,
        out AiProviderConfiguration providerConfiguration)
    {
        providerConfiguration = null!;

        return aiConfiguration.Providers
                   .TryGetValue(provider.ToString(), out providerConfiguration!) &&
               providerConfiguration.Enabled;
    }

    private static IChatClient BuildPipeline(
        IChatClient chatClient,
        Provider provider,
        AiProviderConfiguration providerConfiguration)
    {
        return chatClient
            .AsBuilder()
            .UseOpenTelemetry(
                sourceName: $"ai-provider-{provider.ToString().ToLowerInvariant()}",
                configure: cfg => { cfg.EnableSensitiveData = providerConfiguration.EnableSensitiveData; })
            .Build();
    }
}
