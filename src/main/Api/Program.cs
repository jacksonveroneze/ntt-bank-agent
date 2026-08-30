using CorrelationId;
using FluentValidation;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.QueryAgent.Agent;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Api.Endpoints.Agents;
using NttBank.QueryAgent.Api.Extensions;
using NttBank.QueryAgent.Api.Middlewares;
using NttBank.QueryAgent.Infra.Configurations;
using NttBank.QueryAgent.Infra.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddAppConfigs(builder.Configuration);

var appConfiguration = builder.Configuration
    .Get<AppConfiguration>()!;

builder.AddLogger(appConfiguration);

builder.Services
    .AddAGUIServer()
    .AddAiProviders(appConfiguration)
    .AddMcpToolProvider()
    .AddApplicationServices()
    .AddCached(appConfiguration)
    .AddAgentMemory(appConfiguration)
    .AddOpenTelemetry(appConfiguration)
    .AddMcpAuthentication(builder.Configuration)
    .AddAppAuthentication(appConfiguration)
    .AddAuthorization(appConfiguration)
    .AddHttpClient(appConfiguration)
    .AddCors()
    .AddHttpContextAccessor()
    .AddProblemDetails()
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddCorrelation()
    .AddHealthCheck(appConfiguration);

var app = builder.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseStatusCodePages();

if (builder.Environment.IsDevelopment())
{
    app.UseCors(p => p.AllowAnyOrigin()
        .AllowAnyHeader().AllowAnyMethod());
}

app.UseRouting();
app.AddHealthCheckEndpoints();
app.UseOpenTelemetryPrometheusScrapingEndpoint("metrics");
app.UseAuthentication();
app.UseAuthorization();

var cancellationToken = app.Lifetime.ApplicationStopping;

var providers = app.Services.GetServices<IAgentProvider>();
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
var agent = await HandoffWorkflowFactory.BuildAsync(
    providers, loggerFactory, cancellationToken);

app.MapAGUIServer("/", agent);
app.MapAgentChatEndpoint(agent);

await app.RunAsync();
