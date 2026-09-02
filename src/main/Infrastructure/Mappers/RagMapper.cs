using Mapster;
using Microsoft.Agents.AI;
using NttBank.Agent.Infrastructure.Results;

namespace NttBank.Agent.Infrastructure.Mappers;

public sealed class RagMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        config.NewConfig<RagChunkResult, TextSearchProvider.TextSearchResult>()
            .Map(dest => dest.Text, src => src.Content)
            .Map(dest => dest.SourceName, src => src.DocumentName)
            .Map(dest => dest.SourceLink,
                src =>
                    src.DocumentUrl == null
                        ? string.Empty
                        : src.DocumentUrl.ToString());
    }
}
