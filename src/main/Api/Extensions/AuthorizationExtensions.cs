using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Api.Extensions;

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
