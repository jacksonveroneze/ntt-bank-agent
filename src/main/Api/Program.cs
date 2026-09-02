using CorrelationId;
using FluentValidation;
using NttBank.Agent.Api.Extensions;
using NttBank.Agent.Api.Middlewares;
using NttBank.Agent.Infrastructure.Configurations;
using NttBank.Agent.Infrastructure.Extensions;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddAppConfigs(builder.Configuration);

var appConfiguration = builder.Configuration
    .Get<AppConfiguration>()!;

builder.AddLogger(appConfiguration);

builder.Services
    .AddAGUIServer()
    .AddAiProviders(appConfiguration)
    .AddApplicationServices()
    .AddMapper()
    .AddCached(appConfiguration)
    .AddAgentMemory(appConfiguration)
    .AddOpenTelemetry(appConfiguration)
    .AddMcpAuthentication(appConfiguration)
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

await app.MapAgentAsync();

await app.RunAsync();
