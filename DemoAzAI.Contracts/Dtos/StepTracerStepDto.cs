namespace DemoAzAI.Contracts.Dtos
{
    public class StepTracerStepDto
    {
        public string StepTitle { get; set; } = string.Empty;
        public DateTime? StepDateTime { get; set; }
        public string StepSource { get; set; } = string.Empty;
    }
}
