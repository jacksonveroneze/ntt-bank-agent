using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Services;

internal sealed class ChatClientResolver(
    IServiceProvider serviceProvider) : IChatClientResolver
{
    public IChatClient Resolve(Provider provider)
    {
        return serviceProvider.GetRequiredKeyedService
            <IChatClient>(provider);
    }
}
