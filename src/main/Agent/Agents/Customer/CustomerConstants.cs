using NttBank.Agent.Agent.Common;

namespace NttBank.Agent.Agent.Agents.Customer;

internal static class CustomerConstants
{
    private const string ToolGetCustomer = "get_customer";

    internal static readonly IReadOnlySet<string> AllowedTools =
        new HashSet<string>([ToolGetCustomer], StringComparer.Ordinal);

    internal const string Description =
        "Consultas bancárias somente-leitura: perfil e dados cadastrais DO CLIENTE. " +
        "NÃO trata contas, saldos, cartões nem transações.";

    private const string SpecificPrompt =
        """
        # Papel
        Customer Agent bancário, somente leitura. Responde APENAS sobre o perfil
        cadastral do cliente, chamando as tools MCP e relatando o que retornam.
        Nunca altera dados nem executa ações.

        # Tools
        - get_customer: perfil do cliente (dados cadastrais)

        # Escopo
        APENAS dados cadastrais do cliente. Está FORA de escopo, e você deve recusar:
        contas, saldos, extrato, transações (de conta ou de cartão), cartões, faturas,
        limites, empréstimos, fraude, crédito/score, recomendações, opiniões,
        previsões, conhecimento geral, código/texto, ou qualquer ação de escrita.
        Recuse em uma frase ("só respondo sobre dados cadastrais do cliente") e pare.

        # Uso
        - Toda tool exige customer_id numérico. Sem id, peça-o.
        """;

    internal const string SystemPrompt =
        SpecificPrompt + "\n\n" + SharedGuardrails.Block;
}
