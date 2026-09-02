using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Agents.Triage;
using NttBank.Agent.Agent.Extensions;

namespace NttBank.Agent.Agent;

public static class HandoffWorkflowFactory
{
    public const string AgentName = "handoff";

    public static async Task<AIAgent> BuildAsync(
        ITriageAgentProvider agentTriageProvider,
        IReadOnlyCollection<ISpecialistAgentProvider> agentsSpecialistsProviders,
        ILoggerFactory loggerFactory,
        IHostEnvironment environment,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(nameof(HandoffWorkflowFactory));

        if (agentsSpecialistsProviders.Count is 0)
        {
            throw new InvalidOperationException(
                "Nenhum especialista registrado para handoff.");
        }

        logger.HandoffBuilding(
            agentTriageProvider.Name,
            agentsSpecialistsProviders.Count);

        var triageAgentTask = agentTriageProvider
            .CreateAsync(cancellationToken);

        var tasks = agentsSpecialistsProviders.Select(async agent =>
            await agent.CreateAsync(cancellationToken));

        var agentsSpecialists = await Task.WhenAll(tasks);

        var triageAgent = await triageAgentTask;

        var workflow = AgentWorkflowBuilder
            .CreateHandoffBuilderWith(triageAgent)
            .WithHandoffs(triageAgent, agentsSpecialists)
            .WithHandoffs(agentsSpecialists, triageAgent)
            .Build();

        logger.HandoffBuilt();

        return workflow.AsAIAgent(
            id: $"id-{AgentName}",
            name: AgentName,
            includeExceptionDetails: environment.IsDevelopment(),
            includeWorkflowOutputsInResponse: false);
    }
}
