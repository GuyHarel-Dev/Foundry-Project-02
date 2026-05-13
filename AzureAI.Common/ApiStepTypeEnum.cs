namespace AzureAI.Common
{
    public enum ApiStepTypeEnum
    {
        Unknown = 0,
        BlazorUi = 1,
        WebApi = 2,
    }

    public static class TraceStepperSourceEnumName
    {
        public static string ToDisplayName(this ApiStepTypeEnum source)
        {
            return source switch
            {
                ApiStepTypeEnum.BlazorUi => "Blazor UI",
                ApiStepTypeEnum.WebApi => "Web API",
                _ => "Unknown"
            };
        }
    }
}
