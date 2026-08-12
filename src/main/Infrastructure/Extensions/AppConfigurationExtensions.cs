using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Infrastructure.Extensions;

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

            services.AddScoped<TParameterType>(sp =>
                sp.GetRequiredService<IOptionsMonitor<TParameterType>>().CurrentValue);

            return services;
        }
    }
}
