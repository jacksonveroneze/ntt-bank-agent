namespace NttBank.Agent.Agent.Abstractions;

public record AgentInput(
    string Prompt,
    string? ConversationId);
