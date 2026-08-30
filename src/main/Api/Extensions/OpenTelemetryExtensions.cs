using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Agent.Enums;
using NttBank.QueryAgent.Infrastructure.Configurations;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class OpenTelemetryExtensions
{
    private static readonly string[] ExclusionPathsTrace =
    [
        "/metrics",
        "/health",
        "/health/live",
        "/health/ready",
    ];
    
    public static IServiceCollection AddOpenTelemetry(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.Configure<AspNetCoreTraceInstrumentationOptions>(options =>
        {
            options.Filter = ctx => !ExclusionPathsTrace
                .Contains(ctx.Request.Path.Value);
        });

        services.AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .AddMetrics()
            .AddTracing(appConfiguration);

        return services;

        void ConfigureResource(ResourceBuilder r)
        {
            r.AddService(
                appConfiguration.Application.Name,
                serviceVersion: appConfiguration.Application.Version.ToString(),
                serviceInstanceId: Environment.MachineName);
        }
    }

    extension(IOpenTelemetryBuilder builder)
    {
        private IOpenTelemetryBuilder AddMetrics(
            )
        {
            builder.WithMetrics(options => options
                .AddProcessInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddMeter("Microsoft.Extensions.AI")
                .AddMeter("Microsoft.Agents.AI")
                .AddMeter("Claude")
                .AddMeter("OpenAi")
                .AddMeter("Query")
                .AddMeter("Cards")
                .AddMeter("Documents")
                .AddPrometheusExporter());

            return builder;
        }

        private IOpenTelemetryBuilder AddTracing(
            AppConfiguration appConfiguration)
        {
            if (appConfiguration.OpenTelemetry.EndpointTracing is null)
            {
                return builder;
            }

            builder.WithTracing(options =>
            {
                options
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSource("Microsoft.Agents.AI")
                    .AddSource("Microsoft.Extensions.AI");

                foreach ((Provider key, _) in appConfiguration?.Ai?.Providers
                             .Where(p => p.Value.Enabled)!)
                {
                    options.AddSource(key!.ToString().ToLowerInvariant());
                }
  
                options.AddOtlpExporter(config => config.Endpoint =
                    appConfiguration.OpenTelemetry.EndpointTracing);
            });

            return builder;
        }
    }
}
