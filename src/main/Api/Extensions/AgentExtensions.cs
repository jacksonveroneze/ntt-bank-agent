using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.Agent.Agent;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Agents.Triage;
using NttBank.Agent.Api.Endpoints.Agents;

namespace NttBank.Agent.Api.Extensions;

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
            app.Environment,
            cancellationToken);

        app.MapAGUIServer("/", agent);
        app.MapAgentChatEndpoint(agent);

        return app;
    }
}
