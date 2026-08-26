using TextSearchResult = Microsoft.Agents.AI.TextSearchProvider.TextSearchResult;

namespace NttBank.QueryAgent.Agent.Rag;

public sealed class RagSearchAdapter
{
    private static readonly List<RagChunk> CardFeeChunks =
    [
        new(
            Content: "A anuidade do cartão de crédito é cobrada em 12 parcelas mensais " +
                     "iguais, lançadas na fatura. O valor varia conforme a modalidade do " +
                     "cartão e pode ser isento mediante gasto mínimo mensal definido em contrato.",
            DocumentName: "Tarifas de Cartão de Crédito",
            DocumentUrl: new Uri("https://docs.nttbank.com/tarifas/cartao-credito")),
        new(
            Content: "A taxa de juros do crédito rotativo incide quando o pagamento da " +
                     "fatura é inferior ao valor total. O rotativo vigora por até 30 dias, " +
                     "após os quais o saldo é parcelado automaticamente.",
            DocumentName: "Tarifas de Cartão de Crédito",
            DocumentUrl: new Uri("https://docs.nttbank.com/tarifas/cartao-credito")),
        new(
            Content: "O saque com cartão de crédito possui tarifa fixa por operação mais " +
                     "juros equivalentes aos do crédito rotativo, contados a partir da data do saque.",
            DocumentName: "Tarifas de Cartão de Crédito",
            DocumentUrl: new Uri("https://docs.nttbank.com/tarifas/cartao-credito")),
    ];

    private static readonly List<RagChunk> AccountClosureChunks =
    [
        new(
            Content: "O encerramento de conta pode ser solicitado pelo titular a qualquer " +
                     "momento, sem custo. A solicitação exige que a conta esteja com saldo " +
                     "zerado e sem pendências, como faturas em aberto ou limites utilizados.",
            DocumentName: "Encerramento de Conta",
            DocumentUrl: new Uri("https://docs.nttbank.com/contas/encerramento")),
        new(
            Content: "Após a solicitação, a conta entra em processo de encerramento por até " +
                     "30 dias, período em que débitos automáticos e lançamentos pendentes são " +
                     "liquidados. A conta é encerrada definitivamente ao fim desse prazo.",
            DocumentName: "Encerramento de Conta",
            DocumentUrl: new Uri("https://docs.nttbank.com/contas/encerramento")),
        new(
            Content: "Contas conjuntas exigem a anuência de todos os titulares para o " +
                     "encerramento. Produtos vinculados à conta, como cartões e investimentos, " +
                     "devem ser cancelados ou transferidos previamente.",
            DocumentName: "Encerramento de Conta",
            DocumentUrl: new Uri("https://docs.nttbank.com/contas/encerramento")),
    ];

    public Task<IEnumerable<TextSearchResult>> SearchAsync(
        string query, CancellationToken cancellationToken)
    {
        try
        {
            var ragResults = new List<RagChunk>();

            ragResults.AddRange(CardFeeChunks);
            ragResults.AddRange(AccountClosureChunks);

            var payload = new RagSearchResponse(Results: ragResults);

            var result = payload.Results?.Select(r =>
                new TextSearchResult
                {
                    Text = r.Content,
                    SourceName = r.DocumentName,
                    SourceLink = r.DocumentUrl?.ToString(),
                }) ?? [];

            // log

            return Task.FromResult(result);
        }
        catch
        {
            // log
            return Task.FromResult<IEnumerable<TextSearchResult>>([]);
        }
    }
}

// contratos da SUA API RAG (ajuste aos campos reais)
public sealed record RagSearchRequest(string Query, int TopK);

public sealed record RagSearchResponse(IReadOnlyList<RagChunk>? Results);

public sealed record RagChunk(string Content, string? DocumentName, Uri? DocumentUrl);
