using FluentValidation;
using NttBank.QueryAgent.Agent.Extensions;
using NttBank.QueryAgent.Api.Middlewares;
using NttBank.QueryAgent.Infrastructure.Configurations;
using NttBank.QueryAgent.Infrastructure.Extensions;

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

            builder.ConfigureDefaultServices(appConfiguration);

            return builder;
        }

        private WebApplicationBuilder ConfigureDefaultServices(
            AppConfiguration appConfiguration)
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
                .AddAgent(appConfiguration)
                .AddHealthChecks();

            return builder;
        }
    }
}
