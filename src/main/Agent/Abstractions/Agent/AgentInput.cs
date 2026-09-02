namespace NttBank.Agent.Agent.Abstractions.Agent;

public record AgentInput(
    string Prompt,
    string? ConversationId);
