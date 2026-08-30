using System.Diagnostics.CodeAnalysis;

namespace NttBank.QueryAgent.Infrastructure.Configurations.Mcp;

[ExcludeFromCodeCoverage]
public sealed record McpCardsConfiguration : McpServerConfiguration
{
    public const string SectionName = "McpCards";
}
