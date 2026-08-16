using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Api.Endpoints.Extensions;

namespace NttBank.QueryAgent.Api.Endpoints.Agents.Query.v1;

internal static class QueryAgentEndpoint
{
    private const string Resource = "query";

    public static WebApplication AddQueryAgentEndpoints(
        this WebApplication app)
    {
        RouteGroupBuilder builder =
            app.MapGroup("agents/v1/" + Resource)
                .WithTags(Resource)
                .RequireAuthorization();

        builder.AddChatAgentEndpoint();

        return app;
    }

    private static RouteGroupBuilder AddChatAgentEndpoint(
        this RouteGroupBuilder builder)
    {
        builder.MapPost("chat", async (
                [FromServices] IQueryAgent agent,
                [FromServices] IValidator<AgentInput> validator,
                [FromBody] AgentInput input,
                CancellationToken cancellationToken) =>
            {
                var validationResult = await validator
                    .ValidateAsync(input, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return validationResult
                        .ToValidationProblem();
                }

                var output = await agent.RunAsync(
                    input, cancellationToken);

                return Results.Ok(output);
            })
            .Produces<AgentOutput>(
                statusCode: StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        return builder;
    }
}
