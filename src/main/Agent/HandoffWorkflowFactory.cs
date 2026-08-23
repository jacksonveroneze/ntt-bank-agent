using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent;

public static class HandoffWorkflowFactory
{
    public static async Task<AIAgent> BuildAsync(
        IEnumerable<IAgentProvider> providers,
        CancellationToken cancellationToken)
    {
        var agentProviders = providers as IAgentProvider[] ?? [.. providers];

        var agentTriageProvider = agentProviders
            .First(conf => !conf.IsSpecialist);

        var agentsSpecialistsProviders = agentProviders
            .Where(conf => conf.IsSpecialist).ToArray();

        if (agentsSpecialistsProviders.Length is 0)
        {
            throw new InvalidOperationException(
                "Nenhum especialista registrado para handoff.");
        }

        var triageAgentTask = agentTriageProvider
            .GetAsync(cancellationToken);

        var tasks = agentsSpecialistsProviders.Select(async conf =>
            await conf.GetAsync(cancellationToken));

        var specialistsAgents = await Task.WhenAll(tasks);

        var triageAgent = await triageAgentTask;

        var workflow = AgentWorkflowBuilder
            .CreateHandoffBuilderWith(triageAgent)
            .WithHandoffs(triageAgent, specialistsAgents)
            .WithHandoffs(specialistsAgents, triageAgent)
            .Build();

        return workflow.AsAIAgent();
    }
}
