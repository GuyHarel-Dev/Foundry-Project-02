using DemoAzAI.Contracts.Dtos;

namespace DemoAzAI.Contracts.Responses
{
    public class ChatResponseDto
    {
        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatisticsDto? ModelStatistics { get; set; }
    }
}
