using System.Diagnostics.CodeAnalysis;
using NttBank.QueryAgent.Infrastructure.Configurations;

namespace NttBank.QueryAgent.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLogger(
        this WebApplicationBuilder builder,
        AppConfiguration appConfiguration)
    {
        return builder;
    }
}
