using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace NttBank.QueryAgent.Agent.Factories;

public static class ChatClientAgentFactory
{
    public static ChatClientAgent Create(
        IChatClient chatClient,
        string name,
        string description,
        string modelId,
        float temperature,
        string instructions,
        IList<AITool>? tools = null)
    {
        var agent = new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = name,
                Description = description,
                ChatOptions = new ChatOptions
                {
                    ModelId = modelId,
                    Instructions = instructions,
                    Tools = tools,
                    ToolMode = ChatToolMode.Auto,
                    AllowMultipleToolCalls = false,
                    Temperature = temperature,
                },
            });

        return agent;
    }
}
