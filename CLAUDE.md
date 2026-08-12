# CLAUDE.md

Instruções globais para desenvolvimento neste repositório .NET/C#.

Valem para todo o repositório. Antes de alterar uma camada/projeto, leia também o `CLAUDE.md` local do diretório correspondente.

---

## 1. Papel do agente

Atue como desenvolvedor sênior .NET e arquiteto de software.

Priorize código simples, legível, seguro, testável, observável e fácil de manter.

Faça a menor mudança segura que resolva o problema.
Não introduza bibliotecas, frameworks, padrões arquiteturais ou grandes refatorações sem necessidade clara.

---

## 2. Stack e padrões do repositório

- .NET 10 e C# 14.
- Nullable reference types e implicit usings habilitados.
- Warnings, analyzers e code style tratados como erro.
- Central Package Management em `Directory.Packages.props`.
- Configurações globais em `Directory.Build.props`.
- Estilo em `.editorconfig`.
- APIs banidas em `BannedSymbols.txt`.
- Preferir `System.Text.Json`; não usar `Newtonsoft.Json`.
- Não usar `DateTime.Now` nem `DateTimeOffset.Now`; preferir UTC ou `IDateTimeProvider`.
- Não adicionar versões de pacotes diretamente nos `.csproj`.

---

## 3. Princípios gerais

- Prefira clareza em vez de esperteza.
- Prefira código explícito em vez de mágica.
- Mantenha métodos pequenos e focados.
- Mantenha classes coesas.
- Evite duplicação de regras de negócio.
- Evite abstrações prematuras.
- Evite overengineering.
- Preserve comportamento existente, salvo pedido contrário.
- Siga os padrões já existentes no projeto — **salvo quando divergirem de um `CLAUDE.md`**; nesse caso o `CLAUDE.md` prevalece (ver §13).
- Não misture feature, refatoração e formatação sem motivo.

Quando houver dúvida entre soluções válidas, escolha a mais simples de entender, testar e manter.

---

## 4. Idioma e nomenclatura

- Todo código deve usar nomes em inglês.
- Classes, métodos, propriedades, records, interfaces, enums, variáveis, namespaces, arquivos e pastas técnicas devem ser nomeados em inglês.
- Documentação, comentários explicativos e arquivos de especificações podem ser escritos em português.
- Não misturar nomes em português no código C#.
- Exemplos corretos: `Order`, `OrderMapping`, `GetOrdersByAssetUseCase`, `CheckOrdersByAssetTool`.
- Exemplos incorretos: `Ordem`, `MapeamentoOrdem`, `ConsultarOrdemPorAtivoUseCase`.

---

## 5. Arquitetura

Use Clean Architecture com separação clara de responsabilidades.

Dependências esperadas:

```text
Domain <- Application
Application <- Infrastructure
Domain      <- Infrastructure
Application <- Api
Infrastructure <- Api apenas para composition root/DI/configuração
```

Regras:

- Domain não depende de ASP.NET Core, EF Core, banco, cache, mensageria, REST, GraphQL, gRPC ou MCP.
- Application não depende de transporte, controllers, endpoints, gRPC services, GraphQL resolvers ou MCP tools.
- Infrastructure implementa detalhes técnicos definidos por abstrações de Application/Domain.
- Api adapta protocolos de entrada/saída e configura composição.
- Regras de negócio ficam em Domain ou Application.
- Endpoints, resolvers, gRPC services e MCP tools devem ser finos. Somente receber request, chamar useCase e devoler response.
- Frameworks e tecnologias externas ficam nas bordas.

---

## 6. Organização de projetos

Estrutura padrão:

```text
src/main/Api/
src/main/Application/
src/main/Domain/
src/main/Infrastructure/
tests/
```

Responsabilidades:

- Api: Minimal APIs, rotas, versionamento, OpenAPI/Scalar, auth HTTP, ProblemDetails e respostas.
- Application: use cases, inputs/outputs, validators, abstrações, parâmetros e orquestração.
- Domain: agregados, entidades, value objects, factories, policies, repositórios e invariantes.
- Infrastructure: EF Core, DbContexts, migrations, repositories, cache, Redis, observabilidade, auth técnica, DI e providers.
- CrossCutting: objetos, validadores, erros e utilitários compartilhados e independentes.

Não crie projetos, pastas ou camadas sem responsabilidade clara.

---

## 7. Padrões C#

- Use file-scoped namespaces e braces em blocos de controle.
- Use primary constructors para injeção de dependência (ex. IGetCustomerUseCase useCase).
- Use extension member blocks.
- Respeite limite de linha de 100 caracteres.
- Prefira var em vez de tipo explícito, conforme `.editorconfig`.
- Use nomes claros e orientados à intenção.
- Use guard clauses para entradas inválidas.
- Prefira imutabilidade quando fizer sentido.
- Use `record` para dados imutáveis simples.
- Use `class` para entidades e serviços com comportamento ou identidade.
- Classes/records `sealed` por padrão.
- Use o modificador de acesso mais restritivo possível.
- Evite estado global mutável, service locator, métodos longos e construtores com dependências demais.
- Não retorne `null` para representar erro de negócio.
- Não exponha entidades EF diretamente em contratos de API.

Nomenclatura:

- Interfaces usam prefixo `I`.
- Métodos assíncronos terminam com `Async`.
- `CancellationToken` deve se chamar `cancellationToken`.
- Não usar default no tipo CancellationToken.
- Testes devem descrever o comportamento esperado.

---

## 8. Async/Await

- Todo método assíncrono deve terminar com `Async`.
- Propague `CancellationToken` quando aplicável.
- Não use `.Result`, `.Wait()` ou `.GetAwaiter().GetResult()`.
- Não use `Task.Run` para esconder I/O bloqueante em server-side.
- Não use `async void`, exceto em event handlers.
- Não ignore tasks retornadas.
- Não engula cancelamento.
- Passe `CancellationToken` para EF Core, HTTP calls, Redis, cache, mensageria e operações longas.
- Use `Task` e `Task<T>` por padrão.
- Use `ValueTask` somente com motivo real de performance e entendimento das restrições.

Nunca faça sync-over-async em ASP.NET Core para não causar thread pool starvation.

---

## 9. Erros e validações

- Use Result Pattern para erros esperados de negócio/aplicação.
- Use exceções para falhas inesperadas, não para fluxo normal.
- Use FluentValidation para validação de input quando aplicável.
- Mantenha invariantes de negócio no Domain.
- Mapeie Result para HTTP/gRPC/MCP apenas na Api.
- Não exponha stack trace para clientes.
- Não engula exceções.
- Use constraints de banco para integridade, mas não dependa apenas delas para regras de negócio.

Erros esperados incluem validação, not found, conflito, regra violada,
estado inválido e operação não autorizada. Falhas inesperadas incluem banco indisponível,
timeout, erro de rede, bug e configuração inválida.

---

## 10. Logging, observabilidade e segurança

- Use logging estruturado com Serilog quando disponível.
- Inclua correlation id, trace id ou request id quando disponíveis.
- Use logs para diagnóstico, métricas para comportamento mensurável e traces para fluxo distribuído.
- Use health checks para dependências.
- Use OpenTelemetry/Prometheus quando disponível no projeto.
- Nunca commite secrets, `.env` com credenciais reais, connection strings reais, tokens ou certificados.
- Nunca hardcode credenciais.
- Valide toda entrada externa.
- Aplique autenticação/autorização de forma explícita.
- Prefira policy-based authorization em ASP.NET Core.
- Não confie em identificadores do cliente para autorização.
- Evite overposting, mass assignment e vazamento de detalhes internos.
- Não logue senhas, tokens, secrets, CPF, dados sensíveis ou payloads completos sensíveis.

Em decisões sensíveis, escolha o padrão mais seguro.

---

## 11. Anti-overengineering

Não adicione sem necessidade clara: generic repository, unit of work sobre EF Core sem valor claro,
mediator pipeline, CQRS completo, domain events, microservices, base classes genéricas,
factories desnecessárias, reflection desnecessária, abstrações com uma única implementação,
extension methods sem ganho claro ou frameworks internos.

Antes de criar uma abstração, valide se ela reduz acoplamento na camada correta,
facilita testes/manutenção e compensa a complexidade.

---

## 12. Fluxo de trabalho do agente

Antes de alterar, leia o código existente, o `CLAUDE.md` da raiz e o da camada/projeto alvo,
entenda os padrões atuais, identifique testes relacionados e faça plano breve para mudanças não triviais.

Quando o código existente divergir de um `CLAUDE.md`, o plano **deve listar cada divergência
como um alerta** antes de gerar código, no formato: "o arquivo X faz A, mas o `CLAUDE.md` pede B;
sigo B (ver §13)". Não gere código antes de enumerar essas divergências.

Durante a alteração, mantenha o escopo, reutilize padrões existentes, evite mudanças destrutivas,
evite novas dependências sem justificativa e atualize testes quando necessário.

Depois de alterar, rode build, testes e format quando disponíveis.
Informe o que mudou, comandos executados, riscos, limitações e pendências.

Não execute ações destrutivas sem autorização explícita.

## 13. Precedência / fonte da verdade

Quando houver divergência entre fontes, a ordem de autoridade é:

1. `/CLAUDE.md` (raiz) — regras globais.
2. `CLAUDE.md` da camada — especializa a raiz; prevalece sobre a raiz apenas
   no detalhe local que ela mesma define.
3. Código existente — referência para detalhes que os `CLAUDE.md` **não**
   especificam. **Não** é autoridade quando diverge deles.

Ferramentas obrigatórias (`.editorconfig`, analyzers, `BannedSymbols.txt`) são
restrição rígida — o build tem de passar. Um analyzer **desligado** (ex.
`CA1852 = none`) **não** libera ignorar o `CLAUDE.md`: significa apenas que a
regra não é enforced pela ferramenta; a diretriz do MD continua valendo.

Regra prática quando o código existente contradiz um `CLAUDE.md`:

- **Liste a divergência como alerta** — sempre. Toda divergência código↔MD
  detectada vira um alerta explícito para o usuário; nenhuma é resolvida em
  silêncio. Formato: "o arquivo X faz A, mas o `CLAUDE.md` pede B; sigo B."
- Siga o `CLAUDE.md` no código **novo**. O desvio no arquivo existente é débito,
  não padrão a replicar (ex.: um mapper existente `public class` sem `sealed`
  não autoriza novos mappers sem `sealed`).
- Não siga o código em silêncio nem "corrija" o código em silêncio.
- **Não** retrofite o arquivo divergente no mesmo passo — isso mistura escopo
  (§11/§12). Só corrija se pedido explicitamente; caso contrário, registre como
  pendência.
- Onde código e `CLAUDE.md` **não** conflitam, o exemplo-âncora segue valendo
  como referência de detalhe.

Isto vale para **convenções e diretrizes**. Não confunda com "preservar
comportamento existente" (§3): mudar convenção de código ≠ mudar comportamento
de runtime — este último continua exigindo pedido explícito.