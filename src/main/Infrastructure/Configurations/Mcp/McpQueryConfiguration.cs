using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations.Mcp;

[ExcludeFromCodeCoverage]
public sealed record McpQueryConfiguration : McpServerConfiguration
{
    public const string SectionName = "McpQuery";
}
