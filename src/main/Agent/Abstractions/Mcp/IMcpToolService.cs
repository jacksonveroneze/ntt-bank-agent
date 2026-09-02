using Microsoft.Extensions.AI;

namespace NttBank.Agent.Agent.Abstractions.Mcp;

public interface IMcpToolService
{
    ValueTask<IList<AITool>?> GetToolsAsync(
        CancellationToken cancellationToken);
}
