using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Triage;
using NttBank.QueryAgent.Agent.Extensions;

namespace NttBank.QueryAgent.Agent;

public static class HandoffWorkflowFactory
{
    public static async Task<AIAgent> BuildAsync(
        ITriageAgentProvider agentTriageProvider,
        IReadOnlyCollection<ISpecialistAgentProvider> agentsSpecialistsProviders,
        ILoggerFactory loggerFactory,
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

        return workflow.AsAIAgent();
    }
}
