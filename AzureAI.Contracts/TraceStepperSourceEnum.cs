using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.Contracts
{
    public enum TraceStepperSourceEnum
    {
        Unknown = 0,
        BlazorUi = 1,
        WebApi = 2,
    }

    public static class TraceStepperSourceEnumName
    {
        public static string ToDisplayName(this TraceStepperSourceEnum source)
        {
            return source switch
            {
                TraceStepperSourceEnum.BlazorUi => "Blazor UI",
                TraceStepperSourceEnum.WebApi => "Web API",
                _ => "Unknown"
            };
        }
    }
}
