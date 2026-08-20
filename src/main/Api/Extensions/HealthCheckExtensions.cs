using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NttBank.QueryAgent.Infra.Configurations;

namespace NttBank.QueryAgent.Api.Extensions;

internal static class HealthCheckExtensions
{
    private const string PathHealthStartup = "/health/startup";
    private const string PathHealthReady = "/health/ready";
    private const string PathHealthLive = "/health/live";

    public static IServiceCollection AddHealthCheck(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);

        return services;
    }

    public static WebApplication AddHealthCheckEndpoints(
        this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapHealthChecks(PathHealthStartup, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("startup"),
        });

        app.MapHealthChecks(PathHealthReady, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("ready"),
        });

        app.MapHealthChecks(PathHealthLive, new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live"),
        });

        return app;
    }
}
