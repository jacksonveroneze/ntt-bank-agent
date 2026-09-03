using System.ComponentModel;
using Microsoft.Extensions.AI;

namespace NttBank.Agent.Agent.Agents.Accounts.Tools;

public class CreateAccountTool
{
    private const string ToolName = "create_account";
    private const string ToolDescription = "Cria uma nova conta para o usuário.";

    [Description(ToolDescription)]
    private Task<string> GetCreateAccountAsync(
        CancellationToken cancellationToken)
    {
        return Task.FromResult("A conta foi criada com sucesso!");
    }

    public AIFunction Build()
    {
        return AIFunctionFactory.Create(
            GetCreateAccountAsync,
            new AIFunctionFactoryOptions
            {
                Name = ToolName,
                Description = ToolDescription,
            });
    }
}
