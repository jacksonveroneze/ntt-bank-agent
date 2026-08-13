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

            builder.ConfigureDefaultServices(appConfiguration, builder.Configuration);

            return builder;
        }

        private WebApplicationBuilder ConfigureDefaultServices(
            AppConfiguration appConfiguration,
            IConfiguration configuration)
        {
            builder.Services
                .AddHttpContextAccessor()
                .AddProblemDetails()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddAuthentication(appConfiguration)
                .AddAuthorization(appConfiguration)
                .AddValidatorsFromAssembly(typeof(WebApplicationBuilderExtensions).Assembly)
                .AddCorrelation()
                .AddApplicationServices()
                .AddOpenTelemetry(appConfiguration)
                .AddAiProviders(appConfiguration)
                .AddMcpAuthentication(configuration)
                .AddMcpToolProvider()
                .AddQueryAgent()
                .AddHealthChecks();

            return builder;
        }
    }
}
