using FluentValidation;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Mvc;
using NttBank.QueryAgent.Agent;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Api.Endpoints.Extensions;

namespace NttBank.QueryAgent.Api.Endpoints.Agents;

internal static class AgentChatEndpoint
{
    public static IEndpointRouteBuilder MapAgentChatEndpoint(
        this IEndpointRouteBuilder app,
        AIAgent agent)
    {
        var group = app.MapGroup($"agents/v1/bank")
            .WithTags(agent.Name!);

        group.MapPost("chat", async (
                [FromServices] IValidator<AgentInput> validator,
                [FromServices] ILoggerFactory loggerFactory,
                [FromBody] AgentInput input,
                CancellationToken cancellationToken) =>
            {
                var validation = await validator
                    .ValidateAsync(input, cancellationToken);

                if (!validation.IsValid)
                {
                    return validation.ToValidationProblem();
                }

                var output = await AgentExecutor.RunAsync(
                    agent, input, loggerFactory, cancellationToken);

                return Results.Ok(output);
            })
            .WithName("bank:chat")
            .Produces<AgentOutput>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
