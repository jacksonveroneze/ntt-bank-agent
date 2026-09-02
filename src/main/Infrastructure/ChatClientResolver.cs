using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using NttBank.Agent.Agent.Abstractions.Agent;
using NttBank.Agent.Agent.Enums;

namespace NttBank.Agent.Infrastructure;

internal sealed class ChatClientResolver(
    IServiceProvider serviceProvider) : IChatClientResolver
{
    public IChatClient Resolve(Provider provider)
    {
        return serviceProvider.GetRequiredKeyedService
            <IChatClient>(provider);
    }
}
