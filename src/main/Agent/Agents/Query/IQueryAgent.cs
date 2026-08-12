using NttBank.QueryAgent.Agent.Abstractions;
using NttBank.QueryAgent.Agent.Agents.Query.Models;

namespace NttBank.QueryAgent.Agent.Agents.Query;

public interface IQueryAgent :
    IAgent<AgentInput, AgentOutput>;
