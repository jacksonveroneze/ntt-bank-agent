namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IAgent<in TData, TResponse>
    where TData : AgentInput
    where TResponse : AgentOutput
{
    Task<TResponse> RunAsync(
        TData data,
        CancellationToken cancellationToken);
}
