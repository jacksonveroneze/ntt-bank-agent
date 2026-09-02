using System.Diagnostics.CodeAnalysis;
using NttBank.Agent.Infrastructure.Configurations;

namespace NttBank.Agent.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AuthorizationExtensions
{
    public static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        AppConfiguration appConfiguration)
    {
        ArgumentNullException.ThrowIfNull(appConfiguration);

        services.AddAuthorization();
        
        return services;
    }
}
