using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Agent.Agents.Cards;
using NttBank.QueryAgent.Agent.Agents.Query;
using NttBank.QueryAgent.Api.Configurations;
using NttBank.QueryAgent.Infra;
using NttBank.QueryAgent.Infra.Configurations;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AppConfigurationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAppConfigs(
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddConfiguration<AppConfiguration>(configuration);
            
            services.AddConfiguration<QueryAgentConfiguration>(
                configuration, QueryAgentConfiguration.SectionName); 
            
            services.AddConfiguration<CardsAgentConfiguration>(
                configuration, CardsAgentConfiguration.SectionName);
            
            services.AddConfiguration<McpOptions>(
                configuration, McpOptions.SectionName);

            return services;
        }

        private IServiceCollection AddConfiguration<TParameterType>(
            IConfiguration configuration,
            string? sectionName = null)
            where TParameterType : class
        {
            ArgumentNullException.ThrowIfNull(configuration);

            var section = string.IsNullOrEmpty(sectionName)
                ? configuration
                : configuration.GetSection(sectionName);

            services.AddOptions<TParameterType>()
                .Bind(section)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<TParameterType>(sp =>
                sp.GetRequiredService<IOptions<TParameterType>>().Value);

            return services;
        }
    }
}
