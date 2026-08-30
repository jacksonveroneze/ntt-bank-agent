namespace NttBank.QueryAgent.Agent.Abstractions;

public interface IConversationContext
{
    string GetConversationId();

    string? GetCustomerId();
}
