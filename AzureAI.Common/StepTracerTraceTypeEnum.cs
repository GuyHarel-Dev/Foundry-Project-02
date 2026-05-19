namespace AzureAI.Common
{
    public enum StepTracerTraceTypeEnum
    {
        Unknown = 0,
        BlazorUi = 1,
        WebApi = 2,
    }

    public static class TraceStepperSourceEnumName
    {
        public static string ToDisplayName(this StepTracerTraceTypeEnum source)
        {
            return source switch
            {
                StepTracerTraceTypeEnum.BlazorUi => "Blazor UI",
                StepTracerTraceTypeEnum.WebApi => "Web API",
                _ => "Unknown"
            };
        }
    }
}
