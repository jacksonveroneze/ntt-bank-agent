using System.ComponentModel.DataAnnotations;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Configurations;

public sealed class QueryAgentOptions
{
    public const string SectionName = "Ai:Agents:Query";

    public Provider Provider { get; init; } = Provider.None;

    [Required] 
    public string Model { get; init; } = null!;

    public float Temperature { get; init; }
}
