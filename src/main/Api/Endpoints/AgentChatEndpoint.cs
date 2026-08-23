using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NttBank.QueryAgent.Agent;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Api.Endpoints.Extensions;

namespace NttBank.QueryAgent.Api.Endpoints;

internal static class AgentChatEndpoint
{
    public static IEndpointRouteBuilder MapAgentChatEndpoint(
        this IEndpointRouteBuilder app, IAgentProvider provider)
    {
        var group = app.MapGroup($"agents/v1/{provider.Name}")
            .WithTags(provider.Name);

        group.MapPost("chat", async (
                [FromServices] IValidator<AgentInput> validator,
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
                    provider, input, cancellationToken);

                return Results.Ok(output);
            })
            .WithName($"{provider.Name}:chat")
            .Produces<AgentOutput>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
