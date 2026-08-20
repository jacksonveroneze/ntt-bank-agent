using CorrelationId;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.QueryAgent.Agent.Agents.Query;
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

        var service = app.Services.GetRequiredService<IQueryAgentProvider>();

        var agent=service.GetAsync(CancellationToken.None).GetAwaiter().GetResult();
        
        app.MapAGUIServer("/", agent);
        app.AddQueryAgentEndpoints();

        return app;
    }
}
