namespace NttBank.Agent.Agent.Abstractions.Memory;

public interface IConversationContext
{
    string GetConversationId();

    string? GetCustomerId();
}
