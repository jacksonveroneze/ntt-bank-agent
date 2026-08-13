using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Services;

public interface IMcpToolService
{
    ValueTask<IList<AITool>> GetToolsAsync(
        CancellationToken cancellationToken);
}
