using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IMcpToolService
{
    ValueTask<IList<AITool>?> GetToolsAsync(CancellationToken cancellationToken);
}
