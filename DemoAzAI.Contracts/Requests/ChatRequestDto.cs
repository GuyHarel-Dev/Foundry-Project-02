using DemoAzAI.Contracts.Dtos;

namespace DemoAzAI.Contracts.Requests
{
    public class ChatRequestDto
    {
        public ChatRequestDto(List<StepTracerStepDto> steps)
        {
            Steps = steps;
        }

        public string Prompt { get; set; } = string.Empty;
        public List<StepTracerStepDto> Steps { get; set; } = new();
    }
}
