using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetry(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);
        
        return services;
    }
}
