using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Agents.Query.Models;

namespace NttBank.QueryAgent.Agent.Agents.Query.Middleware;

internal static class AgentGuardrails
{
    private static readonly string[] ForbiddenPatterns =
    [
        "compre", "comprar", "compra", "venda", "vende", "vender",
        "enviar ordem", "envie ordem", "coloca ordem", "coloque ordem",
        "cancele", "cancelar", "cancelamento",
        "altere", "alterar", "modificar", "modifique",
        "recomend", "devo comprar", "devo vender", "vale a pena",
        "carteira", "posição", "patrimônio",
        "provento", "dividendo",
        "chamado", "suporte", "reclamação",
        "perfil de risco", "alteração cadastral", "dados cadastrais",
        "ignore", "esqueça", "desconsidere", "nova instrução",
        "você agora é", "finja que", "aja como", "pretend",
        "ignore previous", "forget previous", "disregard"
    ];

    internal static Task<AgentResponse> ValidateAgentRunAsync(
        IEnumerable<ChatMessage> messages,
        AgentSession? session,
        AgentRunOptions? options,
        AIAgent innerAgent,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(messages);
        ArgumentNullException.ThrowIfNull(innerAgent);

        IEnumerable<ChatMessage> chatMessages = messages 
            as ChatMessage[] ?? messages.ToArray();
       
        var userMessage = chatMessages
            .LastOrDefault(message => message.Role == ChatRole.User)
            ?.Text ?? string.Empty;

        if (!HasForbiddenIntent(userMessage))
        {
            return innerAgent.RunAsync(
                chatMessages, session, options, cancellationToken);
        }

        AgentResponse response = new(
        [
            new ChatMessage(
                ChatRole.Assistant,
                DefaultAgentOutputMessages.SafeRefusalMessage),
        ]);

        return Task.FromResult(response);

    }

    private static bool HasForbiddenIntent(string userMessage)
    {
        if (string.IsNullOrWhiteSpace(userMessage))
        {
            return false;
        }

        return ForbiddenPatterns.Any(pattern =>
            userMessage.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }
}
