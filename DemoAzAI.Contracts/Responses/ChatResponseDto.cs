using DemoAzAI.Contracts.Dtos;

namespace DemoAzAI.Contracts.Responses
{
    public class ChatResponseDto
    {
        public ChatResponseDto(List<StepTracerStepDto> steps)
        {
            Steps.AddRange(steps);
        }

        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatisticsDto? ModelStatistics { get; set; }
        public List<StepTracerStepDto> Steps { get; set; } = new();
    }
}
