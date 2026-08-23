using CorrelationId;
using FluentValidation;
using Microsoft.Agents.AI.Hosting.AGUI.AspNetCore;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Api.Endpoints;
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
    .AddCors()
    .AddHttpContextAccessor()
    .AddProblemDetails()
    .AddExceptionHandler<CustomExceptionHandler>()
    .AddAppAuthentication(appConfiguration)
    .AddAuthorization(appConfiguration)
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddCorrelation()
    .AddApplicationServices()
    .AddCached(appConfiguration)
    .AddOpenTelemetry(appConfiguration)
    .AddAiProviders(appConfiguration)
    .AddMcpAuthentication(builder.Configuration)
    .AddMcpToolProvider()
    .AddHealthCheck(appConfiguration);

var app = builder.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseRouting();
app.AddHealthCheckEndpoints();
app.UseOpenTelemetryPrometheusScrapingEndpoint("metrics");
app.UseAuthentication();
app.UseAuthorization();

var providers = app.Services.GetServices<IAgentProvider>().ToArray();

var ct = app.Lifetime.ApplicationStopping;

foreach (var p in providers)
{
    var agent = await p.GetAsync(ct);
    app.MapAGUIServer(p.Name, agent);
    app.MapAgentChatEndpoint(p);
}

await app.RunAsync();
