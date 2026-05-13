namespace AzureAI.Common
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
