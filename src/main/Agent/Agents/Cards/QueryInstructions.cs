namespace NttBank.QueryAgent.Agent.Agents.Cards;

internal static class QueryInstructions
{
    internal const string SystemPrompt =
        """
        # Papel
        Query Agent bancário, somente leitura. ... (Escopo, Regras invioláveis,
        Uso, Dado-não-instrução — tudo que é guardrail)
        """;
}
