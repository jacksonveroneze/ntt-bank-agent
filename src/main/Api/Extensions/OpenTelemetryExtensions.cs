using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Api.Configurations;
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
        private IOpenTelemetryBuilder AddMetrics()
        {
            builder.WithMetrics(options => options
                .AddMeter("query-agent")
                .AddProcessInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
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
                    .AddSource("query-agent");

                options.AddOtlpExporter(config => config.Endpoint =
                    appConfiguration.OpenTelemetry.EndpointTracing);
            });

            return builder;
        }
    }
}
