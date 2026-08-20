using Microsoft.Extensions.Logging;
using NttBank.QueryAgent.Agent.Abstractions;

namespace NttBank.QueryAgent.Agent.Agents.Cards;

public sealed class CardsAgent(
    ILogger<CardsAgent> logger,
    ICardsAgentProvider provider) : ICardsAgent
{
    public async Task<AgentOutput> RunAsync(
        AgentInput input,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(input);

        logger.LogInformation("• Executando o agente de consulta com o prompt: {Prompt}",
            input.Prompt);

        var agent = await provider
            .GetAsync(cancellationToken);

        var response = await agent.RunAsync(
            message: input.Prompt,
            cancellationToken: cancellationToken);

        return AgentOutputMapper.ToOutput(response);
    }
}
