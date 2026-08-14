using FluentValidation;
using NttBank.QueryAgent.Agent.Extensions;
using NttBank.QueryAgent.Api.Configurations;
using NttBank.QueryAgent.Api.Middlewares;

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

            builder.Services
                .AddHttpContextAccessor()
                .AddProblemDetails()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddAppAuthentication(appConfiguration)
                .AddAuthorization(appConfiguration)
                .AddValidatorsFromAssembly(typeof(WebApplicationBuilderExtensions).Assembly)
                .AddCorrelation()
                .AddApplicationServices()
                .AddOpenTelemetry(appConfiguration)
                .AddAiProviders(appConfiguration)
                .AddMcpAuthentication(builder.Configuration)
                .AddMcpToolProvider()
                .AddQueryAgent()
                .AddHealthCheck(appConfiguration);

            return builder;
        }
    }
}
