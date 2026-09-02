using System.Diagnostics.CodeAnalysis;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using NttBank.Agent.Infrastructure.Mappers;

namespace NttBank.Agent.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class MapperExtensions
{
    public static IServiceCollection AddMapper(
        this IServiceCollection services)
    {
        services.AddSingleton<TypeAdapterConfig>(_ =>
        {
            var config = new TypeAdapterConfig();
            new RagMapper().Register(config);
            return config;
        });

        services.AddSingleton<IMapper, ServiceMapper>();

        return services;
    }
}
