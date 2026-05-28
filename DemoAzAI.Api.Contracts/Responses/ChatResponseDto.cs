using DemoAzAI.Api.Contracts.Dtos;

namespace DemoAzAI.Api.Contracts.Responses
{
    public class ChatResponseDto
    {
        public string ChatResponse { get; set; } = string.Empty;
        public ModelStatisticsDto? ModelStatistics { get; set; }
    }
}
