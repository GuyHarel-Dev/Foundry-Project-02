namespace AzureAI.Contracts.Dtos
{
    public sealed class ApiTraceStep
    {
        public string StepTitle { get; set; } = string.Empty;
        public DateTime? StepDateTime { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}
