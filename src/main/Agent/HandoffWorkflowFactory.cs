using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Extensions;

namespace NttBank.QueryAgent.Agent;

public static class HandoffWorkflowFactory
{
    public static async Task<AIAgent> BuildAsync(
        IEnumerable<IAgentProvider> providers,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(nameof(HandoffWorkflowFactory));

        try
        {
            var agentProviders = providers as IAgentProvider[] ?? [.. providers];

            var agentTriageProvider = agentProviders
                .First(conf => !conf.IsSpecialist);

            var agentsSpecialistsProviders = agentProviders
                .Where(conf => conf.IsSpecialist)
                .ToArray();

            if (agentsSpecialistsProviders.Length is 0)
            {
                throw new InvalidOperationException(
                    "Nenhum especialista registrado para handoff.");
            }

            logger.HandoffBuilding(
                agentTriageProvider.Name,
                agentsSpecialistsProviders.Length);

            var triageAgentTask = agentTriageProvider
                .GetAsync(cancellationToken);

            var tasks = agentsSpecialistsProviders.Select(async agent =>
                await agent.GetAsync(cancellationToken));

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
        catch (Exception ex)
        {
            logger.HandoffBuildFailed(ex);
            throw;
        }
    }
}
