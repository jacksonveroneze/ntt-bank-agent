using CorrelationId;
using NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1;

namespace NttBank.QueryAgent.Api.Extensions;

internal static class WebApplicationExtensions
{
    private const string PathMetrics = "metrics";
    
    public static WebApplication Configure(
        this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseCorrelationId();
        
        app.UseExceptionHandler();
        app.UseStatusCodePages();

        app.UseRouting();

        app.AddHealthCheckEndpoints();
        app.UseOpenTelemetryPrometheusScrapingEndpoint(PathMetrics);

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.AddQueryAgentEndpoints();

        return app;
    }
}
