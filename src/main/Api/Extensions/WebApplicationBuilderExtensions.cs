using FluentValidation;
using NttBank.QueryAgent.Api.Configurations;
using NttBank.QueryAgent.Api.Middlewares;
using NttBank.QueryAgent.Infra;
using NttBank.QueryAgent.Infra.Extensions;

namespace NttBank.QueryAgent.Api.Extensions;

internal static class WebApplicationBuilderExtensions
{
    extension(WebApplicationBuilder builder)
    {
        public WebApplicationBuilder Configure()
        {
            builder.Services.AddAppConfigs(builder.Configuration);

            var appConfiguration = builder.Configuration
                .Get<AppConfiguration>()!;

            //builder.AddLogger(appConfiguration);

            builder.Services.AddAGUIServer();

            builder.Services
                .AddHttpContextAccessor()
                .AddProblemDetails()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddAppAuthentication(appConfiguration)
                .AddAuthorization(appConfiguration)
                .AddValidatorsFromAssembly(typeof(WebApplicationBuilderExtensions).Assembly)
                .AddCorrelation()
                .AddApplicationServices()
                .AddCached(appConfiguration)
                .AddOpenTelemetry(appConfiguration)
                .AddAiProviders(appConfiguration)
                .AddMcpAuthentication(builder.Configuration)
                .AddMcpToolProvider()
                .AddAgentServices()
                .AddHealthCheck(appConfiguration);

            return builder;
        }
    }
}
