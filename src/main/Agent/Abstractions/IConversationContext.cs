namespace NttBank.Agent.Agent.Abstractions;

public interface IConversationContext
{
    string GetConversationId();

    string? GetCustomerId();
}
