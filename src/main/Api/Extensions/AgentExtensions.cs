using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.QueryAgent.Agent;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Triage;
using NttBank.QueryAgent.Api.Endpoints.Agents;

namespace NttBank.QueryAgent.Api.Extensions;

internal static class AgentExtensions
{
    public static async Task<WebApplication> MapAgentAsync(
        this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var cancellationToken = app.Lifetime.ApplicationStopping;

        var triageProvider = app.Services.GetService<ITriageAgentProvider>();

        var specialistProviders = app.Services
            .GetServices<ISpecialistAgentProvider>()
            .ToArray();

        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

        var agent = await HandoffWorkflowFactory.BuildAsync(
            triageProvider!,
            specialistProviders,
            loggerFactory,
            cancellationToken);

        app.MapAGUIServer("/", agent);
        app.MapAgentChatEndpoint(agent);

        return app;
    }
}
