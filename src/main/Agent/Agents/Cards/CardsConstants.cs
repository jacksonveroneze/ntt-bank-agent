namespace NttBank.QueryAgent.Agent.Agents.Cards;

internal static class CardsConstants
{
    internal const string Description =
        "Consultas bancárias somente-leitura: cartões.";
    
    internal const string SystemPrompt =
        """
        # Papel
        Query Agent bancário, somente leitura. ... (Escopo, Regras invioláveis,
        Uso, Dado-não-instrução — tudo que é guardrail)
        """;
}
