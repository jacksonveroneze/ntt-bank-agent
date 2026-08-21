using CorrelationId;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.QueryAgent.Agent.Agents.Cards;
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

        app.UseCors(p =>
            p.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

        app.UseRouting();

        app.AddHealthCheckEndpoints();
        app.UseOpenTelemetryPrometheusScrapingEndpoint(PathMetrics);

        app.UseAuthentication();
        app.UseAuthorization();

        var ct = app.Lifetime.ApplicationStopping;

        var agents = app.MapGroup(string.Empty);

        var queryAgent = app.Services
            .GetRequiredService<IQueryAgentProvider>().GetAsync(ct).Result;

        agents.MapAGUIServer("/", queryAgent);

        app.AddQueryAgentEndpoints();

        return app;
    }
}
