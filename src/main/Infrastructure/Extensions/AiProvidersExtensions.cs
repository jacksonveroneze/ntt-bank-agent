using System.Diagnostics.CodeAnalysis;
using System.Net;
using Anthropic;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NttBank.Agent.Agent.Enums;
using NttBank.Agent.Infrastructure.Configurations;
using OllamaSharp;
using OpenAI;

namespace NttBank.Agent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class AiProvidersExtensions
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
        services.AddKeyedSingleton<IChatClient>(provider, (sp, _) =>
        {
            var env = sp.GetRequiredService<IHostEnvironment>();

            IChatClient inner = provider switch
            {
                Provider.Ollama => new OllamaApiClient(
                    new Uri(cfg.Endpoint!), cfg.Model!),

                Provider.Claude => new AnthropicClient
                {
                    ApiKey = cfg.ApiKey,
                    HttpClient = GetHttpClientProxy(),
                }.AsIChatClient(cfg.Model!),

                Provider.OpenAi => new OpenAIClient(cfg.ApiKey!)
                    .GetChatClient(cfg.Model!)
                    .AsIChatClient(),

                _ => throw new InvalidOperationException(
                    $"Provider {provider} not supported."),
            };

            return inner
                .AsBuilder()
                .UseOpenTelemetry(
                    sourceName: provider.ToString().ToLowerInvariant(),
                    configure: c => c.EnableSensitiveData =
                        cfg.EnableSensitiveData && env.IsDevelopment())
                .Build();
        });
    }

    private static HttpClient GetHttpClientProxy()
    {
        var proxy = new WebProxy("http://localhost:1457")
        {
            BypassProxyOnLocal = true 
        };

        var handler = new HttpClientHandler();
        handler.Proxy = proxy;
        handler.UseProxy = true;

        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            
        handler.ServerCertificateCustomValidationCallback =
            (_, _, _, _) => true;

        return new HttpClient(handler);
    }
}
