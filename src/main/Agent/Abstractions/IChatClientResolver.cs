using Microsoft.Extensions.AI;
using NttBank.Agent.Agent.Enums;

namespace NttBank.Agent.Agent.Abstractions;

public interface IChatClientResolver
{
    IChatClient Resolve(Provider provider);
}
