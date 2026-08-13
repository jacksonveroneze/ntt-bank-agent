using CorrelationId;
using NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1;

namespace NttBank.QueryAgent.Api.Extensions;

internal static class WebApplicationExtensions
{
    private const string PathHealth = "/health";
    private const string PathMetrics = "metrics";
    
    public static WebApplication Configure(
        this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseCorrelationId();
        
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.UseRouting();

        app.UseHealthChecks(PathHealth);
        app.UseOpenTelemetryPrometheusScrapingEndpoint(PathMetrics);

        app.AddOrdersEndpoints();

        return app;
    }
}
