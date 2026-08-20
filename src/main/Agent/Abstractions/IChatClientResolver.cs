using Microsoft.Extensions.AI;
using NttBank.QueryAgent.Agent.Enums;

namespace NttBank.QueryAgent.Agent.Abstractions;

internal interface IChatClientResolver
{
    IChatClient Resolve(Provider provider);
}
