namespace NttBank.QueryAgent.Agent.Abstractions;

public record AgentInput(
    string Prompt,
    string? ConversationId);
