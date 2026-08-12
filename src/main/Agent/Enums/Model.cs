using System.ComponentModel;

namespace NttBank.QueryAgent.Agent.Enums;

public enum Model
{
    None = 0,

    [Description("gemma4")] 
    Gemma4 = 1,

    [Description("quen2.5-coder:7b")] 
    Quen25Coder7B = 2,

    [Description("gpt-40-mini")] 
    Gpt40Mini = 3,
}
